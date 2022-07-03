﻿namespace TibberTask.Models
{
    public class Execution
    {
        public Execution(int id, DateTime timest, int com, long res, double dur)
        {
            id = id;
            timestamp = timest;
            commands = com;
            result = res;
            duration = dur;
        }
        public int id { get; set; }
        public DateTime timestamp { get; set; }
        public int commands { get; set; }
        public long result { get; set; }
        public double duration { get; set; }
           
        
    }
}
