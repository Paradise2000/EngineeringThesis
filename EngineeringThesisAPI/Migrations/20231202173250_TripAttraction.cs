using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EngineeringThesisAPI.Migrations
{
    /// <inheritdoc />
    public partial class TripAttraction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TripAttraction",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AttractionId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TripAttraction", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TripAttraction_Attractions_AttractionId",
                        column: x => x.AttractionId,
                        principalTable: "Attractions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TripAttraction_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TripAttraction_AttractionId",
                table: "TripAttraction",
                column: "AttractionId");

            migrationBuilder.CreateIndex(
                name: "IX_TripAttraction_UserId",
                table: "TripAttraction",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TripAttraction");
        }
    }
}
