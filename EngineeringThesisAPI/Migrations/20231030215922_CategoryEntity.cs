using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EngineeringThesisAPI.Migrations
{
    /// <inheritdoc />
    public partial class CategoryEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "Attractions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Category",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Category", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Attractions_CategoryId",
                table: "Attractions",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Attractions_Category_CategoryId",
                table: "Attractions",
                column: "CategoryId",
                principalTable: "Category",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attractions_Category_CategoryId",
                table: "Attractions");

            migrationBuilder.DropTable(
                name: "Category");

            migrationBuilder.DropIndex(
                name: "IX_Attractions_CategoryId",
                table: "Attractions");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "Attractions");
        }
    }
}
