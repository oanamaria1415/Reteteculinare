﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Reteteculinare.Migrations
{
    /// <inheritdoc />
    public partial class AddImagePathToRecipe : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImagePath",
                table: "Recipe",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImagePath",
                table: "Recipe");
        }
    }
}
