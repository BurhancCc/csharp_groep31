using csharp_groep31.Data;
using csharp_groep31.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using csharp_groep31.Services.Interfaces;

namespace csharp_groep31.Controllers.Api
{
    [ApiController]
    [Route("api/animals")]
    public class AnimalApiController : ControllerBase
    {
        private readonly ZooContext _context;
        private readonly IAnimalQueryService _animalQuery;

        public AnimalApiController(ZooContext context, IAnimalQueryService animalQuery)
        {
            _context = context;
            _animalQuery = animalQuery;
        }

        // GET alle dieren
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Animal>>> GetAll()
        {
            var animals = await _context.Animals
                .Include(a => a.Category)
                .Include(a => a.Enclosure)
                .ToListAsync();

            return Ok(animals);
        }

        // GET een dier /api/animals/{int id}
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Animal>> GetById(int id)
        {
            var animal = await _context.Animals
                .Include(a => a.Category)
                .Include(a => a.Enclosure)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (animal == null)
                return NotFound();

            return Ok(animal);
        }

        // POST een dier /api/animals
        [HttpPost]
        public async Task<ActionResult<Animal>> Create([FromBody] Animal animal)
        {
            _context.Animals.Add(animal);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = animal.Id }, animal);
        }

        // PUT update een dier /api/animals/{int id}
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] Animal animal)
        {
            if (id != animal.Id)
                return BadRequest("Id in route en body komen niet overeen.");

            var exists = await _context.Animals.AnyAsync(a => a.Id == id);
            if (!exists)
                return NotFound();

            _context.Entry(animal).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return Conflict("Concurrency conflict bij updaten.");
            }

            return NoContent(); // 204
        }

        // DELETE een dier /api/animals/{int id}
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var animal = await _context.Animals.FindAsync(id);
            if (animal == null)
                return NotFound();

            _context.Animals.Remove(animal);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // GET /api/animals?{params}
        [HttpGet("search")]
        public async Task<ActionResult> Search
            (
            [FromQuery] string? species,
            [FromQuery] int? categoryId,
            [FromQuery] int? enclosureId,
            [FromQuery] string? name,
            [FromQuery] string? sortBy,
            [FromQuery] bool desc = false
            )
        {
            var animals = await _animalQuery.SearchAnimalsAsync(species, categoryId, enclosureId, name, sortBy, desc);
            return Ok(animals);
        }

        [HttpGet("group-by-enclosure")]
        public async Task<ActionResult> GroupByEnclosure([FromQuery] int? categoryId = null)
        {
            var result = await _animalQuery.GroupAnimalsByEnclosureAsync(categoryId);
            return Ok(result);
        }
    }
}
