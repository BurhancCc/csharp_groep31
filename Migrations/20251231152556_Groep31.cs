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
                    { 1, 0, 3, 0, 3, "Milan", null, 0, 4, 0.0, "Crocodile" },
                    { 2, 0, 1, 0, 3, "Max", null, 0, 4, 0.0, "Polar Bear" },
                    { 3, 0, 3, 0, 1, "Kevin", null, 0, 3, 0.0, "Cobra" },
                    { 4, 0, 1, 0, 1, "Rick", null, 0, 4, 0.0, "Lion" },
                    { 5, 0, 2, 0, 6, "Emma", null, 0, 1, 0.0, "Parrot" },
                    { 6, 0, 3, 0, 3, "Fleur", null, 0, 4, 0.0, "Crocodile" },
                    { 7, 0, 3, 0, 2, "Max", null, 0, 1, 0.0, "Iguana" },
                    { 8, 0, 3, 0, 2, "Sander", null, 0, 1, 0.0, "Iguana" },
                    { 9, 0, 3, 0, 3, "Kevin", null, 0, 4, 0.0, "Crocodile" },
                    { 10, 0, 1, 0, 4, "Lotte", null, 0, 1, 0.0, "Otter" },
                    { 11, 0, 1, 0, 8, "Niels", null, 0, 4, 0.0, "Dolphin" },
                    { 12, 0, 2, 0, 6, "Jesse", null, 0, 1, 0.0, "Owl" },
                    { 13, 0, 1, 0, 8, "Rick", null, 0, 4, 0.0, "Dolphin" },
                    { 14, 0, 1, 0, 2, "Kevin", null, 0, 2, 0.0, "Bonobo" },
                    { 15, 0, 1, 0, 2, "Sander", null, 0, 4, 0.0, "Giraffe" },
                    { 16, 0, 2, 0, 5, "Julian", null, 0, 3, 0.0, "Eagle" },
                    { 17, 0, 1, 0, 8, "Nick", null, 0, 4, 0.0, "Dolphin" },
                    { 18, 0, 3, 0, 3, "Tim", null, 0, 4, 0.0, "Crocodile" },
                    { 19, 0, 1, 0, 1, "Bram", null, 0, 4, 0.0, "Lion" },
                    { 20, 0, 1, 0, 2, "Lucas", null, 0, 2, 0.0, "Bonobo" }
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
