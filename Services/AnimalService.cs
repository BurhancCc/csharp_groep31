using csharp_groep31.Data;
using csharp_groep31.Enums;
using csharp_groep31.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace csharp_groep31.Services
{
    public class AnimalService : IAnimalService
    {
        private readonly ZooContext _context;
        public AnimalService(ZooContext context)
        {
            _context = context;
        }

        private async Task<Models.Animal?> GetAnimalFullAsync(int id)
        {
            return await _context.Animals
                .Include(a => a.Category)
                .Include(a => a.Enclosure!)
                    .ThenInclude(e => e.Animals)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<ServiceResult> SunriseAsync(int animalId)
        {
            var animal = await GetAnimalFullAsync(animalId);
            if (animal == null)
            {
                return ServiceResult.Fail($"Animal {animalId} not found.");
            }

            string msg;

            switch (animal.ActivityPattern)
            {
                case ActivityPattern.Diurnal:
                    msg = $"Sunrise: {animal.Name} wakes up ({animal.ActivityPattern}).";
                    break;

                case ActivityPattern.Nocturnal:
                    msg = $"Sunrise: {animal.Name} goes to sleep ({animal.ActivityPattern}).";
                    break;

                case ActivityPattern.Cathemeral:
                    msg = $"Sunrise: {animal.Name} is always active ({animal.ActivityPattern}).";
                    break;

                default:
                    msg = $"Sunrise: unknown activity pattern for {animal.Name}.";
                    break;
            }
            return ServiceResult.Ok(msg);
        }
        public async Task<ServiceResult> SunsetAsync(int animalId)
        {
            var animal = await GetAnimalFullAsync(animalId);
            if (animal == null)
            {
                return ServiceResult.Fail($"Animal {animalId} not found.");
            }

            string msg;

            switch (animal.ActivityPattern)
            {
                case ActivityPattern.Diurnal:
                    msg = $"Sunset: {animal.Name} goes to sleep ({animal.ActivityPattern}).";
                    break;

                case ActivityPattern.Nocturnal:
                    msg = $"Sunset: {animal.Name} wakes up ({animal.ActivityPattern}).";
                    break;

                case ActivityPattern.Cathemeral:
                    msg = $"Sunset: {animal.Name} is always active ({animal.ActivityPattern}).";
                    break;

                default:
                    msg = $"Sunset: unknown activity pattern for {animal.Name}.";
                    break;
            }
            return ServiceResult.Ok(msg);
        }
        public async Task<ServiceResult> FeedingTimeAsync(int animalId)
        {
            var animal = await GetAnimalFullAsync(animalId);
            if (animal == null)
            {
                return ServiceResult.Fail($"Animal {animalId} not found.");
            }

            // Prooi voor eten
            if (animal.Prey != null && animal.Prey.Count > 0)
            {
                var preyList = string.Join(", ", animal.Prey);
                return ServiceResult.Ok($"FeedingTime: {animal.Name} will prioritize eating other animals (prey) over provided food. Prey: {preyList}.");
            }
            return ServiceResult.Ok($"FeedingTime: {animal.Name} eats {animal.DietaryClass} food.");
        }
        public async Task<ServiceResult> CheckConstraintsAsync(int animalId)
        {
            var animal = await GetAnimalFullAsync(animalId);
            if (animal == null)
            {
                return ServiceResult.Fail($"Animal {animalId} not found.");
            }

            var messages = new List<string>();
            bool ok = true;

            // Categorie mag null zijn
            if (animal.CategoryId == null)
            {
                messages.Add("No Category assigned (NULL is allowed).");
            }
            else
            {
                messages.Add("Category assigned.");
            }

            // Enclosure mag null zijn
            if (animal.Enclosure == null)
            {
                messages.Add("No Enclosure assigned (NULL is allowed).");
                return new ServiceResult { Success = ok, Messages = messages };
            }

            messages.Add($"Enclosure assigned: '{animal.Enclosure.Name}'.");

            if (animal.SpaceRequirement <= 0)
            {
                ok = false;
                messages.Add("SpaceRequirement must be > 0.");
            }
            else if (animal.SpaceRequirement > animal.Enclosure.Size)
            {
                ok = false;
                messages.Add($"Not enough space. Needs {animal.SpaceRequirement}, enclosure has {animal.Enclosure.Size}.");
            }
            else
            {
                messages.Add($"Space OK. Needs {animal.SpaceRequirement}, enclosure has {animal.Enclosure.Size}.");
            }

            if (animal.SecurityRequirement > animal.Enclosure.SecurityLevel)
            {
                ok = false;
                messages.Add($"Security too low. Requires {animal.SecurityRequirement}, enclosure is {animal.Enclosure.SecurityLevel}.");
            }
            else
            {
                messages.Add($"Security OK. Requires {animal.SecurityRequirement}, enclosure is {animal.Enclosure.SecurityLevel}.");
            }

            return new ServiceResult { Success = ok, Messages = messages };
        }
    }
}
