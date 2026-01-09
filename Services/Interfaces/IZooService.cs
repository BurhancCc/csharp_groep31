using csharp_groep31.Services;

namespace csharp_groep31.Services.Interfaces
{
    public interface IZooService
    {
        Task<ServiceResult> SunriseAsync();
        Task<ServiceResult> SunsetAsync();
        Task<ServiceResult> FeedingTimeAsync();
        Task<ServiceResult> CheckConstraintsAsync();
        Task<ServiceResult> AutoAssignAsync(bool resetAll);
    }
}