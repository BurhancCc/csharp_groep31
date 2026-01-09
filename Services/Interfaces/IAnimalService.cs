using csharp_groep31.Services;

namespace csharp_groep31.Services.Interfaces
{
    public interface IAnimalService
    {
        Task<ServiceResult> SunriseAsync(int animalId);
        Task<ServiceResult> SunsetAsync(int animalId);
        Task<ServiceResult> FeedingTimeAsync(int animalId);
        Task<ServiceResult> CheckConstraintsAsync(int animalId);
    }
}
