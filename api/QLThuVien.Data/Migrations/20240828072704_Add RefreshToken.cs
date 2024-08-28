using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace QLThuVien.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddRefreshToken : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("a6f258bf-6ef8-4159-97aa-9aa14610bcea"), new Guid("3917d549-0582-448d-be51-4b53b90842c8") });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("9dd84572-e545-4fb8-8d28-ea5ecf5e137d"), new Guid("8b7e0cc7-7a11-4854-bdef-39455912be81") });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("9dd84572-e545-4fb8-8d28-ea5ecf5e137d"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("a6f258bf-6ef8-4159-97aa-9aa14610bcea"));

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("3917d549-0582-448d-be51-4b53b90842c8"));

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("8b7e0cc7-7a11-4854-bdef-39455912be81"));

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Categories",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateTable(
                name: "RefreshTokens",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Token = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    JwtId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsRevoked = table.Column<bool>(type: "bit", nullable: false),
                    DateAdded = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateExpire = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshTokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RefreshTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Description", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("832212c3-0ef1-4018-bb6b-e84b10a1dc2b"), null, "Thủ thư", "admin", "admin" },
                    { new Guid("a09e161c-68fa-454c-b59e-826190c609bb"), null, "Thành viên", "member", "member" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "DateOfBirth", "Email", "EmailConfirmed", "FullName", "Gender", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { new Guid("3906aea5-b99a-4631-92c4-95bb006d3b21"), 0, "85813ac9-e3af-48b8-b2aa-89926b783a74", new DateOnly(2024, 8, 28), "admin@gmail.com", false, "admin 123", "male", false, null, null, null, null, null, false, null, false, null },
                    { new Guid("a335f4aa-492a-4644-a2a6-7aa3a8070e92"), 0, "918be849-1e49-4e1c-a425-fa83e6b29005", new DateOnly(2024, 8, 28), "member1@gmail.com", false, "Member1 Name", "female", false, null, null, null, null, null, false, null, false, null }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { new Guid("832212c3-0ef1-4018-bb6b-e84b10a1dc2b"), new Guid("3906aea5-b99a-4631-92c4-95bb006d3b21") },
                    { new Guid("a09e161c-68fa-454c-b59e-826190c609bb"), new Guid("a335f4aa-492a-4644-a2a6-7aa3a8070e92") }
                });

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_UserId",
                table: "RefreshTokens",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RefreshTokens");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("832212c3-0ef1-4018-bb6b-e84b10a1dc2b"), new Guid("3906aea5-b99a-4631-92c4-95bb006d3b21") });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("a09e161c-68fa-454c-b59e-826190c609bb"), new Guid("a335f4aa-492a-4644-a2a6-7aa3a8070e92") });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("832212c3-0ef1-4018-bb6b-e84b10a1dc2b"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("a09e161c-68fa-454c-b59e-826190c609bb"));

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("3906aea5-b99a-4631-92c4-95bb006d3b21"));

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("a335f4aa-492a-4644-a2a6-7aa3a8070e92"));

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Categories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Description", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("9dd84572-e545-4fb8-8d28-ea5ecf5e137d"), null, "Thủ thư", "admin", "admin" },
                    { new Guid("a6f258bf-6ef8-4159-97aa-9aa14610bcea"), null, "Thành viên", "member", "member" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "DateOfBirth", "Email", "EmailConfirmed", "FullName", "Gender", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { new Guid("3917d549-0582-448d-be51-4b53b90842c8"), 0, "791d1311-369d-4000-8d49-e1625cfc3aa9", new DateOnly(2024, 8, 27), "member1@gmail.com", false, "Member1 Name", "female", false, null, null, null, null, null, false, null, false, null },
                    { new Guid("8b7e0cc7-7a11-4854-bdef-39455912be81"), 0, "1508d3a9-7c47-4861-8b3d-645f02394fde", new DateOnly(2024, 8, 27), "admin@gmail.com", false, "admin 123", "male", false, null, null, null, null, null, false, null, false, null }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { new Guid("a6f258bf-6ef8-4159-97aa-9aa14610bcea"), new Guid("3917d549-0582-448d-be51-4b53b90842c8") },
                    { new Guid("9dd84572-e545-4fb8-8d28-ea5ecf5e137d"), new Guid("8b7e0cc7-7a11-4854-bdef-39455912be81") }
                });
        }
    }
}
