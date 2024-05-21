using Microsoft.EntityFrameworkCore.Migrations;
using System;
namespace MyGuitarShopData.Migrations
{
    /// <inheritdoc />
    public partial class AddPasswordHashAndSalt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Assuming the Customers table is the one being updated.
            migrationBuilder.AddColumn<string>(
                name: "PasswordHash",
                table: "Customers",
                type: "varchar(255)",
                unicode: false,
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<byte[]>(
                name: "Salt",
                table: "Customers",
                type: "varbinary(16)",
                nullable: false,
                defaultValue: new byte[0]);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Revert the Customers table to its previous state by removing the columns.
            migrationBuilder.DropColumn(
                name: "PasswordHash",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "Salt",
                table: "Customers");
        }
    }
}