using csharp_groep31.Models;

namespace csharp_groep31.Services.Interfaces
{
    public interface IAnimalQueryService
    {
        Task<List<Animal>> SearchAnimalsAsync
            (
            string? species,
            int? categoryId,
            int? enclosureId,
            string? name,
            string? sortBy,
            bool desc = false
            );
        Task<List<EnclosureGroupResult>> GroupAnimalsByEnclosureAsync(int? categoryId = null);
    }

    public class EnclosureGroupResult
    {
        public int? EnclosureId { get; set; }
        public string EnclosureName { get; set; } = "No enclosure";
        public int Count { get; set; }
        public List<Animal> Animals { get; set; } = new();
    }
}
