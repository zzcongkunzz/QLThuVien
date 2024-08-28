using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace QLThuVien.Data.Migrations
{
    /// <inheritdoc />
    public partial class MergePenalty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Penalties");

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

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Borrows");

            migrationBuilder.RenameColumn(
                name: "StartDate",
                table: "Borrows",
                newName: "StartTime");

            migrationBuilder.RenameColumn(
                name: "ExpectedReturnDate",
                table: "Borrows",
                newName: "ExpectedReturnTime");

            migrationBuilder.RenameColumn(
                name: "ActualReturnDate",
                table: "Borrows",
                newName: "ActualReturnTime");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Categories",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "Count",
                table: "Borrows",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<float>(
                name: "IssuedPenalties",
                table: "Borrows",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "PaidPenalties",
                table: "Borrows",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AlterColumn<DateOnly>(
                name: "PublishDate",
                table: "Books",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Books",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "PublisherName",
                table: "Books",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Description", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("45444bae-4339-4d3e-9732-6de4b30a2729"), null, "Thủ thư", "admin", "admin" },
                    { new Guid("aefb78f9-ac50-49d7-bf91-4c7dc60d3d65"), null, "Thành viên", "member", "member" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "DateOfBirth", "Email", "EmailConfirmed", "FullName", "Gender", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { new Guid("8c69534c-0ed7-4467-96e1-466fc1745872"), 0, "1274ae25-c04a-43d5-b195-6150bbed91c7", new DateOnly(2024, 8, 28), "member1@gmail.com", false, "Member1 Name", "female", false, null, null, null, null, null, false, null, false, null },
                    { new Guid("cd92595d-f9f5-4e5c-9973-8a4a40f1ea7a"), 0, "3de5345f-adb5-4fe3-9132-0fee2207518a", new DateOnly(2024, 8, 28), "admin@gmail.com", false, "admin 123", "male", false, null, null, null, null, null, false, null, false, null }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { new Guid("aefb78f9-ac50-49d7-bf91-4c7dc60d3d65"), new Guid("8c69534c-0ed7-4467-96e1-466fc1745872") },
                    { new Guid("45444bae-4339-4d3e-9732-6de4b30a2729"), new Guid("cd92595d-f9f5-4e5c-9973-8a4a40f1ea7a") }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("aefb78f9-ac50-49d7-bf91-4c7dc60d3d65"), new Guid("8c69534c-0ed7-4467-96e1-466fc1745872") });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("45444bae-4339-4d3e-9732-6de4b30a2729"), new Guid("cd92595d-f9f5-4e5c-9973-8a4a40f1ea7a") });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("45444bae-4339-4d3e-9732-6de4b30a2729"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("aefb78f9-ac50-49d7-bf91-4c7dc60d3d65"));

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("8c69534c-0ed7-4467-96e1-466fc1745872"));

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("cd92595d-f9f5-4e5c-9973-8a4a40f1ea7a"));

            migrationBuilder.DropColumn(
                name: "Count",
                table: "Borrows");

            migrationBuilder.DropColumn(
                name: "IssuedPenalties",
                table: "Borrows");

            migrationBuilder.DropColumn(
                name: "PaidPenalties",
                table: "Borrows");

            migrationBuilder.DropColumn(
                name: "PublisherName",
                table: "Books");

            migrationBuilder.RenameColumn(
                name: "StartTime",
                table: "Borrows",
                newName: "StartDate");

            migrationBuilder.RenameColumn(
                name: "ExpectedReturnTime",
                table: "Borrows",
                newName: "ExpectedReturnDate");

            migrationBuilder.RenameColumn(
                name: "ActualReturnTime",
                table: "Borrows",
                newName: "ActualReturnDate");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Categories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Borrows",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<DateTime>(
                name: "PublishDate",
                table: "Books",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateOnly),
                oldType: "date");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Books",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "Penalties",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BorrowId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Fees = table.Column<float>(type: "real", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: false),
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
                name: "IX_Penalties_BorrowId",
                table: "Penalties",
                column: "BorrowId");
        }
    }
}
