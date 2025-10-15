using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddRoleAuthTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "role",
                table: "users");

            migrationBuilder.CreateTable(
                name: "roles",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    role_name = table.Column<int>(type: "integer", nullable: false),
                    description = table.Column<string>(type: "varchar", maxLength: 250, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_roles", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "user_roles",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    is_active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    user_id = table.Column<int>(type: "integer", nullable: false),
                    role_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_roles", x => x.id);
                    table.ForeignKey(
                        name: "FK_user_roles_roles_role_id",
                        column: x => x.role_id,
                        principalTable: "roles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_user_roles_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "idx_role_name",
                table: "roles",
                column: "role_name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "idx_user_role",
                table: "user_roles",
                columns: new[] { "user_id", "role_id" });

            migrationBuilder.CreateIndex(
                name: "IX_user_roles_role_id",
                table: "user_roles",
                column: "role_id");
            
            // Seed SystemRole enum values into roles
            migrationBuilder.InsertData(
                table: "roles",
                columns: new[] { "id", "role_name", "description" },
                values: new object[,]
                {
                    { 1, 1, "Full system access, user management, system configuration" },
                    { 2, 2, "Department courses, faculty management, student records (department)" },
                    { 3, 3, "Own courses, student grades, course materials" },
                    { 4, 4, "Limited course access, grading assistance" },
                    { 5, 5, "Student records, academic planning, course recommendations" },
                    { 6, 6, "Application processing, student enrollment" },
                    { 7, 7, "Own records, course enrollment, grade viewing" },
                    { 8, 8, "Student organization management, event planning" },
                    { 9, 9, "Technical support, account management" },
                    { 10, 10, "Library resources, research assistance" }
                });

            // Seed user_roles with role assignments for user IDs 1-14
            migrationBuilder.InsertData(
                table: "user_roles",
                columns: new[] { "id", "user_id", "role_id", "is_active" },
                values: new object[,]
                {
                    { 1, 1, 1, true }, // Admin
                    { 2, 2, 2, true }, // DepartmentHead
                    { 3, 3, 3, true }, // Faculty
                    { 4, 4, 4, true }, // TeachingAssistant
                    { 5, 5, 5, true }, // Advisor
                    { 6, 6, 6, true }, // AdmissionsOfficer
                    { 7, 7, 7, true }, // Student
                    { 8, 8, 8, true }, // StudentLeader
                    { 9, 9, 9, true }, // ItSupport
                    { 10, 10, 10, true }, // Librarian
                    { 11, 12, 1, true } // Admin
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "user_roles");

            migrationBuilder.DropTable(
                name: "roles");

            migrationBuilder.AddColumn<int>(
                name: "role",
                table: "users",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
