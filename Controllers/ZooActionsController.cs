using csharp_groep31.Services;
using csharp_groep31.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace csharp_groep31.Controllers
{
    public class ZooActionsController : Controller
    {
        private readonly IZooService _zooService;

        public ZooActionsController(IZooService zooService)
        {
            _zooService = zooService;
        }

        private IActionResult BackHome(string title, ServiceResult r)
        {
            TempData["ActionTitle"] = title;
            TempData["ActionSuccess"] = r.Success;
            TempData["ActionMessages"] = string.Join("||", r.Messages);
            return RedirectToAction("Index", "Home");
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Sunrise()
            => BackHome("Zoo Sunrise", await _zooService.SunriseAsync());

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Sunset()
            => BackHome("Zoo Sunset", await _zooService.SunsetAsync());

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> FeedingTime()
            => BackHome("Zoo FeedingTime", await _zooService.FeedingTimeAsync());

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> CheckConstraints()
            => BackHome("Zoo CheckConstraints", await _zooService.CheckConstraintsAsync());

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> AutoAssign(bool resetAll)
            => BackHome(resetAll ? "Zoo AutoAssign (RESET)" : "Zoo AutoAssign (KEEP)",
                await _zooService.AutoAssignAsync(resetAll));
    }
}
