using csharp_groep31.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace csharp_groep31.Controllers.Api
{
    [ApiController]
    [Route("api/enclosures/{id:int}")]
    public class EnclosureActionsApiController : ControllerBase
    {
        private readonly IEnclosureService _enclosureService;

        public EnclosureActionsApiController(IEnclosureService enclosureService)
        {
            _enclosureService = enclosureService;
        }

        [HttpPost("sunrise")]
        public async Task<IActionResult> Sunrise(int id)
        {
            var r = await _enclosureService.SunriseAsync(id);
            return r.Messages.Any(m => m.Contains("not found")) ? NotFound(r) : Ok(r);
        }

        [HttpPost("sunset")]
        public async Task<IActionResult> Sunset(int id)
        {
            var r = await _enclosureService.SunsetAsync(id);
            return r.Messages.Any(m => m.Contains("not found")) ? NotFound(r) : Ok(r);
        }

        [HttpPost("feedingtime")]
        public async Task<IActionResult> FeedingTime(int id)
        {
            var r = await _enclosureService.FeedingTimeAsync(id);
            return r.Messages.Any(m => m.Contains("not found")) ? NotFound(r) : Ok(r);
        }

        [HttpPost("checkconstraints")]
        public async Task<IActionResult> CheckConstraints(int id)
        {
            var r = await _enclosureService.CheckConstraintsAsync(id);
            return r.Messages.Any(m => m.Contains("not found")) ? NotFound(r) : Ok(r);
        }
    }
}
