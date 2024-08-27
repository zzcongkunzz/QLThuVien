using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace QLThuVien.Data.Migrations
{
    /// <inheritdoc />
    public partial class UserRoleSeeded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RoleUser");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "DateOfBirth",
                table: "AspNetUsers",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Description", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("302dc5bd-88a1-42eb-b7ff-45ddbc4ae4a3"), null, "Thủ thư", "admin", "admin" },
                    { new Guid("bd61beee-6fea-4252-8a83-eac3d961e726"), null, "Thành viên", "member", "member" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "DateOfBirth", "Email", "EmailConfirmed", "FullName", "Gender", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { new Guid("20735d6c-1978-4d34-97f8-d1b13f39d863"), 0, "bd6a6d31-b654-44a3-8a59-e8f41d89b656", new DateOnly(2024, 8, 27), "admin@gmail.com", false, "admin 123", "male", false, null, null, null, null, null, false, null, false, null },
                    { new Guid("d11d9a00-cd9c-4564-bfbb-02fa0fbd5251"), 0, "358047b2-7cfa-4617-a022-3b5e8e12539b", new DateOnly(2024, 8, 27), "member1@gmail.com", false, "Member1 Name", "female", false, null, null, null, null, null, false, null, false, null }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { new Guid("302dc5bd-88a1-42eb-b7ff-45ddbc4ae4a3"), new Guid("20735d6c-1978-4d34-97f8-d1b13f39d863") },
                    { new Guid("bd61beee-6fea-4252-8a83-eac3d961e726"), new Guid("d11d9a00-cd9c-4564-bfbb-02fa0fbd5251") }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("302dc5bd-88a1-42eb-b7ff-45ddbc4ae4a3"), new Guid("20735d6c-1978-4d34-97f8-d1b13f39d863") });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("bd61beee-6fea-4252-8a83-eac3d961e726"), new Guid("d11d9a00-cd9c-4564-bfbb-02fa0fbd5251") });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("302dc5bd-88a1-42eb-b7ff-45ddbc4ae4a3"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("bd61beee-6fea-4252-8a83-eac3d961e726"));

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("20735d6c-1978-4d34-97f8-d1b13f39d863"));

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("d11d9a00-cd9c-4564-bfbb-02fa0fbd5251"));

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateOfBirth",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateOnly),
                oldType: "date");

            migrationBuilder.CreateTable(
                name: "RoleUser",
                columns: table => new
                {
                    RolesId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UsersId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleUser", x => new { x.RolesId, x.UsersId });
                    table.ForeignKey(
                        name: "FK_RoleUser_AspNetRoles_RolesId",
                        column: x => x.RolesId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RoleUser_AspNetUsers_UsersId",
                        column: x => x.UsersId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RoleUser_UsersId",
                table: "RoleUser",
                column: "UsersId");
        }
    }
}
