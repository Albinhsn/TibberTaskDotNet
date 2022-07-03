using System.Collections;
using System.Diagnostics;
using TibberTask.Models;


namespace TibberTask.Helpers
{
    public class RobotHelper
    {                
        public int CalculateResult(ExecutionRequest Req)
        {
            if (Req.commands.Count == 0)
            {
                return 1;
            }
            int Result = 0;    
            //Lines are stored with position on the other axis as key, then a list<(int, int)> containing the lines and their lowest and highest point 
            Hashtable XX = new();
            List<int> XXKeys = new();
            Hashtable YY = new();            
            List<int> YYKeys = new();
            Point CurrentPosition = Req.start;            

            //Iterate over the given commands
            //Calculate the position, the low and high points and the axis of the new line
            for (int i = 0; i<Req.commands.Count; i++)
            {
                (CurrentPosition, int Low, int High, char Axis) = CreateNewline(CurrentPosition, Req.commands[i]);
                
                //Add the new line and calculate the amount of new points it (creates on the same axis)
                if (Axis == 'y')
                {
                    (YY, YYKeys, int newPoints) = AddNewLine(YY, YYKeys, Low, High, CurrentPosition.x, Req.commands[i].steps);
                    Result += newPoints;
                }
                else
                {
                    (XX, XXKeys, int newPoints) = AddNewLine(XX, XXKeys, Low, High, CurrentPosition.y, Req.commands[i].steps);
                    Result += newPoints;
                }                                
            }
            //Calculate and remove the number of intersections between the axes
            Result -= CheckIntersections(XX, YY, XXKeys, YYKeys);
            return Result;
        }

        //Takes in the current position and the command
        //Returns the new position, the low and high of the new line and the axis for it
        public (Point, int, int, char) CreateNewline(Point point, command command)
        {
            int Low;
            int High;
            char Axis;
            switch (command.direction)
            {
                case "north":
                    Axis = 'y';
                    Low = point.y;
                    point.y += command.steps;
                    High = point.y;
                    break;
                case "south":
                    Axis = 'y';
                    High = point.y;
                    point.y -= command.steps;
                    Low = point.y;                                        
                    break;
                case "east":
                    Axis = 'x';
                    Low = point.x;
                    point.x += command.steps;
                    High = point.x;
                    break;
                default:
                    //Direction is west
                    Axis = 'x';
                    High = point.x;
                    point.x -= command.steps;
                    Low = point.x;
                    break;
            }
            
            return (point, Low, High, Axis);
        }

        //Checks how many times the lines on the different axes intersected
        //Returns the number of intersections
        public int CheckIntersections(Hashtable XX, Hashtable YY, List<int> XXKeys, List<int> YYKeys)
        {
		    int intersections = 0;    
            for(int i = 0; i<XXKeys.Count; i++)
            {
                List<(int, int)> xLines = (List<(int, int)>) XX[XXKeys[i]];
                for(int j = 0; j < xLines.Count; j++)
                {                 
                    List<int> FoundKeys = YYKeys.FindAll(y => xLines[j].Item2 >= y && y >= xLines[j].Item1);                            
                    for(int k = 0; k<FoundKeys.Count; k++)
                    {
                        List<(int, int)> yLine = (List<(int, int)>)YY[FoundKeys[k]];                                                
                        for(int a = 0; a < yLine.Count; a++)
                        {
                            if (yLine[a].Item2>=XXKeys[i] && XXKeys[i]>=yLine[a].Item1)
                            {
                                intersections++;                                
                                break;
                            }                            
                        }                        
                    }                    
                }
            }            
		    return intersections;
        }

        /*  Adds or inserts a new line to the existing lines
            Also adds a new key to keys if present
            Returns the new lines, a list of existing keys and the number of new points
         */
        public (Hashtable, List<int>, int) AddNewLine(Hashtable Lines, List<int> Keys, int Low, int High, int key, int Steps)
        {
            int Count = Steps + 1;
            if (Lines.ContainsKey(key))
            {
                List<(int, int)> line = (List<(int, int)>) Lines[key];
                //Line exist above every existing line
                if (Low > line[line.Count - 1].Item2)
                {
                    line.Add((Low, High));
                    Lines[key] = line;
                }
                //Line exist below every existing line
                else if (High < line[0].Item1)
                {
                    line.Insert(0, (Low, High));
                    Lines[key] = line;
                }
                //Line exist somewhere inbetween or inside existing ones
                else
                {
                    //Iterate over existing lines to find the location of the new line
                    int lowIdx = 0, highIdx = 0, prevCount = 0;
                    for(int i = 0; i<line.Count; i++)
                    {
                        if (line[i].Item1 > High)
                        {
                            break;
                        }
                        if (Low > line[i].Item2)
                        {
                            lowIdx = i + 1;
                            continue;
                        }
                        prevCount += Math.Abs(line[i].Item1 - line[i].Item2) + 1;
                        highIdx = i;
                    }
                    //Assign the low and high for the line 
                    if (line[lowIdx].Item1 < Low)
                    {
                        Low = line[lowIdx].Item1;
                    }
                    if (line[highIdx].Item2 > High)
                    {
                        High = line[highIdx].Item2;
                    }                   
                    //Deletes previous lines and inserts the combined one  
                    line.RemoveRange(lowIdx, (highIdx - lowIdx) + 1);
                    line.Insert(lowIdx, (Low, High));
                    //Count is difference between the new line and the amount of points from the previous line
                    Count = Math.Abs(High- Low) + 1 - prevCount;
                }
            }
            //No line existed with the given key
            else
            {
                Lines[key] = new List<(int, int)> { (Low, High) };
                Keys.Add(key);
            }            
            return (Lines, Keys, Count);
        }
    }

}
