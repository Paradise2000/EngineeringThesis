using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EngineeringThesisAPI.Migrations
{
    /// <inheritdoc />
    public partial class MainPhotoId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MainPhotoId",
                table: "Attractions",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MainPhotoId",
                table: "Attractions");
        }
    }
}
