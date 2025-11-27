using System.Collections.Generic;

namespace csharp_groep31.Models
{
    public class Zoo
    {
        public List<Animal> Animals { get; set; } = new();
        public List<Enclosure> Enclosures { get; set; } = new();
        public List<Category> Categories { get; set; } = new();

        // Wordt nog uitgebreid
    }
}
