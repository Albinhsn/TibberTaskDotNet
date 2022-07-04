using Microsoft.AspNetCore.Mvc;
using TibberTask.Helpers;
using TibberTask.Models;
using System.Diagnostics;
using System.Text.Json;
using TibberTask.Repo;

namespace TibberTask.Controllers
{
    [ApiController]
    
    public class RobotController : ControllerBase
    {
        ExecutionRepo _repo;
        public RobotController(ExecutionRepo repo)
        {
            _repo = repo; 
        }

        [HttpPost("/tibber-developer-test/enter-path")]
        public IActionResult PostExecution([FromBody] ExecutionRequest req)
        {            
            RobotHelper helper = new();
            Stopwatch stopwatch = new();
            stopwatch.Start();
            long result = helper.CalculateResult(req);
            stopwatch.Stop();
          
            
            execution exe= new();
            exe.duration = stopwatch.Elapsed.TotalSeconds;
            exe.result = result;
            exe.commands = req.commands.Count;
            exe.timestamp = DateTime.UtcNow;
            
            execution createdExe;
            try
            {
                createdExe = _repo.InsertExecutionAsync(exe).Result;
            }
            catch (Exception ex)
            {
                return BadRequest("Something went wrong" + ex.ToString());
            }

            return Ok(JsonSerializer.Serialize(createdExe));
        }
    }
}
