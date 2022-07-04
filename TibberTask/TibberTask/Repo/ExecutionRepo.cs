using TibberTask.Models;
using TibberTask.PG;

namespace TibberTask.Repo
{
    public class ExecutionRepo
    {

        private readonly ExecutionCont _context;

        public ExecutionRepo(ExecutionCont context)
        {
            _context = context;
        }

        public async Task<execution> InsertExecutionAsync(execution exe)
        {
            
            _context.AddAsync(exe);
            Console.WriteLine(exe.id.ToString());
            Console.WriteLine(exe.result.ToString());
            Console.WriteLine(exe.duration.ToString());
            Console.WriteLine(exe.commands.ToString());
            Console.WriteLine(exe.timestamp.ToString());
            await _context.SaveChangesAsync();            
            return exe;
        }
    }
}
