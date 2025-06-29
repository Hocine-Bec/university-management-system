using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddSemesterEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Status",
                table: "enrollments",
                newName: "enrollment_status");

            migrationBuilder.CreateTable(
                name: "semesters",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    term_code = table.Column<string>(type: "varchar", maxLength: 8, nullable: false),
                    term = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    year = table.Column<int>(type: "integer", nullable: false),
                    start_date = table.Column<DateTime>(type: "date", nullable: false),
                    end_date = table.Column<DateTime>(type: "date", nullable: true),
                    registration_start_date = table.Column<DateTime>(type: "date", nullable: true),
                    registration_end_date = table.Column<DateTime>(type: "date", nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_semesters", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "ix_semesters_year_term",
                table: "semesters",
                columns: new[] { "year", "term" },
                unique: true);
            
            // Insert seed data
            migrationBuilder.InsertData(
                table: "semesters",
                columns: new[] { "term_code", "term", "year", "start_date", "end_date", "registration_start_date", "registration_end_date", "is_active" },
                values: new object[,]
                {
                    // Current and upcoming semesters
                    { "FA24", "Fall", 2024, new DateTime(2024, 8, 26), new DateTime(2024, 12, 13), new DateTime(2024, 4, 1), new DateTime(2024, 5, 15), true },
                    { "SU24", "Summer", 2024, new DateTime(2024, 6, 3), new DateTime(2024, 8, 9), new DateTime(2024, 3, 15), new DateTime(2024, 4, 15), true },
                    { "SP24", "Spring", 2024, new DateTime(2024, 1, 16), new DateTime(2024, 5, 10), new DateTime(2023, 11, 1), new DateTime(2023, 12, 1), false },
            
                    // Future semester (in planning)
                    { "SP25", "Spring", 2025, new DateTime(2025, 1, 21), new DateTime(2025, 5, 16), null, null, true },
            
                    // Past semesters
                    { "FA23", "Fall", 2023, new DateTime(2023, 8, 28), new DateTime(2023, 12, 15), new DateTime(2023, 4, 1), new DateTime(2023, 5, 15), false },
                    { "SU23", "Summer", 2023, new DateTime(2023, 6, 5), new DateTime(2023, 8, 11), new DateTime(2023, 3, 13), new DateTime(2023, 4, 14), false }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "semesters");

            migrationBuilder.RenameColumn(
                name: "enrollment_status",
                table: "enrollments",
                newName: "Status");
        }
    }
}
