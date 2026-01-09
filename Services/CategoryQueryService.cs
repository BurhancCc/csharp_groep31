using csharp_groep31.Data;
using csharp_groep31.Models;
using csharp_groep31.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace csharp_groep31.Services
{
    public class CategoryQueryService : ICategoryQueryService
    {
        private readonly ZooContext _context;

        public CategoryQueryService(ZooContext context)
        {
            _context = context;
        }

        public async Task<List<Category>> SearchCategoriesAsync(string? name, bool desc = false)
        {
            IQueryable<Category> q = _context.Categories;

            if (!string.IsNullOrWhiteSpace(name))
            {
                var n = name.Trim();
                q = q.Where(c => c.Name.Contains(n));
            }

            q = desc ? q.OrderByDescending(c => c.Name) : q.OrderBy(c => c.Name);
            return await q.AsNoTracking().ToListAsync();
        }
    }
}