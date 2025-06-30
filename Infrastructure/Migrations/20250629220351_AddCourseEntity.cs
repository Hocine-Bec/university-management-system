using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCourseEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "courses",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    code = table.Column<string>(type: "varchar", maxLength: 10, nullable: false),
                    title = table.Column<string>(type: "varchar", maxLength: 50, nullable: false),
                    description = table.Column<string>(type: "varchar", maxLength: 1000, nullable: true),
                    credit_hours = table.Column<int>(type: "integer", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_courses", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "ix_courses_code",
                table: "courses",
                column: "code",
                unique: true);
            
            // Seed data for courses
            migrationBuilder.InsertData(
                table: "courses",
                columns: new[] { "code", "title", "description", "credit_hours", "is_active" },
                values: new object[,]
            {
                // Computer Science Courses
                {
                    "CS101", 
                    "Introduction to Computer Science", 
                    "Fundamentals of computing and problem solving with algorithms", 
                    4, 
                    true
                },
                {
                    "CS201", 
                    "Data Structures", 
                    "Study of fundamental data structures and their applications", 
                    4, 
                    true
                },
                {
                    "CS301", 
                    "Algorithms", 
                    "Design and analysis of efficient algorithms", 
                    4, 
                    true
                },
                {
                    "CS401", 
                    "Database Systems", 
                    "Principles of database design and implementation", 
                    3, 
                    true
                },
    
                // Mathematics Courses
                {
                    "MATH101", 
                    "Calculus I", 
                    "Differential and integral calculus of single variable functions", 
                    4, 
                    true
                },
                {
                    "MATH201", 
                    "Linear Algebra", 
                    "Vector spaces, linear transformations, and matrices", 
                    3, 
                    true
                },
                {
                    "MATH301", 
                    "Discrete Mathematics", 
                    "Mathematical structures fundamental to computer science", 
                    3, 
                    true
                },
    
                // Business Courses
                {
                    "BUS101", 
                    "Principles of Management", 
                    "Introduction to management theory and practice", 
                    3, 
                    true
                },
                {
                    "BUS201", 
                    "Financial Accounting", 
                    "Fundamentals of financial reporting and analysis", 
                    3, 
                    true
                },
                {
                    "BUS301", 
                    "Business Ethics", 
                    "Ethical decision-making in business contexts", 
                    2, 
                    true
                },
    
                // Engineering Courses
                {
                    "ENG101", 
                    "Engineering Fundamentals", 
                    "Introduction to engineering principles and design", 
                    4, 
                    true
                },
                {
                    "ENG201", 
                    "Circuit Theory", 
                    "Analysis of electrical circuits and networks", 
                    4, 
                    true
                },
                {
                    "ENG301", 
                    "Thermodynamics", 
                    "Principles of energy conversion and heat transfer", 
                    3, 
                    true
                },
    
                // Inactive/Archived Courses
                {
                    "CS105", 
                    "Legacy Programming", 
                    "Deprecated programming concepts (no longer offered)", 
                    3, 
                    false
                },
                {
                    "MATH105", 
                    "Basic Arithmetic", 
                    "Remedial math course (replaced by MATH100)", 
                    2, 
                    false
                }
            });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "courses");

            migrationBuilder.RenameColumn(
                name: "enrollment_status",
                table: "enrollments",
                newName: "Status");
        }
    }
}
