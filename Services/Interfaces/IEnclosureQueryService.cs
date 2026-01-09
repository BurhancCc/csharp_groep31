using csharp_groep31.Models;

namespace csharp_groep31.Services.Interfaces
{
    public interface IEnclosureQueryService
    {
        Task<List<Enclosure>> SearchAsync(
            string? name,
            string? climate,
            string? habitatType,
            string? securityLevel,
            string? sortBy,
            bool desc = false);
    }
}