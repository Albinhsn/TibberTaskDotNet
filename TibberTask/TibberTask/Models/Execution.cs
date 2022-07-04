using System.ComponentModel.DataAnnotations.Schema;

namespace TibberTask.Models
{
    public class execution
    {
        
        

        public int id { get; set; }        
        [Column("created_at")]
        public DateTime timestamp { get; set; }
        public int commands { get; set; }
        public long result { get; set; }
        public double duration { get; set; }
           
        
    }
}
