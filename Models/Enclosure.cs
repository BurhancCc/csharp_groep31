using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using csharp_groep31.Enums;

namespace csharp_groep31.Models
{
    public class Enclosure
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        public List<Animal>? Animals { get; set; }

        [Required]
        public Climate Climate { get; set; }

        [Required]
        public HabitatType HabitatType { get; set; }

        [Required]
        public SecurityLevel SecurityLevel { get; set; }

        [Range(1, double.MaxValue)]
        public double Size { get; set; }
    }
}
