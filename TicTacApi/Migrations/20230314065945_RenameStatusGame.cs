using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TicTacApi.Migrations
{
    /// <inheritdoc />
    public partial class RenameStatusGame : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Status",
                table: "Games",
                newName: "StatusGame");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "StatusGame",
                table: "Games",
                newName: "Status");
        }
    }
}
