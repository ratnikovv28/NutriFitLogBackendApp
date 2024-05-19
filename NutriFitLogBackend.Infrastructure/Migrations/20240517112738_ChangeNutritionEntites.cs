using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NutriFitLogBackend.Infrastructure.Migrations
{
    /// <inheritdoc />
    [ExcludeFromCodeCoverage]
    public partial class ChangeNutritionEntites : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Unit",
                table: "MealFoods");

            migrationBuilder.AddColumn<int>(
                name: "Unit",
                table: "Foods",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Unit",
                table: "Foods");

            migrationBuilder.AddColumn<int>(
                name: "Unit",
                table: "MealFoods",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
