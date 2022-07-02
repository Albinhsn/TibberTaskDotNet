namespace TibberTask.Models
{
    public class Execution
    {
        public Execution(int id, DateTime timestamp, int commands, long result, double duration)
        {
            Id = id;
            Timestamp = timestamp;
            Commands = commands;
            Result = result;
            Duration = duration;
        }
        public int Id { get; set; }
        public DateTime Timestamp { get; set; }
        public int Commands { get; set; }
        public long Result { get; set; }
        public double Duration { get; set; }
           
        
    }
}
