using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QLThuVien.Data.Migrations
{
    /// <inheritdoc />
    public partial class FixCols : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Count",
                table: "Books",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Count",
                table: "Books");
        }
    }
}
