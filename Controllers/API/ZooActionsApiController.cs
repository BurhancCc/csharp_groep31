using csharp_groep31.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace csharp_groep31.Controllers.Api
{
    [ApiController]
    [Route("api/zoo")]
    public class ZooActionsApiController : ControllerBase
    {
        private readonly IZooService _zooService;

        public ZooActionsApiController(IZooService zooService)
        {
            _zooService = zooService;
        }

        [HttpPost("sunrise")]
        public async Task<IActionResult> Sunrise()
        {
            var r = await _zooService.SunriseAsync();
            return Ok(r);
        }

        [HttpPost("sunset")]
        public async Task<IActionResult> Sunset()
        {
            var r = await _zooService.SunsetAsync();
            return Ok(r);
        }

        [HttpPost("feedingtime")]
        public async Task<IActionResult> FeedingTime()
        {
            var r = await _zooService.FeedingTimeAsync();
            return Ok(r);
        }

        [HttpPost("checkconstraints")]
        public async Task<IActionResult> CheckConstraints()
        {
            var r = await _zooService.CheckConstraintsAsync();
            return Ok(r);
        }

        [HttpPost("autoassign")]
        public async Task<IActionResult> AutoAssign([FromQuery] bool resetAll = false)
        {
            var r = await _zooService.AutoAssignAsync(resetAll);
            return Ok(r);
        }
    }
}
