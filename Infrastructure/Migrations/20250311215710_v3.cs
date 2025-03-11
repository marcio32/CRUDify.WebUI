using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class v3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "Age", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "1", 0, 0, "b21bda5c-9e66-4ed2-85e0-4b78e10821bc", "marcioabriola@gmail.com", true, "", "", false, null, "marcioabriola@gmail.com", null, "AQAAAAIAAYagAAAAEC+2IMe9eSkccfToKHGmDXW1Jt/f9Ffwyaz+FvOmYvINOvqwn7y+qFDAkfpc0AmgXg==", null, false, "803cf98a-e3a4-40be-82d0-d5c33672c86c", false, "admin" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1");
        }
    }
}
