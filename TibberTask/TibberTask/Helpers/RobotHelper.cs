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
            Hashtable XX = new();
            Hashtable YY = new();
            List<int> XXKeys = new();
            List<int> YYKeys = new();
            Point CurrentPosition = Req.start;            
            for (int i = 0; i<Req.commands.Count; i++)
            {
                (CurrentPosition, int Low, int High, char Axis) = CreateNewline(CurrentPosition, Req.commands[i]);
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
                Result -= CheckIntersections(XX, YY, XXKeys, YYKeys);
            return Result;
        }
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
        public int CheckIntersections(Hashtable XX, Hashtable YY, List<int> XXKeys, List<int> YYKeys)
        {
		    int intersections = 0;    
            for(int i = 0; i<XXKeys.Count; i++)
            {
                List<(int, int)> xLines = (List<(int, int)>) XX[XXKeys[i]];
                for(int j = 0; j < xLines.Count; j++)
                {
                    //Filter out Keys that isn't within xLines[j].Item1 and Item2
                    List<int> FoundYKeys = YYKeys.FindAll(y => xLines[j].Item2 >= y && y >= xLines[j].Item1);

                    //Iterate over YY with remaining keys.                     
                    for(int k = 0; k<FoundYKeys.Count; k++)
                    {
                        List<(int, int)> yLine = (List<(int, int)>)YY[FoundYKeys[k]];
                        
                        bool flag = false;
                        for(int a = 0; a < yLine.Count; a++)
                        {
                            if (yLine[a].Item2>=XXKeys[i] && XXKeys[i]>=yLine[a].Item1)
                            {
                                intersections++;
                                flag = true;
                                break;
                            }                            
                        }
                        
                    }
                    //If i is within yLines[k].Item1 and item2
                    //  Intersections ++; 
                    //  Break;
                }
            }            
		    return intersections;
        }
        public (Hashtable, List<int>, int) AddNewLine(Hashtable Lines, List<int> Keys, int Low, int High, int P, int Steps)
        {
            int Count = Steps + 1;
            if (Lines.ContainsKey(P))
            {
                List<(int, int)> line = (List<(int, int)>) Lines[P];
                if (Low > line[line.Count - 1].Item2)
                {
                    line.Add((Low, High));
                    Lines[P] = line;

                }
                else if (High < line[0].Item1)
                {
                    line.Insert(0, (Low, High));
                    Lines[P] = line;
                }
                else
                {
                    int l = 0, h = 0, prevCount = 0;
                    for(int i = 0; i<line.Count; i++)
                    {
                        if (line[i].Item1 > High)
                        {
                            break;
                        }
                        if (Low > line[i].Item2)
                        {
                            l = i + 1;
                            continue;
                        }
                        prevCount += Math.Abs(line[i].Item1 - line[i].Item2) + 1;
                        h = i;
                    }
                    if (line[l].Item1 < Low)
                    {
                        Low = line[l].Item1;
                    }
                    if (line[h].Item2 > High)
                    {
                        High = line[h].Item2;
                    }
                    
                    
                    line.RemoveRange(l, (h - l) + 1);
                    line.Insert(l, (Low, High));                    
                    Count = Math.Abs(High- Low) + 1 - prevCount;
                }

            }
            else
            {
                Lines[P] = new List<(int, int)> { (Low, High) };
                Keys.Add(P);
            }            
            return (Lines, Keys, Count);
        }
    }

}
