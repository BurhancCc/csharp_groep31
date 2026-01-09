using csharp_groep31.Data;
using csharp_groep31.Enums;
using csharp_groep31.Models;
using csharp_groep31.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace csharp_groep31.Services
{
    public class ZooService : IZooService
    {
        private readonly ZooContext _context;
        public ZooService(ZooContext context)
        {
            _context = context;
        }
        private async Task<List<Models.Animal>> GetAllAnimalsAsync()
        {
            return await _context.Animals
                .Include(a => a.Category)
                .Include(a => a.Enclosure)
                .OrderBy(a => a.Name)
                .ToListAsync();
        }

        public async Task<ServiceResult> SunriseAsync()
        {
            var animals = await GetAllAnimalsAsync();
            var messages = new List<string> { $"Zoo Sunrise ({animals.Count} animals):" };

            foreach (var a in animals)
            {
                switch (a.ActivityPattern)
                {
                    case ActivityPattern.Diurnal:
                        messages.Add($"{a.Name} wakes up.");
                        break;

                    case ActivityPattern.Nocturnal:
                        messages.Add($"{a.Name} goes to sleep.");
                        break;

                    case ActivityPattern.Cathemeral:
                        messages.Add($"{a.Name} is always active.");
                        break;

                    default:
                        messages.Add($"? {a.Name} unknown activity.");
                        break;
                }
            }

            return new ServiceResult { Success = true, Messages = messages };
        }

        public async Task<ServiceResult> SunsetAsync()
        {
            var animals = await GetAllAnimalsAsync();
            var messages = new List<string> { $"Zoo Sunset ({animals.Count} animals):" };

            foreach (var a in animals)
            {
                switch (a.ActivityPattern)
                {
                    case ActivityPattern.Diurnal:
                        messages.Add($"{a.Name} goes to sleep.");
                        break;

                    case ActivityPattern.Nocturnal:
                        messages.Add($"{a.Name} wakes up.");
                        break;

                    case ActivityPattern.Cathemeral:
                        messages.Add($"{a.Name} is always active.");
                        break;

                    default:
                        messages.Add($"? {a.Name} unknown activity.");
                        break;
                }
            }

            return new ServiceResult { Success = true, Messages = messages };
        }

        public async Task<ServiceResult> FeedingTimeAsync()
        {
            var animals = await GetAllAnimalsAsync();
            var messages = new List<string> { $"Zoo FeedingTime ({animals.Count} animals):" };

            foreach (var a in animals)
            {
                if (a.Prey != null && a.Prey.Any())
                {
                    messages.Add($"{a.Name} eats prey before provided food.");
                }
                else
                {
                    messages.Add($"{a.Name} eats {a.DietaryClass} food.");
                }
            }

            return new ServiceResult { Success = true, Messages = messages };
        }

        public async Task<ServiceResult> CheckConstraintsAsync()
        {
            var animals = await GetAllAnimalsAsync();
            var messages = new List<string> { $"Zoo CheckConstraints ({animals.Count} animals):" };

            bool ok = true;

            foreach (var a in animals)
            {
                if (a.Enclosure == null)
                {
                    ok = false;
                    messages.Add($"{a.Name}: no enclosure assigned.");
                    continue;
                }

                if (a.SpaceRequirement <= 0 || a.SpaceRequirement > a.Enclosure.Size)
                {
                    ok = false;
                    messages.Add($"{a.Name}: space constraint failed.");
                }
                else
                {
                    messages.Add($"{a.Name}: space OK.");
                }

                if (a.SecurityRequirement > a.Enclosure.SecurityLevel)
                {
                    ok = false;
                    messages.Add($"{a.Name}: security too low.");
                }
                else
                {
                    messages.Add($"{a.Name}: security OK.");
                }
            }

            return new ServiceResult { Success = ok, Messages = messages };
        }

        private static TEnum FirstEnumValue<TEnum>() where TEnum : struct, Enum
            => Enum.GetValues<TEnum>()[0];

        private static int NextAutoNumber(string name)
        {
            var parts = name.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length > 0 && int.TryParse(parts[^1], out var n)) return n;
            return 0;
        }

        public async Task<ServiceResult> AutoAssignAsync(bool resetAll)
        {
            var messages = new List<string>();
            var ok = true;
            var animals = await _context.Animals
                .OrderBy(a => a.Id)
                .ToListAsync();

            if (animals.Count == 0)
            {
                messages.Add("No animals in the zoo.");
                return new ServiceResult { Success = true, Messages = messages };
            }

            await using var tx = await _context.Database.BeginTransactionAsync();

            try
            {
                // Bij deze optie worden alle verblijven verwijderd
                if (resetAll)
                {
                    messages.Add("Reset mode: unlink all animals and recreate enclosures.");

                    // Eerst worden alle dieren ontkoppeld van hun verblijven
                    foreach (var a in animals)
                        a.EnclosureId = null;

                    await _context.SaveChangesAsync();

                    // Daarna worden alle verblijven verwijderd
                    var enclosures = await _context.Enclosures.ToListAsync();
                    _context.Enclosures.RemoveRange(enclosures);
                    await _context.SaveChangesAsync();

                    messages.Add($"Removed {enclosures.Count} enclosures.");
                }
                else
                {
                    messages.Add("Keep mode: keep existing enclosures and only create when needed.");
                }

                // Alle verblijven worden opnieuw geladen in het geval dat het wordt gereset
                var existing = await _context.Enclosures
                    .OrderBy(e => e.Id)
                    .ToListAsync();

                // Automatisch gemaakte verblijven krijgen een automatische naam
                var maxAuto = existing
                    .Select(e => e.Name ?? "")
                    .Where(n => n.StartsWith("Auto Enclosure"))
                    .Select(NextAutoNumber)
                    .DefaultIfEmpty(0)
                    .Max();

                var createdCount = 0;
                var assignedCount = 0;

                // Voor ieder dier wordt gekeken of de security voldoende is en of het dier er in past
                // Er wordt eerst gekeken wat het kleinste verblijf is waar het dier nog in past
                // Als een dier nergens meer past wordt er een nieuw verblijf aangemaakt
                foreach (var a in animals)
                {
                    if (!resetAll && a.EnclosureId != null)
                    {
                        continue;
                    }

                    if (a.SpaceRequirement <= 0)
                    {
                        ok = false;
                        messages.Add($"{a.Name}: SpaceRequirement must be > 0 (cannot auto-assign reliably).");
                        continue;
                    }

                    var candidate = existing
                        .Where(e => (int)e.Size >= a.SpaceRequirement && (int)e.SecurityLevel >= (int)a.SecurityRequirement)
                        .OrderBy(e => (int)e.Size)
                        .ThenBy(e => (int)e.SecurityLevel)
                        .FirstOrDefault();

                    if (candidate == null)
                    {
                        // Nieuw verblijf aanmaken als er geen geschikt verblijf is voor dier
                        maxAuto++;
                        var newEnclosure = new Enclosure
                        {
                            Name = $"Auto Enclosure {maxAuto}",
                            Size = a.SpaceRequirement,
                            SecurityLevel = a.SecurityRequirement,
                            Climate = FirstEnumValue<Climate>(),
                            HabitatType = FirstEnumValue<HabitatType>()
                        };

                        _context.Enclosures.Add(newEnclosure);
                        await _context.SaveChangesAsync();

                        existing.Add(newEnclosure);
                        candidate = newEnclosure;

                        createdCount++;
                        messages.Add($"Created '{candidate.Name}' (Size={candidate.Size}, Security={candidate.SecurityLevel}).");
                    }

                    a.EnclosureId = candidate.Id;
                    assignedCount++;
                    messages.Add($"Assigned {a.Name} -> {candidate.Name}.");
                }

                await _context.SaveChangesAsync();
                await tx.CommitAsync();

                messages.Add($"Done. Assigned: {assignedCount}. Created enclosures: {createdCount}.");

                return new ServiceResult { Success = ok, Messages = messages };
            }
            catch (Exception ex)
            {
                await tx.RollbackAsync();
                return ServiceResult.Fail($"AutoAssign failed: {ex.Message}");
            }
        }
    }
}