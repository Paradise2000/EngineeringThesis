using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EngineeringThesisAPI.Migrations
{
    /// <inheritdoc />
    public partial class CommentDate1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "date",
                table: "Comments",
                newName: "Date");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Date",
                table: "Comments",
                newName: "date");
        }
    }
}
