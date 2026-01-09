using csharp_groep31.Data;
using csharp_groep31.Models;
using csharp_groep31.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace csharp_groep31.Services
{
    public class AnimalQueryService : IAnimalQueryService
    {
        private readonly ZooContext _context;

        public AnimalQueryService(ZooContext context)
        {
            _context = context;
        }

        public async Task<List<Animal>> SearchAnimalsAsync
            (
            string? species,
            int? categoryId,
            int? enclosureId,
            string? name,
            string? sortBy,
            bool desc = false
            )
        {
            IQueryable<Animal> q = _context.Animals
                .Include(a => a.Category)
                .Include(a => a.Enclosure);

            // Filter op gedeeltelijke match
            if (!string.IsNullOrWhiteSpace(species))
            {
                var s = species.Trim();
                q = q.Where(a => a.Species.Contains(s));
            }

            // Filter op categorie
            if (categoryId.HasValue)
                q = q.Where(a => a.CategoryId == categoryId.Value);

            // Filter op enclosure
            if (enclosureId.HasValue)
                q = q.Where(a => a.EnclosureId == enclosureId.Value);

            // Filter op naam
            if (!string.IsNullOrWhiteSpace(name))
            {
                var n = name.Trim();
                q = q.Where(a => a.Name.Contains(n));
            }

            // Sorteer
            q = ApplySorting(q, sortBy, desc);

            return await q.AsNoTracking().ToListAsync();
        }

        public async Task<List<EnclosureGroupResult>> GroupAnimalsByEnclosureAsync(int? categoryId = null)
        {
            IQueryable<Animal> q = _context.Animals
                .Include(a => a.Enclosure)
                .Include(a => a.Category);

            if (categoryId.HasValue)
            {
                q = q.Where(a => a.CategoryId == categoryId.Value);
            }

            var grouped = await q.AsNoTracking()
                .GroupBy(a => new { a.EnclosureId, EnclosureName = a.Enclosure != null ? a.Enclosure.Name : "No enclosure" })
                .Select(g => new EnclosureGroupResult
                {
                    EnclosureId = g.Key.EnclosureId,
                    EnclosureName = g.Key.EnclosureName,
                    Count = g.Count(),
                    Animals = g.ToList()
                })
                .OrderByDescending(x => x.Count)
                .ToListAsync();

            return grouped;
        }

        private static IQueryable<Animal> ApplySorting(IQueryable<Animal> q, string? sortBy, bool desc)
        {
            // Standaard sortering
            if (string.IsNullOrWhiteSpace(sortBy))
            {
                return q.OrderBy(a => a.Name);
            }

            var key = sortBy.Trim().ToLowerInvariant();

            // Dit is echt cool blijkbaar kan je ook zo switches maken
            return (key, desc) switch
            {
                ("name", false) => q.OrderBy(a => a.Name),
                ("name", true) => q.OrderByDescending(a => a.Name),

                ("species", false) => q.OrderBy(a => a.Species).ThenBy(a => a.Name),
                ("species", true) => q.OrderByDescending(a => a.Species).ThenBy(a => a.Name),

                ("size", false) => q.OrderBy(a => a.Size).ThenBy(a => a.Name),
                ("size", true) => q.OrderByDescending(a => a.Size).ThenBy(a => a.Name),

                ("category", false) => q.OrderBy(a => a.Category!.Name).ThenBy(a => a.Name),
                ("category", true) => q.OrderByDescending(a => a.Category!.Name).ThenBy(a => a.Name),

                ("enclosure", false) => q.OrderBy(a => a.Enclosure!.Name).ThenBy(a => a.Name),
                ("enclosure", true) => q.OrderByDescending(a => a.Enclosure!.Name).ThenBy(a => a.Name),

                ("space", false) or ("spacerequirement", false)
                    => q.OrderBy(a => a.SpaceRequirement).ThenBy(a => a.Name),

                ("space", true) or ("spacerequirement", true)
                    => q.OrderByDescending(a => a.SpaceRequirement).ThenBy(a => a.Name),

                ("security", false) or ("securityrequirement", false)
                    => q.OrderBy(a => a.SecurityRequirement).ThenBy(a => a.Name),

                ("security", true) or ("securityrequirement", true)
                    => q.OrderByDescending(a => a.SecurityRequirement).ThenBy(a => a.Name),

                ("dietaryclass", false) => q.OrderBy(a => a.DietaryClass).ThenBy(a => a.Name),
                ("dietaryclass", true) => q.OrderByDescending(a => a.DietaryClass).ThenBy(a => a.Name),

                ("activitypattern", false) => q.OrderBy(a => a.ActivityPattern).ThenBy(a => a.Name),
                ("activitypattern", true) => q.OrderByDescending(a => a.ActivityPattern).ThenBy(a => a.Name),

                _ => q.OrderBy(a => a.Name)
            };
        }
    }
}
