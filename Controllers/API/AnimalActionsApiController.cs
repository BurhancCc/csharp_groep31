using csharp_groep31.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace csharp_groep31.Controllers.Api
{
    [ApiController]
    [Route("api/animals/{id:int}")]
    public class AnimalActionsApiController : ControllerBase
    {
        private readonly IAnimalService _animalService;

        public AnimalActionsApiController(IAnimalService animalService)
        {
            _animalService = animalService;
        }

        [HttpPost("sunrise")]
        public async Task<IActionResult> Sunrise(int id)
        {
            var r = await _animalService.SunriseAsync(id);
            return r.Success ? Ok(r) : NotFound(r);
        }

        [HttpPost("sunset")]
        public async Task<IActionResult> Sunset(int id)
        {
            var r = await _animalService.SunsetAsync(id);
            return r.Success ? Ok(r) : NotFound(r);
        }

        [HttpPost("feedingtime")]
        public async Task<IActionResult> FeedingTime(int id)
        {
            var r = await _animalService.FeedingTimeAsync(id);
            return r.Success ? Ok(r) : NotFound(r);
        }

        [HttpPost("checkconstraints")]
        public async Task<IActionResult> CheckConstraints(int id)
        {
            var r = await _animalService.CheckConstraintsAsync(id);
            return r.Messages.Contains("Animal not found") ? NotFound(r) : Ok(r);
        }
    }
}
