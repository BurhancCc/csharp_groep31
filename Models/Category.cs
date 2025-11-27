using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace csharp_groep31.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;
        // Soms kunnen Category's leeg zijn
        public List<Animal>? Animals { get; set; }
    }
}
