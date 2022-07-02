using System.Collections;
using TibberTask.Models;


namespace TibberTask.Helpers
{
    public class RobotHelper
    {
        public int CalculateResult(ExecutionRequest Req)
        {
            (int, int )Point = (Req.Start.X, Req.Start.Y);
            
            List<(int, int)> Points = new();
            Points.Add(Point);
            foreach (var command in Req.Commands)
            {
                //(Points, Point) = GeneratePointsFromSteps(Points, Point, command.Steps, command.Direction);                                
            }                        
            return Points.Count;
        }        


        //public (List<(int, int)>, (int, int)) GeneratePointsFromSteps(List<(int, int)>Points, (int, int) Point, int Steps, string Direction)
        //{
            
        //    switch (Direction)
        //    {
        //        case "north":
        //            var GeneratedPoints = Enumerable.Range(1, Steps).Select(i => (Point.Item1, Point.Item2 + i)).ToList();                          
        //            Points.UnionWith(GeneratedPoints);
        //            Point.Item2 += Steps;
        //            break;
        //        case "south":
        //            GeneratedPoints = Enumerable.Range(1, Steps).Select(i => (Point.Item1, Point.Item2 - i)).ToList();                    
        //            Points.UnionWith(GeneratedPoints);
        //            Point.Item2 -= Steps;
        //            break;
        //        case "east":
        //            GeneratedPoints = Enumerable.Range(1, Steps).Select(i => (Point.Item1 + i, Point.Item2)).ToList();
        //            Points.UnionWith(GeneratedPoints);
        //            Point.Item1 += Steps;
        //            break;
        //        //Direction is west
        //        default:
        //            GeneratedPoints = Enumerable.Range(1, Steps).Select(i => (Point.Item1 - i, Point.Item2)).ToList();                                       
        //            Points.UnionWith(GeneratedPoints);
        //            Point.Item1 -= Steps;
        //            break;
        //    }   
            
        //    return (Points, Point);
        //}

        public int CalculateResultAdv(ExecutionRequest Req)
        {
            int Result = 0;
            //TODO:            
            //  Better check intersections
            Hashtable XX = new();
            Hashtable YY = new();
            List<int> XXKeys = new();
            List<int> YYKeys = new();
            Point CurrentPosition = Req.Start;            
            for (int i = 0; i<Req.Commands.Count; i++)
            {
                (CurrentPosition, int Low, int High, char Axis) = CreateNewline(CurrentPosition, Req.Commands[i]);
                if (Axis == 'y')
                {
                    (YY, YYKeys, int newPoints) = AddNewLine(YY, YYKeys, Low, High, CurrentPosition.X, Req.Commands[i].Steps);
                    Result += newPoints;

                }
                else
                {
                    (XX, XXKeys, int newPoints) = AddNewLine(XX, XXKeys, Low, High, CurrentPosition.Y, Req.Commands[i].Steps);
                    Result += newPoints;
                }                                
            }
                Result -= CheckIntersections(XX, YY, XXKeys, YYKeys);
            return Result;
        }
        public (Point, int, int, char) CreateNewline(Point point, Command command)
        {
            int Low;
            int High;
            char Axis;
            switch (command.Direction)
            {
                case "north":
                    Axis = 'y';
                    Low = point.Y;
                    point.Y += command.Steps;
                    High = point.Y;
                    break;
                case "south":
                    Axis = 'y';
                    High = point.Y;
                    point.Y -= command.Steps;
                    Low = point.Y;                                        
                    break;
                case "east":
                    Axis = 'x';
                    Low = point.X;
                    point.X += command.Steps;
                    High = point.X;
                    break;
                default:
                    //Direction is west
                    Axis = 'x';
                    High = point.X;                                       
                    point.X -= command.Steps;
                    Low = point.X;
                    break;
            }
            
            return (point, Low, High, Axis);
        }
        public int CheckIntersections(Hashtable XX, Hashtable YY, List<int> XXKeys, List<int> YYKeys)
        {
		int intersections = 0;
            int[] xKeys = XXKeys.ToArray();
            for (int i = 0; i < xKeys.Length; i++)
            {
                List<Line> xx = (List<Line>)XX[xKeys[i]];
                Line[] XLines = xx.ToArray();
                for (int j = 0;j<XLines.Length; j++)
                {
                    for(int k =XLines[j].Low; k<=XLines[j].High; k++)
                    {
                        if (YYKeys.Contains(k))
                        {
                            List<Line> YLines = (List<Line>) YY[k];
                            for(int a = 0; a < YLines.Count; a++)
                            {
                                if (YLines[a].High >= xKeys[i] && xKeys[i] >= YLines[a].Low)
                                {
                                    intersections++;
                                    break;
                                }
                            }
                        }
                    }                    
                }
            }        
		return intersections;
        }
        public (Hashtable, List<int>, int) AddNewLine(Hashtable line, List<int> Keys, int Low, int High, int P, int Steps)
        {
            int Count = Steps + 1;
		    if(line.ContainsKey(P)){
                //Line exists after last line
                List<Line> l = (List<Line>) line[P];
                if(Low>l[l.Count - 1].High){                    
				    l.Add(new Line(Low, High));
			    }
			    else if(High<l[0].Low){
				    l.Insert(0, new Line(Low, High));                    
                }
			    else{
                    int L = 0, H = 0, PrevCount = 0;             
				    for(int i = 0; i<l.Count;i++){

					    if(l[i].Low>High){
						    break;
					    }
					    if(l[i].High<Low){
						    L = i + 1;
                            continue;
					    }
					    if(l[i].High>=High && l[i].Low<=Low){
						    return (line,Keys,0);
					    }
					    PrevCount += Math.Abs(l[i].High - l[i].Low) + 1;
					    H = i;
				    }
				    if(l[L].Low<Low){
					    Low = l[L].Low;
				    }
				    if(l[H].High>High){
					    High = l[H].High;
				    }

				    l.RemoveRange(L, (H-L)+1);
				    l.Insert(L, new Line(Low, High));
                    
                    Count = Math.Abs(High-Low) + 1 - PrevCount;
			    }
		    }
		    else{
			    line[P] = new List<Line>{new(Low, High) };
			    Keys.Add(P);
                
            }
                return (line,Keys,Count);
        }

    }

}
