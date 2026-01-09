using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace csharp_groep31.Migrations
{
    /// <inheritdoc />
    public partial class Groep31 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Enclosures",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Climate = table.Column<int>(type: "int", nullable: false),
                    HabitatType = table.Column<int>(type: "int", nullable: false),
                    SecurityLevel = table.Column<int>(type: "int", nullable: false),
                    Size = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Enclosures", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Animals",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Species = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: true),
                    Size = table.Column<int>(type: "int", nullable: false),
                    DietaryClass = table.Column<int>(type: "int", nullable: false),
                    ActivityPattern = table.Column<int>(type: "int", nullable: false),
                    Prey = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EnclosureId = table.Column<int>(type: "int", nullable: true),
                    SpaceRequirement = table.Column<double>(type: "float", nullable: false),
                    SecurityRequirement = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Animals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Animals_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Animals_Enclosures_EnclosureId",
                        column: x => x.EnclosureId,
                        principalTable: "Enclosures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Mammals" },
                    { 2, "Birds" },
                    { 3, "Reptiles" },
                    { 4, "Fish" }
                });

            migrationBuilder.InsertData(
                table: "Enclosures",
                columns: new[] { "Id", "Climate", "HabitatType", "Name", "SecurityLevel", "Size" },
                values: new object[,]
                {
                    { 1, 0, 0, "Agressive land animal enclosure", 2, 1502.0 },
                    { 2, 0, 0, "Non-agressive land animal enclosure", 1, 711.0 },
                    { 3, 0, 0, "Agressive ambhibious animal enclosure", 2, 688.0 },
                    { 4, 0, 0, "Non-agressive ambhibious animal enclosure", 0, 1284.0 },
                    { 5, 0, 0, "Agressive flying animal enclosure", 1, 752.0 },
                    { 6, 0, 0, "Non-agressive flying animal enclosure", 1, 893.0 },
                    { 7, 0, 0, "Agressive sea animal enclosure", 2, 1586.0 },
                    { 8, 0, 0, "Non-agressive sea animal enclosure", 1, 1269.0 }
                });

            migrationBuilder.InsertData(
                table: "Animals",
                columns: new[] { "Id", "ActivityPattern", "CategoryId", "DietaryClass", "EnclosureId", "Name", "Prey", "SecurityRequirement", "Size", "SpaceRequirement", "Species" },
                values: new object[,]
                {
                    { 1, 1, 2, 0, 6, "Thijs", null, 0, 1, 0.80000000000000004, "Parrot" },
                    { 2, 2, 1, 0, 3, "Jan", null, 2, 4, 12.0, "Polar Bear" },
                    { 3, 2, 1, 3, 2, "Max", null, 1, 4, 12.0, "Giraffe" },
                    { 4, 0, 1, 3, 2, "Eva", null, 0, 3, 6.0, "Zebra" },
                    { 5, 2, 2, 0, 6, "Thomas", null, 0, 1, 0.80000000000000004, "Parrot" },
                    { 6, 1, 1, 1, 2, "Daan", null, 1, 2, 3.0, "Bonobo" },
                    { 7, 2, 2, 2, 6, "Sanne", null, 0, 1, 1.0, "Owl" },
                    { 8, 2, 1, 0, 1, "Thijs", null, 2, 4, 8.0, "Lion" },
                    { 9, 1, 3, 1, 2, "Anouk", null, 1, 1, 1.0, "Iguana" },
                    { 10, 2, 3, 3, 3, "Jayden", null, 2, 4, 10.0, "Crocodile" },
                    { 11, 0, 4, 1, 7, "Sem", null, 2, 5, 25.0, "Great White Shark" },
                    { 12, 0, 1, 3, 3, "Daan", null, 2, 4, 12.0, "Polar Bear" },
                    { 13, 0, 1, 4, 3, "Bas", null, 2, 4, 12.0, "Polar Bear" },
                    { 14, 1, 3, 3, 2, "Rick", null, 1, 1, 1.0, "Iguana" },
                    { 15, 1, 2, 1, 6, "Lucas", null, 0, 1, 0.80000000000000004, "Parrot" },
                    { 16, 2, 3, 4, 3, "Bram", null, 2, 4, 10.0, "Crocodile" },
                    { 17, 0, 2, 4, 4, "Lars", null, 0, 1, 1.2, "Penguin" },
                    { 18, 0, 1, 2, 3, "Bram", null, 2, 4, 12.0, "Polar Bear" },
                    { 19, 0, 1, 4, 5, "Anne", null, 0, 1, 0.80000000000000004, "Bat" },
                    { 20, 1, 1, 2, 3, "Lucas", null, 2, 4, 12.0, "Polar Bear" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Animals_CategoryId",
                table: "Animals",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Animals_EnclosureId",
                table: "Animals",
                column: "EnclosureId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Animals");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Enclosures");
        }
    }
}
