using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ImproveEnrollmentTracking : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "graduation_date",
                table: "enrollments",
                newName: "actual_grad_date");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "enrollments",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "enrollments");

            migrationBuilder.RenameColumn(
                name: "actual_grad_date",
                table: "enrollments",
                newName: "graduation_date");
        }
    }
}
