using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ambev.Dev.Test.Data.Migrations
{
    /// <inheritdoc />
    public partial class EmployeePhonePrefixNameChanged : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Prefix",
                table: "EmployeePhones",
                newName: "PhonePrefix");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PhonePrefix",
                table: "EmployeePhones",
                newName: "Prefix");
        }
    }
}
