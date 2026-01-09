using csharp_groep31.Data;
using csharp_groep31.Enums;
using csharp_groep31.Models;
using csharp_groep31.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace csharp_groep31.Services
{
    public class EnclosureQueryService : IEnclosureQueryService
    {
        private readonly ZooContext _context;

        public EnclosureQueryService(ZooContext context)
        {
            _context = context;
        }

        public async Task<List<Enclosure>> SearchAsync(
            string? name,
            string? climate,
            string? habitatType,
            string? securityLevel,
            string? sortBy,
            bool desc = false)
        {
            IQueryable<Enclosure> q = _context.Enclosures;

            if (!string.IsNullOrWhiteSpace(name))
                q = q.Where(e => e.Name.Contains(name));

            if (Enum.TryParse<Climate>(climate, true, out var c))
                q = q.Where(e => e.Climate == c);

            if (Enum.TryParse<HabitatType>(habitatType, true, out var h))
                q = q.Where(e => e.HabitatType == h);

            if (Enum.TryParse<SecurityLevel>(securityLevel, true, out var s))
                q = q.Where(e => e.SecurityLevel == s);

            var key = (sortBy ?? "").ToLowerInvariant();

            q = (key, desc) switch
            {
                ("name", false) => q.OrderBy(e => e.Name),
                ("name", true) => q.OrderByDescending(e => e.Name),

                ("climate", false) => q.OrderBy(e => e.Climate),
                ("climate", true) => q.OrderByDescending(e => e.Climate),

                ("habitat", false) => q.OrderBy(e => e.HabitatType),
                ("habitat", true) => q.OrderByDescending(e => e.HabitatType),

                ("security", false) => q.OrderBy(e => e.SecurityLevel),
                ("security", true) => q.OrderByDescending(e => e.SecurityLevel),

                ("size", false) => q.OrderBy(e => e.Size),
                ("size", true) => q.OrderByDescending(e => e.Size),

                _ => q.OrderBy(e => e.Name)
            };

            return await q.AsNoTracking().ToListAsync();
        }
    }
}