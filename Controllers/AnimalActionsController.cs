using csharp_groep31.Services;
using csharp_groep31.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace csharp_groep31.Controllers
{
    public class AnimalActionsController : Controller
    {
        private readonly IAnimalService _animalService;

        public AnimalActionsController(IAnimalService animalService)
        {
            _animalService = animalService;
        }

        private IActionResult BackToDetails(int id, string title, ServiceResult r)
        {
            TempData["ActionTitle"] = title;
            TempData["ActionSuccess"] = r.Success;
            TempData["ActionMessages"] = string.Join("||", r.Messages);
            return RedirectToAction("Details", "Animals", new { id });
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Sunrise(int id)
            => BackToDetails(id, "Sunrise", await _animalService.SunriseAsync(id));

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Sunset(int id)
            => BackToDetails(id, "Sunset", await _animalService.SunsetAsync(id));

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> FeedingTime(int id)
            => BackToDetails(id, "FeedingTime", await _animalService.FeedingTimeAsync(id));

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> CheckConstraints(int id)
            => BackToDetails(id, "CheckConstraints", await _animalService.CheckConstraintsAsync(id));
    }
}
