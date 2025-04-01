using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class v4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Services",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Services", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "9f4c408b-8272-420c-becd-647ebdc58fae", "AQAAAAIAAYagAAAAEHOIMYtk0aHeipo+XUtBpX4HnZae7M6c848W2kJ1Rbu3jCU3ZEl2Flc4u71I9ov78Q==", "e39451cb-f204-4483-adb8-8a2285c0ce8a" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Services");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "b21bda5c-9e66-4ed2-85e0-4b78e10821bc", "AQAAAAIAAYagAAAAEC+2IMe9eSkccfToKHGmDXW1Jt/f9Ffwyaz+FvOmYvINOvqwn7y+qFDAkfpc0AmgXg==", "803cf98a-e3a4-40be-82d0-d5c33672c86c" });
        }
    }
}
