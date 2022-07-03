using Microsoft.AspNetCore.Mvc;
using TibberTask.Helpers;
using TibberTask.Models;
using System.Diagnostics;
using System.Text.Json;

namespace TibberTask.Controllers
{
    [ApiController]
    
    public class RobotController : ControllerBase
    {
        [HttpPost("/tibber-developer-test/enter-path")]
        public IActionResult PostExecution([FromBody] ExecutionRequest JsonRequest)
        {
            ExecutionRequest req = JsonRequest;            
            DateTime Timestamp = DateTime.Now;
            RobotHelper helper = new();
            Stopwatch stopwatch = new();
            stopwatch.Start();
            long result = helper.CalculateResult(req);
            stopwatch.Stop();
          
            //TODO: Generate id from postgresql instead
            Execution execution = new(
                0,
                Timestamp,
                req.commands.Count,
                result,
                stopwatch.Elapsed.TotalSeconds
            );
 
            return Ok(JsonSerializer.Serialize(execution));
        }
    }
}
