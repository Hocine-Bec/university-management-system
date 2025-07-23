using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPrerequisiteEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "prerequisites",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    minimum_grade = table.Column<decimal>(type: "numeric(4,2)", nullable: false),
                    course_id = table.Column<int>(type: "integer", nullable: false),
                    prerequisite_course_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_prerequisites", x => x.id);
                    table.ForeignKey(
                        name: "fk_prerequisites_course",
                        column: x => x.course_id,
                        principalTable: "courses",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_prerequisites_prerequisite_course",
                        column: x => x.prerequisite_course_id,
                        principalTable: "courses",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "ix_prerequisites_course_prerequisite",
                table: "prerequisites",
                columns: new[] { "course_id", "prerequisite_course_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_prerequisites_prerequisite_course_id",
                table: "prerequisites",
                column: "prerequisite_course_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "prerequisites");
        }
    }
}
