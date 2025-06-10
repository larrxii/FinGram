using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinGram.Data.Migrations
{
    /// <inheritdoc />
    public partial class SyncWithRepo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "FinGramUsers",
                columns: new[] { "Id", "BirthDate", "Country", "FirstName", "LastName", "Login", "UserPasswordHash" },
                values: new object[] { 2, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Russia", "Camilla", "Trifonova", "larrx", "hashedpassword" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "FinGramUsers",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
