using csharp_groep31.Models;

namespace csharp_groep31.Services.Interfaces
{
    public interface ICategoryQueryService
    {
        Task<List<Category>> SearchCategoriesAsync(string? name, bool desc = false);
    }
}