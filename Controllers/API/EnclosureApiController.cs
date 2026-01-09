using csharp_groep31.Data;
using csharp_groep31.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using csharp_groep31.Services.Interfaces;

namespace csharp_groep31.Controllers.Api
{
    [ApiController]
    [Route("api/enclosures")]
    public class EnclosureApiController : ControllerBase
    {
        private readonly ZooContext _context;
        private readonly IEnclosureQueryService _enclosureQuery;

        public EnclosureApiController(ZooContext context, IEnclosureQueryService enclosureQuery)
        {
            _context = context;
            _enclosureQuery = enclosureQuery;
        }

        // GET alle verblijven
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Enclosure>>> GetAll()
        {
            var enclosures = await _context.Enclosures
                .Include(e => e.Animals) // inclusief dieren
                .ToListAsync();

            return Ok(enclosures);
        }

        // GET een verblijf /api/enclosures/{int id}
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Enclosure>> GetById(int id)
        {
            var enclosure = await _context.Enclosures
                .Include(e => e.Animals) // inclusief dieren
                .FirstOrDefaultAsync(e => e.Id == id);

            if (enclosure == null)
                return NotFound();

            return Ok(enclosure);
        }

        // POST een verblijf /api/enclosures
        [HttpPost]
        public async Task<ActionResult<Enclosure>> Create([FromBody] Enclosure enclosure)
        {
            _context.Enclosures.Add(enclosure);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = enclosure.Id }, enclosure);
        }

        // PUT update een verblijf /api/enclosures/{int id}
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] Enclosure enclosure)
        {
            if (id != enclosure.Id)
                return BadRequest("Id in route en body komen niet overeen.");

            var exists = await _context.Enclosures.AnyAsync(e => e.Id == id);
            if (!exists)
                return NotFound();

            _context.Entry(enclosure).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return Conflict("Concurrency conflict bij updaten.");
            }

            return NoContent();
        }

        // DELETE een verblijf /api/enclosures/{int id}
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var enclosure = await _context.Enclosures.FindAsync(id);
            if (enclosure == null)
                return NotFound();

            _context.Enclosures.Remove(enclosure);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // GET /api/enclosures?{params}
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<Enclosure>>> Get
            (
            [FromQuery] string? name,
            [FromQuery] string? climate,
            [FromQuery] string? habitatType,
            [FromQuery] string? securityLevel,
            [FromQuery] string? sortBy,
            [FromQuery] bool desc = false
            )
        {
            var result = await _enclosureQuery.SearchAsync(name, climate, habitatType, securityLevel, sortBy, desc);
            return Ok(result);
        }
    }
}
