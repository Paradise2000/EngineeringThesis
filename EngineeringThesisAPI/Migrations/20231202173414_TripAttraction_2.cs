using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EngineeringThesisAPI.Migrations
{
    /// <inheritdoc />
    public partial class TripAttraction_2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TripAttraction_Attractions_AttractionId",
                table: "TripAttraction");

            migrationBuilder.DropForeignKey(
                name: "FK_TripAttraction_Users_UserId",
                table: "TripAttraction");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TripAttraction",
                table: "TripAttraction");

            migrationBuilder.RenameTable(
                name: "TripAttraction",
                newName: "TripAttractions");

            migrationBuilder.RenameIndex(
                name: "IX_TripAttraction_UserId",
                table: "TripAttractions",
                newName: "IX_TripAttractions_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_TripAttraction_AttractionId",
                table: "TripAttractions",
                newName: "IX_TripAttractions_AttractionId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TripAttractions",
                table: "TripAttractions",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TripAttractions_Attractions_AttractionId",
                table: "TripAttractions",
                column: "AttractionId",
                principalTable: "Attractions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TripAttractions_Users_UserId",
                table: "TripAttractions",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TripAttractions_Attractions_AttractionId",
                table: "TripAttractions");

            migrationBuilder.DropForeignKey(
                name: "FK_TripAttractions_Users_UserId",
                table: "TripAttractions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TripAttractions",
                table: "TripAttractions");

            migrationBuilder.RenameTable(
                name: "TripAttractions",
                newName: "TripAttraction");

            migrationBuilder.RenameIndex(
                name: "IX_TripAttractions_UserId",
                table: "TripAttraction",
                newName: "IX_TripAttraction_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_TripAttractions_AttractionId",
                table: "TripAttraction",
                newName: "IX_TripAttraction_AttractionId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TripAttraction",
                table: "TripAttraction",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TripAttraction_Attractions_AttractionId",
                table: "TripAttraction",
                column: "AttractionId",
                principalTable: "Attractions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TripAttraction_Users_UserId",
                table: "TripAttraction",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
