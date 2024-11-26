using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AT.Server.Migrations
{
    /// <inheritdoc />
    public partial class UpdateStaff : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SummaryName",
                table: "Staffs",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SummaryName",
                table: "Staffs");
        }
    }
}
