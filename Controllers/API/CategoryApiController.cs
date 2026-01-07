using csharp_groep31.Data;
using csharp_groep31.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace csharp_groep31.Controllers.Api
{
    [ApiController]
    [Route("api/categories")]
    public class CategoryApiController : ControllerBase
    {
        private readonly ZooContext _context;

        public CategoryApiController(ZooContext context)
        {
            _context = context;
        }

        // GET alle categorieën
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Category>>> GetAll()
        {
            var categories = await _context.Categories
                .Include(c => c.Animals) // inclusief dieren
                .ToListAsync();

            return Ok(categories);
        }

        // GET een category /api/categories/{int id}
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Category>> GetById(int id)
        {
            var category = await _context.Categories
                .Include(c => c.Animals) // inclusief dieren
                .FirstOrDefaultAsync(c => c.Id == id);

            if (category == null)
                return NotFound();

            return Ok(category);
        }

        // POST een categorie /api/categories
        [HttpPost]
        public async Task<ActionResult<Category>> Create([FromBody] Category category)
        {
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = category.Id }, category);
        }

        // PUT update een categorie /api/categories/{int id}
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] Category category)
        {
            if (id != category.Id)
                return BadRequest("Id in route en body komen niet overeen.");

            var exists = await _context.Categories.AnyAsync(c => c.Id == id);
            if (!exists)
                return NotFound();

            _context.Entry(category).State = EntityState.Modified;

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

        // DELETE een categorie /api/categories/{int id}
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
                return NotFound();

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
