

namespace TibberTask.Models
{
    public class ExecutionRequest
    {
        public Point start { get; set; }
        public List<command> commands { get; set; }
    }
}
