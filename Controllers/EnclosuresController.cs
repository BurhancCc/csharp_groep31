using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using csharp_groep31.Data;
using csharp_groep31.Models;
using csharp_groep31.Services.Interfaces;

namespace csharp_groep31.Controllers
{
    public class EnclosuresController : Controller
    {
        private readonly ZooContext _context;
        private readonly IEnclosureQueryService _enclosureQuery;

        public EnclosuresController(ZooContext context, IEnclosureQueryService enclosureQuery)
        {
            _context = context;
            _enclosureQuery = enclosureQuery;
        }

        // GET: Enclosures
        public async Task<IActionResult> Index
            (
            string? name,
            string? climate,
            string? habitatType,
            string? securityLevel,
            string? sortBy,
            bool desc = false
            )
        {
            var result = await _enclosureQuery.SearchAsync(name, climate, habitatType, securityLevel, sortBy, desc);
            return View(result);
        }

        // GET: Enclosures/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var enclosure = await _context.Enclosures
                .FirstOrDefaultAsync(m => m.Id == id);

            if (enclosure == null)
            {
                return NotFound();
            }

            return View(enclosure);
        }

        // GET: Enclosures/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Enclosures/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Climate,HabitatType,SecurityLevel,Size")] Enclosure enclosure)
        {
            if (ModelState.IsValid)
            {
                _context.Add(enclosure);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(enclosure);
        }

        // GET: Enclosures/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var enclosure = await _context.Enclosures
                .Include(e => e.Animals)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (enclosure == null) return NotFound();

            ViewBag.UnassignedAnimals = new SelectList
                (
                await _context.Animals
                    .Where(a => a.EnclosureId == null)
                    .OrderBy(a => a.Name)
                    .ToListAsync(),
                "Id",
                "Name"
                );

            return View(enclosure);
        }



        // POST: Enclosures/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Climate,HabitatType,SecurityLevel,Size")] Enclosure enclosure)
        {
            if (id != enclosure.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(enclosure);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EnclosureExists(enclosure.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(enclosure);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddAnimal(int enclosureId, int animalId)
        {
            var animal = await _context.Animals.FindAsync(animalId);
            if (animal == null) return NotFound();

            animal.EnclosureId = enclosureId;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Edit), new { id = enclosureId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveAnimal(int enclosureId, int animalId)
        {
            var animal = await _context.Animals.FindAsync(animalId);
            if (animal == null) return NotFound();

            if (animal.EnclosureId == enclosureId)
            {
                animal.EnclosureId = null;
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Edit), new { id = enclosureId });
        }


        // GET: Enclosures/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var enclosure = await _context.Enclosures
                .FirstOrDefaultAsync(m => m.Id == id);

            if (enclosure == null)
            {
                return NotFound();
            }

            return View(enclosure);
        }

        // POST: Enclosures/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var enclosure = await _context.Enclosures.FindAsync(id);

            if (enclosure != null)
            {
                _context.Enclosures.Remove(enclosure);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EnclosureExists(int id)
        {
            return _context.Enclosures.Any(e => e.Id == id);
        }
    }
}
