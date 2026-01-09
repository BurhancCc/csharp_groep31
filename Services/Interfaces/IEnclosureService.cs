using csharp_groep31.Services;

namespace csharp_groep31.Services.Interfaces
{
    public interface IEnclosureService
    {
        Task<ServiceResult> SunriseAsync(int enclosureId);
        Task<ServiceResult> SunsetAsync(int enclosureId);
        Task<ServiceResult> FeedingTimeAsync(int enclosureId);
        Task<ServiceResult> CheckConstraintsAsync(int enclosureId);
    }
}
