namespace TibberTask.Models
{
    public class Line
    {
        public Line(int l, int h)
        {
            Low = l;
            High = h;
        }
        public int Low { get; set; }
        public int High { get; set; }
    }
}
