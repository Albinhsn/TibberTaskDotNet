

namespace TibberTask.Models
{
    public class ExecutionRequest
    {
        public Point Start { get; set; }
        public List<Command> Commands { get; set; }
    }
}
