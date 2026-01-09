using Bogus;
using Bogus.DataSets;
using csharp_groep31.Enums;
using csharp_groep31.Models;
using Microsoft.EntityFrameworkCore;

namespace csharp_groep31.Data
{
    public class ZooContext : DbContext
    {
        public ZooContext(DbContextOptions<ZooContext> options)
            : base(options)
        {
        }

        public DbSet<Animal> Animals { get; set; } = null!;
        public DbSet<Category> Categories { get; set; } = null!;
        public DbSet<Enclosure> Enclosures { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Animal>()
                .HasOne(animal => animal.Category)
                .WithMany(category => category.Animals)
                .HasForeignKey(animal => animal.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Animal>()
                .HasOne(animal => animal.Enclosure)
                .WithMany(enclosure => enclosure.Animals)
                .HasForeignKey(animal => animal.EnclosureId)
                .OnDelete(DeleteBehavior.Restrict);

            // Seeding van Category's en Enclosure's
            var categories = new List<Category>
            {
                new Category { Id = 1, Name = "Mammals" },
                new Category { Id = 2, Name = "Birds" },
                new Category { Id = 3, Name = "Reptiles" },
                new Category { Id = 4, Name = "Fish"},
            };
            modelBuilder.Entity<Category>().HasData(categories);

            Random random = new Random(42);

            // Ik maak alleen heel simpele enclosures. Geen HabitatType want straks heb je Aquatic Desert das niet logsich
            var enclosures = new List<Enclosure>
            {
                new Enclosure
                {
                    Id = 1,
                    Name = "Agressive land animal enclosure",
                    SecurityLevel = SecurityLevel.High,
                    Size = random.Next(500, 2000),
                },
                new Enclosure
                {
                    Id = 2,
                    Name = "Non-agressive land animal enclosure",
                    SecurityLevel = SecurityLevel.Medium,
                    Size = random.Next(500, 2000),
                },
                new Enclosure
                {
                    Id = 3,
                    Name = "Agressive ambhibious animal enclosure",
                    SecurityLevel = SecurityLevel.High,
                    Size = random.Next(500, 2000),
                },
                new Enclosure
                {
                    Id = 4,
                    Name = "Non-agressive ambhibious animal enclosure",
                    SecurityLevel = SecurityLevel.Low,
                    Size = random.Next(500, 2000),
                },
                new Enclosure
                {
                    Id = 5,
                    Name = "Agressive flying animal enclosure",
                    SecurityLevel = SecurityLevel.Medium,
                    Size = random.Next(500, 2000),
                },
                new Enclosure
                {
                    Id = 6,
                    Name = "Non-agressive flying animal enclosure",
                    SecurityLevel = SecurityLevel.Medium,
                    Size = random.Next(500, 2000),
                },
                new Enclosure
                {
                    Id = 7,
                    Name = "Agressive sea animal enclosure",
                    SecurityLevel = SecurityLevel.High,
                    Size = random.Next(500, 2000),
                },
                new Enclosure {
                    Id = 8,
                    Name = "Non-agressive sea animal enclosure",
                    SecurityLevel = SecurityLevel.Medium,
                    Size = random.Next(500, 2000),
                },
            };
            modelBuilder.Entity<Enclosure>().HasData(enclosures);

            // Bogus kan geen diernamen verzinnen dus doe ik het maar.
            // Moet een paar mappers aanmaken zodat bijvoorbeeld leeuwen niet als reptiel worden opgeslagen.
            var speciesMap = new Dictionary<string, (Size Size, double SpaceRequirement, SecurityLevel SecurityRequirement, int CategoryId, int EnclosureId)>
            {
                { "Lion", (Size.Large, 8.0, SecurityLevel.High, 1, 1) },
                { "Elephant", (Size.Large, 20.0, SecurityLevel.Medium, 1, 2) },
                { "Zebra", (Size.Medium, 6.0, SecurityLevel.Low, 1, 2) },
                { "Giraffe", (Size.Large, 12.0, SecurityLevel.Medium, 1, 2) },
                { "Bonobo", (Size.Small, 3.0, SecurityLevel.Medium, 1, 2) },
                { "Otter", (Size.VerySmall, 1.5, SecurityLevel.Medium, 1, 4) },
                { "Bat", (Size.VerySmall, 0.8, SecurityLevel.Low, 1, 5) },
                { "Parrot", (Size.VerySmall, 0.8, SecurityLevel.Low, 2, 6) },
                { "Eagle", (Size.Medium, 3.0, SecurityLevel.Medium, 2, 5) },
                { "Owl", (Size.VerySmall, 1.0, SecurityLevel.Low, 2, 6) },
                { "Cobra", (Size.Medium, 2.5, SecurityLevel.High, 3, 1) },
                { "Iguana", (Size.VerySmall, 1.0, SecurityLevel.Medium, 3, 2) },
                { "Crocodile", (Size.Large, 10.0, SecurityLevel.High, 3, 3) },
                { "Dolphin", (Size.Large, 12.0, SecurityLevel.High, 1, 8) },
                { "Great White Shark", (Size.VeryLarge, 25.0, SecurityLevel.High, 4, 7) },
                { "Penguin", (Size.VerySmall, 1.2, SecurityLevel.Low, 2, 4) },
                { "Polar Bear", (Size.Large, 12.0, SecurityLevel.High, 1, 3) },
            };

            var species = speciesMap.Keys.ToList(); // Niet DRY

            // Seeden met een faker
            var faker = new Faker<Animal>("nl")
                .RuleFor(animal => animal.Id, fake => fake.IndexFaker + 1)
                .RuleFor(animal => animal.Name, fake => fake.Name.FirstName())
                .RuleFor(animal => animal.Species, fake => fake.PickRandom(species))
                .RuleFor(animal => animal.Size, (fake, animal) => speciesMap[animal.Species].Size)
                .RuleFor(animal => animal.SpaceRequirement, (fake, animal) => speciesMap[animal.Species].SpaceRequirement)
                .RuleFor(animal => animal.SecurityRequirement, (fake, animal) => speciesMap[animal.Species].SecurityRequirement)
                .RuleFor(animal => animal.CategoryId, (fake, animal) => speciesMap[animal.Species].CategoryId)
                .RuleFor(animal => animal.EnclosureId, (fake, animal) => speciesMap[animal.Species].EnclosureId)
                .RuleFor(animal => animal.DietaryClass, fake => fake.PickRandom<DietaryClass>())
                .RuleFor(animal => animal.ActivityPattern, fake => fake.PickRandom<ActivityPattern>());

            var animals = faker.Generate(20);
            modelBuilder.Entity<Animal>().HasData(animals);
        }
    }
}
