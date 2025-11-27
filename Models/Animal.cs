using System.ComponentModel.DataAnnotations;
using csharp_groep31.Enums;

namespace csharp_groep31.Models
{
    public class Animal
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Species { get; set; } = string.Empty;

        public int? CategoryId { get; set; }
        public Category? Category { get; set; }

        [Required]
        public Size Size { get; set; }

        [Required]
        public DietaryClass DietaryClass { get; set; }

        [Required]
        public ActivityPattern ActivityPattern { get; set; }
        // Dit is geen List<Animal> omdat EF Core geen self-referencing snapt. Anders moet ik een nieuwe class maken
        // en dat mag niet!
        public List<string>? Prey { get; set; }

        public int? EnclosureId { get; set; }
        public Enclosure? Enclosure { get; set; }

        [Range(0.1, double.MaxValue)]
        public double SpaceRequirement { get; set; }

        [Required]
        public SecurityLevel SecurityRequirement { get; set; }
    }
}
