using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Reteteculinare.Migrations
{
    /// <inheritdoc />
    public partial class RemoveCategoryID : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ingredient_Recipe_RecipeID",
                table: "Ingredient");

            migrationBuilder.DropForeignKey(
                name: "FK_Recipe_Category_CategoryID",
                table: "Recipe");

            migrationBuilder.DropTable(
                name: "Category");

            migrationBuilder.DropIndex(
                name: "IX_Recipe_CategoryID",
                table: "Recipe");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Ingredient",
                table: "Ingredient");

            migrationBuilder.DropColumn(
                name: "CategoryID",
                table: "Recipe");

            migrationBuilder.RenameTable(
                name: "Ingredient",
                newName: "Ingredients");

            migrationBuilder.RenameIndex(
                name: "IX_Ingredient_RecipeID",
                table: "Ingredients",
                newName: "IX_Ingredients_RecipeID");

            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "Recipe",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Ingredients",
                table: "Ingredients",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Ingredients_Recipe_RecipeID",
                table: "Ingredients",
                column: "RecipeID",
                principalTable: "Recipe",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ingredients_Recipe_RecipeID",
                table: "Ingredients");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Ingredients",
                table: "Ingredients");

            migrationBuilder.DropColumn(
                name: "Category",
                table: "Recipe");

            migrationBuilder.RenameTable(
                name: "Ingredients",
                newName: "Ingredient");

            migrationBuilder.RenameIndex(
                name: "IX_Ingredients_RecipeID",
                table: "Ingredient",
                newName: "IX_Ingredient_RecipeID");

            migrationBuilder.AddColumn<int>(
                name: "CategoryID",
                table: "Recipe",
                type: "int",
                maxLength: 50,
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Ingredient",
                table: "Ingredient",
                column: "ID");

            migrationBuilder.CreateTable(
                name: "Category",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Category", x => x.ID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Recipe_CategoryID",
                table: "Recipe",
                column: "CategoryID");

            migrationBuilder.AddForeignKey(
                name: "FK_Ingredient_Recipe_RecipeID",
                table: "Ingredient",
                column: "RecipeID",
                principalTable: "Recipe",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Recipe_Category_CategoryID",
                table: "Recipe",
                column: "CategoryID",
                principalTable: "Category",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
