using System.Drawing;
using TibberTask.Models;

namespace TibberTask.Helpers
{
    public class RobotHelper
    {
        public int CalculateResult(ExecutionRequest Req)
        {
            (int, int )Point = (Req.Start.X, Req.Start.Y);
            
            HashSet<(int, int)> Points = new();
            Points.Add(Point);
            foreach (var command in Req.Commands)
            {
                (Points, Point) = GeneratePointsFromSteps(Points, Point, command.Steps, command.Direction);                                
            }                        
            return Points.Count;
        }        


        public (HashSet<(int, int)>, (int, int)) GeneratePointsFromSteps(HashSet<(int, int)>Points, (int, int) Point, int Steps, string Direction)
        {
            
            switch (Direction)
            {
                case "north":
                    var GeneratedPoints = Enumerable.Range(1, Steps).Select(i => (Point.Item1, Point.Item2 + i)).ToList();                          
                    Points.UnionWith(GeneratedPoints);
                    Point.Item2 += Steps;
                    break;
                case "south":
                    GeneratedPoints = Enumerable.Range(1, Steps).Select(i => (Point.Item1, Point.Item2 - i)).ToList();                    
                    Points.UnionWith(GeneratedPoints);
                    Point.Item2 -= Steps;
                    break;
                case "east":
                    GeneratedPoints = Enumerable.Range(1, Steps).Select(i => (Point.Item1 + i, Point.Item2)).ToList();
                    Points.UnionWith(GeneratedPoints);
                    Point.Item1 += Steps;
                    break;
                //Direction is west
                default:
                    GeneratedPoints = Enumerable.Range(1, Steps).Select(i => (Point.Item1 - i, Point.Item2)).ToList();                    
                    Points.UnionWith(GeneratedPoints);
                    Point.Item1 -= Steps;
                    break;
            }
            
            return (Points, Point);
        }

        public int CalculateResultAdv(ExecutionRequest Req)
        {
            int Result = 0;
            Dictionary<char, Line> XX = new();
            Dictionary<char, Line> YY = new();

            return Result;
        }
        public ((int, int), int, int, char) CreateNewline((int, int) Point, Command command)
        {
            int Low;
            int High;
            char Axis;
            switch (command.Direction)
            {
                case "north":
                    Axis = 'y';
                    Low = Point.Item2;
                    Point.Item2 += command.Steps;
                    High = Point.Item2;
                    break;
                case "south":
                    Axis = 'y';
                    High = Point.Item2;
                    Point.Item2 -= command.Steps;
                    Low = Point.Item2;                                        
                    break;
                case "east":
                    Axis = 'x';
                    High = Point.Item1;
                    Point.Item1 += command.Steps;
                    Low = Point.Item1;
                    break;
                default:
                    //Direction is west
                    Axis = 'x';
                    High = Point.Item1;                                       
                    Point.Item1 -= command.Steps;
                    Low = Point.Item1;
                    break;
            }
            
            return (Point, Low, High, Axis);
        }
        public int CheckIntersection(Line LineToCheck, Line SameLine, int Low, int High, int P, string Direction)
        {
            int intersections = 0;


            return intersections;
        }
        public (Line, int) AddNewLine(Line Line, int Low, int High, int P, string Direction)
        {
            int Count = 0;


            return (Line, Count);
        }

    }

}
