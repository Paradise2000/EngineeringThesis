using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EngineeringThesisAPI.Migrations
{
    /// <inheritdoc />
    public partial class cascadeDeleteAttraction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Attractions_AttractionId",
                table: "Comments");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Attractions_AttractionId",
                table: "Comments",
                column: "AttractionId",
                principalTable: "Attractions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Attractions_AttractionId",
                table: "Comments");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Attractions_AttractionId",
                table: "Comments",
                column: "AttractionId",
                principalTable: "Attractions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
