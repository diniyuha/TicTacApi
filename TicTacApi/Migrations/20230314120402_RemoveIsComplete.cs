using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TicTacApi.Migrations
{
    /// <inheritdoc />
    public partial class RemoveIsComplete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsComplete",
                table: "Games");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsComplete",
                table: "Games",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
