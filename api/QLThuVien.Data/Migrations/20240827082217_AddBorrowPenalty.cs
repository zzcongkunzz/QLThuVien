using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace QLThuVien.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddBorrowPenalty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.CreateTable(
                name: "Borrows",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BookId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExpectedReturnDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ActualReturnDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Borrows", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Borrows_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Borrows_Books_BookId",
                        column: x => x.BookId,
                        principalTable: "Books",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Penalties",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BorrowId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Fees = table.Column<float>(type: "real", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Penalties", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Penalties_Borrows_BorrowId",
                        column: x => x.BorrowId,
                        principalTable: "Borrows",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_Borrows_BookId",
                table: "Borrows",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_Borrows_UserId",
                table: "Borrows",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Penalties_BorrowId",
                table: "Penalties",
                column: "BorrowId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Penalties");

            migrationBuilder.DropTable(
                name: "Borrows");

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
    }
}
