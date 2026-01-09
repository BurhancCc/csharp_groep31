using csharp_groep31.Services;
using csharp_groep31.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace csharp_groep31.Controllers
{
    public class EnclosureActionsController : Controller
    {
        private readonly IEnclosureService _enclosureService;

        public EnclosureActionsController(IEnclosureService enclosureService)
        {
            _enclosureService = enclosureService;
        }

        private IActionResult BackToDetails(int id, string title, ServiceResult r)
        {
            TempData["ActionTitle"] = title;
            TempData["ActionSuccess"] = r.Success;
            TempData["ActionMessages"] = string.Join("||", r.Messages);
            return RedirectToAction("Details", "Enclosures", new { id });
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Sunrise(int id)
            => BackToDetails(id, "Sunrise", await _enclosureService.SunriseAsync(id));

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Sunset(int id)
            => BackToDetails(id, "Sunset", await _enclosureService.SunsetAsync(id));

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> FeedingTime(int id)
            => BackToDetails(id, "FeedingTime", await _enclosureService.FeedingTimeAsync(id));

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> CheckConstraints(int id)
            => BackToDetails(id, "CheckConstraints", await _enclosureService.CheckConstraintsAsync(id));
    }
}
