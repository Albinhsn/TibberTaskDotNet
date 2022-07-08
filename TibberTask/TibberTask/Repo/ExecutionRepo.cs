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
            await _context.SaveChangesAsync();            
            return exe;
        }
    }
}
