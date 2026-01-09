using csharp_groep31.Data;
using csharp_groep31.Enums;
using csharp_groep31.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace csharp_groep31.Services
{
    public class EnclosureService : IEnclosureService
    {
        private readonly ZooContext _context;

        public EnclosureService(ZooContext context)
        {
            _context = context;
        }

        private async Task<Models.Enclosure?> GetEnclosureFullAsync(int id)
        {
            return await _context.Enclosures
                .Include(e => e.Animals)
                .ThenInclude(a => a.Category)
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<ServiceResult> SunriseAsync(int enclosureId)
        {
            var enclosure = await GetEnclosureFullAsync(enclosureId);
            if (enclosure == null)
            {
                return ServiceResult.Fail($"Enclosure {enclosureId} not found.");
            }

            var messages = new List<string>
            {
                $"Sunrise in enclosure '{enclosure.Name}' ({enclosure.Animals.Count} animals):"
            };

            if (enclosure.Animals.Count == 0)
            {
                messages.Add("No animals in this enclosure.");
                return new ServiceResult { Success = true, Messages = messages };
            }

            foreach (var animal in enclosure.Animals.OrderBy(a => a.Name))
            {
                string msg;

                switch (animal.ActivityPattern)
                {
                    case ActivityPattern.Diurnal:
                        msg = $"{animal.Name} wakes up ({animal.ActivityPattern}).";
                        break;

                    case ActivityPattern.Nocturnal:
                        msg = $"{animal.Name} goes to sleep ({animal.ActivityPattern}).";
                        break;

                    case ActivityPattern.Cathemeral:
                        msg = $"{animal.Name} is always active ({animal.ActivityPattern}).";
                        break;

                    default:
                        msg = $"? {animal.Name}: unknown activity pattern.";
                        break;
                }
                messages.Add(msg);
            }

            return new ServiceResult { Success = true, Messages = messages };
        }

        public async Task<ServiceResult> SunsetAsync(int enclosureId)
        {
            var enclosure = await GetEnclosureFullAsync(enclosureId);
            if (enclosure == null)
            {
                return ServiceResult.Fail($"Enclosure {enclosureId} not found.");
            }

            var messages = new List<string>
            {
                $"Sunset in enclosure '{enclosure.Name}' ({enclosure.Animals.Count} animals):"
            };

            if (enclosure.Animals.Count == 0)
            {
                messages.Add("No animals in this enclosure.");
                return new ServiceResult { Success = true, Messages = messages };
            }

            foreach (var animal in enclosure.Animals.OrderBy(a => a.Name))
            {
                string msg;

                switch (animal.ActivityPattern)
                {
                    case ActivityPattern.Diurnal:
                        msg = $"{animal.Name} goes to sleep ({animal.ActivityPattern}).";
                        break;

                    case ActivityPattern.Nocturnal:
                        msg = $"{animal.Name} wakes up ({animal.ActivityPattern}).";
                        break;

                    case ActivityPattern.Cathemeral:
                        msg = $"{animal.Name} is always active ({animal.ActivityPattern}).";
                        break;

                    default:
                        msg = $"? {animal.Name}: unknown activity pattern.";
                        break;
                }
                messages.Add(msg);
            }

            return new ServiceResult { Success = true, Messages = messages };
        }

        public async Task<ServiceResult> FeedingTimeAsync(int enclosureId)
        {
            var enclosure = await GetEnclosureFullAsync(enclosureId);
            if (enclosure == null)
            {
                return ServiceResult.Fail($"Enclosure {enclosureId} not found.");
            }

            var messages = new List<string>
            {
                $"FeedingTime in enclosure '{enclosure.Name}' ({enclosure.Animals.Count} animals):"
            };

            if (enclosure.Animals.Count == 0)
            {
                messages.Add("No animals in this enclosure.");
                return new ServiceResult { Success = true, Messages = messages };
            }

            foreach (var animal in enclosure.Animals.OrderBy(a => a.Name))
            {
                if (animal.Prey != null && animal.Prey.Count > 0)
                {
                    var preyList = string.Join(", ", animal.Prey);
                    messages.Add($"{animal.Name} will prioritize eating prey over provided food. Prey: {preyList}.");
                }
                else
                {
                    messages.Add($"{animal.Name} eats {animal.DietaryClass} food.");
                }
            }

            return new ServiceResult { Success = true, Messages = messages };
        }

        public async Task<ServiceResult> CheckConstraintsAsync(int enclosureId)
        {
            var enclosure = await GetEnclosureFullAsync(enclosureId);
            if (enclosure == null)
            {
                return ServiceResult.Fail($"Enclosure {enclosureId} not found.");
            }

            var messages = new List<string>
            {
                $"CheckConstraints for enclosure '{enclosure.Name}' (Size={enclosure.Size}, Security={enclosure.SecurityLevel}):"
            };

            bool ok = true;

            if (enclosure.Animals.Count == 0)
            {
                messages.Add("No animals in this enclosure.");
                return new ServiceResult { Success = true, Messages = messages };
            }

            foreach (var animal in enclosure.Animals.OrderBy(a => a.Name))
            {
                if (animal.CategoryId == null)
                    messages.Add($"{animal.Name}: No Category assigned (NULL is allowed).");
                else
                    messages.Add($"{animal.Name}: Category assigned.");

                if (animal.SpaceRequirement <= 0)
                {
                    ok = false;
                    messages.Add($"{animal.Name}: SpaceRequirement must be > 0.");
                }
                else if (animal.SpaceRequirement > enclosure.Size)
                {
                    ok = false;
                    messages.Add($"{animal.Name}: Not enough space. Needs {animal.SpaceRequirement}, enclosure has {enclosure.Size}.");
                }
                else
                {
                    messages.Add($"{animal.Name}: Space OK. Needs {animal.SpaceRequirement}, enclosure has {enclosure.Size}.");
                }

                if (animal.SecurityRequirement > enclosure.SecurityLevel)
                {
                    ok = false;
                    messages.Add($"{animal.Name}: Security too low. Requires {animal.SecurityRequirement}, enclosure is {enclosure.SecurityLevel}.");
                }
                else
                {
                    messages.Add($"{animal.Name}: Security OK. Requires {animal.SecurityRequirement}, enclosure is {enclosure.SecurityLevel}.");
                }
            }

            return new ServiceResult { Success = ok, Messages = messages };
        }
    }
}
