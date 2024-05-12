using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NutriFitLogBackend.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangeMealFood : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MealFoods_DayParts_DayPartId",
                table: "MealFoods");

            migrationBuilder.DropForeignKey(
                name: "FK_MealFoods_Foods_FoodId",
                table: "MealFoods");

            migrationBuilder.DropColumn(
                name: "PortionDescription",
                table: "MealFoods");

            migrationBuilder.AddForeignKey(
                name: "FK_MealFoods_DayParts_DayPartId",
                table: "MealFoods",
                column: "DayPartId",
                principalTable: "DayParts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MealFoods_Foods_FoodId",
                table: "MealFoods",
                column: "FoodId",
                principalTable: "Foods",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MealFoods_DayParts_DayPartId",
                table: "MealFoods");

            migrationBuilder.DropForeignKey(
                name: "FK_MealFoods_Foods_FoodId",
                table: "MealFoods");

            migrationBuilder.AddColumn<string>(
                name: "PortionDescription",
                table: "MealFoods",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_MealFoods_DayParts_DayPartId",
                table: "MealFoods",
                column: "DayPartId",
                principalTable: "DayParts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MealFoods_Foods_FoodId",
                table: "MealFoods",
                column: "FoodId",
                principalTable: "Foods",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
