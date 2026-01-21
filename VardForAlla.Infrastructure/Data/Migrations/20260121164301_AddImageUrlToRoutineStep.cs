using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VardForAlla.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddImageUrlToRoutineStep : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "RoutineSteps",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "RoutineSteps");
        }
    }
}
