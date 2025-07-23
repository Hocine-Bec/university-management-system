using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateIndexesAndSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_sections_course_id",
                table: "sections");

            migrationBuilder.DropIndex(
                name: "ix_sections_section_number",
                table: "sections");

            migrationBuilder.DropIndex(
                name: "ix_registrations_student_id",
                table: "registrations");

            migrationBuilder.DropIndex(
                name: "ix_grades_student",
                table: "grades");

            migrationBuilder.DropIndex(
                name: "ix_enrollments_student_id",
                table: "enrollments");

            migrationBuilder.CreateIndex(
                name: "ix_sections_course_semester_number",
                table: "sections",
                columns: new[] { "course_id", "semester_id", "section_number" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_registrations_student_section_semester",
                table: "registrations",
                columns: new[] { "student_id", "section_id", "semester_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_grades_student_course_semester",
                table: "grades",
                columns: new[] { "student_id", "course_id", "semester_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_financial_holds_active",
                table: "financial_holds",
                column: "is_active");

            migrationBuilder.CreateIndex(
                name: "ix_enrollments_student_program",
                table: "enrollments",
                columns: new[] { "student_id", "program_id" },
                unique: true);
            
            //Seeding Initial Data
             // Countries (no dependencies)
            migrationBuilder.InsertData(
                table: "countries",
                columns: new[] { "name", "code" },
                values: new object[,]
                {
                    { "United States", "US" },
                    { "Canada", "CA" },
                    { "United Kingdom", "UK" },
                    { "Australia", "AU" },
                    { "Germany", "DE" },
                    { "France", "FR" },
                    { "Japan", "JP" },
                    { "China", "CN" },
                    { "India", "IN" },
                    { "Brazil", "BR" }
                });

            // People (depends on countries)
            migrationBuilder.InsertData(
                table: "people",
                columns: new[] 
                { "first_name", "last_name", "date_of_birth", "address", "phone_number", "email", "image_path", "country_id" },
                values: new object[,]
                {
                    {
                        "John", "Smith", new DateTime(1995, 5, 10, 0, 0, 0, DateTimeKind.Utc), "123 Main St, Boston, MA", "+1-555-0101",
                        "john.smith@university.edu", "/images/john_smith.jpg", 1
                    },
                    {
                        "Emily", "Johnson", new DateTime(1996, 7, 15, 0, 0, 0, DateTimeKind.Utc), "456 Oak Ave, Boston, MA", "+1-555-0102",
                        "emily.johnson@university.edu", "/images/emily_johnson.jpg", 2
                    },
                    {
                        "Michael", "Williams", new DateTime(1997, 3, 22, 0, 0, 0, DateTimeKind.Utc), "789 Pine Rd, Boston, MA", "+1-555-0103",
                        "michael.williams@university.edu", "/images/michael_williams.jpg", 3
                    },
                    {
                        "Sarah", "Brown", new DateTime(1996, 9, 5, 0, 0, 0, DateTimeKind.Utc), "321 Elm St, Boston, MA", "+1-555-0104",
                        "sarah.brown@university.edu", "/images/sarah_brown.jpg", 4
                    },
                    {
                        "David", "Jones", new DateTime(1998, 11, 30, 0, 0, 0, DateTimeKind.Utc), "654 Maple Dr, Boston, MA", "+1-555-0105",
                        "david.jones@university.edu", "/images/david_jones.jpg", 5
                    },
                    {
                        "Jessica", "Garcia", new DateTime(1997, 1, 18, 0, 0, 0, DateTimeKind.Utc), "987 Cedar Ln, Boston, MA", "+1-555-0106",
                        "jessica.garcia@university.edu", "/images/jessica_garcia.jpg", 6
                    },
                    {
                        "Robert", "Miller", new DateTime(1996, 8, 12, 0, 0, 0, DateTimeKind.Utc), "135 Walnut St, Boston, MA", "+1-555-0107",
                        "robert.miller@university.edu", "/images/robert_miller.jpg", 7
                    },
                    {
                        "Jennifer", "Davis", new DateTime(1999, 4, 25, 0, 0, 0, DateTimeKind.Utc), "246 Birch Ave, Boston, MA", "+1-555-0108",
                        "jennifer.davis@university.edu", "/images/jennifer_davis.jpg", 8
                    },
                    {
                        "William", "Rodriguez", new DateTime(1995, 12, 8, 0, 0, 0, DateTimeKind.Utc), "369 Spruce Rd, Boston, MA", "+1-555-0109",
                        "william.rodriguez@university.edu", "/images/william_rodriguez.jpg", 9
                    },
                    {
                        "Elizabeth", "Martinez", new DateTime(1998, 6, 14, 0, 0, 0, DateTimeKind.Utc), "159 Willow Ln, Boston, MA", "+1-555-0110",
                        "elizabeth.martinez@university.edu", "/images/elizabeth_martinez.jpg", 10
                    },
                    {
                        "James", "Hernandez", new DateTime(1994, 2, 28, 0, 0, 0, DateTimeKind.Utc), "753 Oak Dr, Boston, MA", "+1-555-0111",
                        "james.hernandez@university.edu", "/images/james_hernandez.jpg", 1
                    },
                    {
                        "Patricia", "Lopez", new DateTime(1997, 10, 3, 0, 0, 0, DateTimeKind.Utc), "852 Pine St, Boston, MA", "+1-555-0112",
                        "patricia.lopez@university.edu", "/images/patricia_lopez.jpg", 2
                    },
                    {
                        "Charles", "Gonzalez", new DateTime(1996, 7, 19, 0, 0, 0, DateTimeKind.Utc), "951 Maple Ave, Boston, MA", "+1-555-0113",
                        "charles.gonzalez@university.edu", "/images/charles_gonzalez.jpg", 1
                    },
                    {
                        "Linda", "Wilson", new DateTime(1999, 9, 7, 0, 0, 0, DateTimeKind.Utc), "357 Elm Rd, Boston, MA", "+1-555-0114",
                        "linda.wilson@university.edu", "/images/linda_wilson.jpg", 3
                    },
                    {
                        "Thomas", "Anderson", new DateTime(1995, 4, 11, 0, 0, 0, DateTimeKind.Utc), "468 Cedar Dr, Boston, MA", "+1-555-0115",
                        "thomas.anderson@university.edu", "/images/thomas_anderson.jpg", 4
                    },
                    {
                        "Margaret", "Thomas", new DateTime(1980, 5, 15, 0, 0, 0, DateTimeKind.Utc), "1000 Faculty Ave, Boston, MA", "+1-555-0201",
                        "margaret.thomas@university.edu", "/images/margaret_thomas.jpg", 5
                    },
                    {
                        "Christopher", "Taylor", new DateTime(1975, 8, 22, 0, 0, 0, DateTimeKind.Utc), "1100 Staff St, Boston, MA", "+1-555-0202",
                        "christopher.taylor@university.edu", "/images/christopher_taylor.jpg", 6
                    },
                    {
                        "Lisa", "Moore", new DateTime(1982, 3, 30, 0, 0, 0, DateTimeKind.Utc), "1200 Admin Rd, Boston, MA", "+1-555-0203",
                        "lisa.moore@university.edu", "/images/lisa_moore.jpg", 7
                    },
                    {
                        "Daniel", "Jackson", new DateTime(1978, 11, 12, 0, 0, 0, DateTimeKind.Utc), "1300 Service Ln, Boston, MA", "+1-555-0204",
                        "daniel.jackson@university.edu", "/images/daniel_jackson.jpg", 8
                    },
                    {
                        "Susan", "White", new DateTime(1985, 7, 8, 0, 0, 0, DateTimeKind.Utc), "1400 Support Dr, Boston, MA", "+1-555-0205",
                        "susan.white@university.edu", "/images/susan_white.jpg", 9
                    },
                    {
                        "Matthew", "Harris", new DateTime(1979, 9, 25, 0, 0, 0, DateTimeKind.Utc), "1500 Help St, Boston, MA", "+1-555-0206",
                        "matthew.harris@university.edu", "/images/matthew_harris.jpg", 4
                    },
                    {
                        "Karen", "Martin", new DateTime(1983, 4, 17, 0, 0, 0, DateTimeKind.Utc), "1600 Assist Ave, Boston, MA", "+1-555-0207",
                        "karen.martin@university.edu", "/images/karen_martin.jpg", 6
                    },
                    {
                        "Anthony", "Thompson", new DateTime(1976, 12, 5, 0, 0, 0, DateTimeKind.Utc), "1700 Aid Rd, Boston, MA", "+1-555-0208",
                        "anthony.thompson@university.edu", "/images/anthony_thompson.jpg", 2
                    }
                });

            // Professors (depends on people)
            migrationBuilder.InsertData(
                table: "professors",
                columns: new[]
                {
                    "employee_number", "academic_rank", "hire_date", "specialization", "office_location", "salary",
                    "is_active", "person_id"
                },
                values: new object[,]
                {
                    {
                        "Y5Z6A7Z8", 4, new DateTime(2010, 8, 15, 0, 0, 0, DateTimeKind.Utc), "Computer Science", "Building A, Room 101", 95000.00m,
                        true, 16
                    },
                    {
                        "C9D0E5F2", 5, new DateTime(2005, 1, 20, 0, 0, 0, DateTimeKind.Utc), "Mathematics", "Building B, Room 205", 110000.00m,
                        true, 17
                    },
                    { "G3H8I5J6", 3, new DateTime(2015, 9, 1, 0, 0, 0, DateTimeKind.Utc), "Physics", "Building C, Room 310", 85000.00m, true, 18 },
                    {
                        "K7L8M9X0", 4, new DateTime(2012, 3, 10, 0, 0, 0, DateTimeKind.Utc), "Chemistry", "Building D, Room 150", 92000.00m, true,
                        19
                    },
                    {
                        "O1PLQ3R4", 5, new DateTime(2008, 7, 1, 0, 0, 0, DateTimeKind.Utc), "Biology", "Building E, Room 220", 105000.00m, true, 20
                    },
                    {
                        "S5T6UQV8", 2, new DateTime(2018, 1, 15, 0, 0, 0, DateTimeKind.Utc), "Engineering", "Building F, Room 180", 78000.00m,
                        true, 21
                    },
                    {
                        "W9XJY1Z2", 4, new DateTime(2011, 5, 20, 0, 0, 0, DateTimeKind.Utc), "Psychology", "Building G, Room 125", 90000.00m, true,
                        22
                    },
                    { "A3B4C5V6", 3, new DateTime(2016, 9, 10, 0, 0, 0, DateTimeKind.Utc), "History", "Building H, Room 275", 82000.00m, true, 23 }
                });

            // Students (depends on people)
            migrationBuilder.InsertData(
                table: "students",
                columns: new[] { "student_number", "student_status", "notes", "person_id" },
                values: new object[,]
                {
                    { "A1B2C3D4", 1, "Dean's List student", 1 },
                    { "E5F6G7H8", 1, "Active in student government", 2 },
                    { "I9J0K1L2", 1, "Computer Science major", 3 },
                    { "M3N4O5P6", 1, "Mathematics major", 4 },
                    { "Q7R8S9T0", 2, "On academic probation", 5 },
                    { "U1V2W3X4", 1, "Physics major", 6 },
                    { "Y5Z6A7B8", 1, "Engineering student", 7 },
                    { "C9D0E1F2", 1, "Honor student", 8 },
                    { "G3H4I5J6", 1, "Biology major", 9 },
                    { "K7L8M9N0", 1, "Chemistry major", 10 },
                    { "O1P2Q3R4", 3, "Graduated early", 11 },
                    { "S5T6U7V8", 1, "Psychology major", 12 },
                    { "W9X0Y1Z2", 1, "History major", 13 },
                    { "A3B4C5D6", 1, "English Literature major", 14 },
                    { "E7F8G9H0", 1, "Business Administration major", 15 }
                });

            // Users (depends on people)
            migrationBuilder.InsertData(
                table: "users",
                columns: new[]
                    { "username", "password", "role", "is_active", "created_at", "last_login_at", "person_id" },
                values: new object[,]
                {
                    {
                        "admin", "$2a$11$X5z4bB5J9L2d5V8vQ7zZ.e9v7U1wYb9X9z0bB5J9L2d5V8vQ7zZ.e", 1, true,
                        new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc), 
                        new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc), 16
                    },
                    {
                        "prof.taylor", "$2a$11$X5z4bB5J9L2d5V8vQ7zZ.e9v7U1wYb9X9z0bB5J9L2d5V8vQ7zZ.e", 2, true,
                        new DateTime(2024, 2, 1, 0, 0, 0, DateTimeKind.Utc), 
                        new DateTime(2025, 2, 2, 0, 0, 0, DateTimeKind.Utc), 17
                    },
                    {
                        "registrar", "$2a$11$X5z4bB5J9L2d5V8vQ7zZ.e9v7U1wYb9X9z0bB5J9L2d5V8vQ7zZ.e", 3, true,
                        new DateTime(2024, 3, 1, 0, 0, 0, DateTimeKind.Utc), 
                        new DateTime(2025, 3, 3, 0, 0, 0, DateTimeKind.Utc), 18
                    },
                    {
                        "john.smith", "$2a$11$X5z4bB5J9L2d5V8vQ7zZ.e9v7U1wYb9X9z0bB5J9L2d5V8vQ7zZ.e", 4, true,
                        new DateTime(2024, 4, 1, 0, 0, 0, DateTimeKind.Utc), 
                        new DateTime(2025, 4, 4, 0, 0, 0, DateTimeKind.Utc), 1
                    },
                    {
                        "emily.johnson", "$2a$11$X5z4bB5J9L2d5V8vQ7zZ.e9v7U1wYb9X9z0bB5J9L2d5V8vQ7zZ.e", 4, true,
                        new DateTime(2024, 5, 1, 0, 0, 0, DateTimeKind.Utc), 
                        new DateTime(2025, 5, 5, 0, 0, 0, DateTimeKind.Utc), 2
                    },
                    {
                        "michael.williams", "$2a$11$X5z4bB5J9L2d5V8vQ7zZ.e9v7U1wYb9X9z0bB5J9L2d5V8vQ7zZ.e", 4, true,
                        new DateTime(2024, 3, 1, 0, 0, 0, DateTimeKind.Utc), 
                        new DateTime(2025, 5, 6, 0, 0, 0, DateTimeKind.Utc), 3
                    },
                    {
                        "sarah.brown", "$2a$11$X5z4bB5J9L2d5V8vQ7zZ.e9v7U1wYb9X9z0bB5J9L2d5V8vQ7zZ.e", 4, true,
                        new DateTime(2024, 3, 1, 0, 0, 0, DateTimeKind.Utc), 
                        new DateTime(2025, 8, 7, 0, 0, 0, DateTimeKind.Utc), 4
                    },
                    {
                        "david.jones", "$2a$11$X5z4bB5J9L2d5V8vQ7zZ.e9v7U1wYb9X9z0bB5J9L2d5V8vQ7zZ.e", 4, true,
                        new DateTime(2024, 2, 1, 0, 0, 0, DateTimeKind.Utc), 
                        new DateTime(2025, 12, 8, 0, 0, 0, DateTimeKind.Utc), 5
                    },
                    {
                        "jessica.garcia", "$2a$11$X5z4bB5J9L2d5V8vQ7zZ.e9v7U1wYb9X9z0bB5J9L2d5V8vQ7zZ.e", 4, true,
                        new DateTime(2024, 3, 1, 0, 0, 0, DateTimeKind.Utc), 
                        new DateTime(2025, 11, 9, 0, 0, 0, DateTimeKind.Utc), 6
                    },
                    {
                        "robert.miller", "$2a$11$X5z4bB5J9L2d5V8vQ7zZ.e9v7U1wYb9X9z0bB5J9L2d5V8vQ7zZ.e", 4, true,
                        new DateTime(2024, 8, 1, 0, 0, 0, DateTimeKind.Utc), 
                        new DateTime(2025, 10, 10, 0, 0, 0, DateTimeKind.Utc), 7
                    },
                    {
                        "jennifer.davis", "$2a$11$X5z4bB5J9L2d5V8vQ7zZ.e9v7U1wYb9X9z0bB5J9L2d5V8vQ7zZ.e", 4, true,
                        new DateTime(2024, 9, 1, 0, 0, 0, DateTimeKind.Utc), 
                        new DateTime(2025, 12, 11, 0, 0, 0, DateTimeKind.Utc), 8
                    },
                    {
                        "william.rodriguez", "$2a$11$X5z4bB5J9L2d5V8vQ7zZ.e9v7U1wYb9X9z0bB5J9L2d5V8vQ7zZ.e", 4, true,
                        new DateTime(2024, 12, 1, 0, 0, 0, DateTimeKind.Utc), 
                        new DateTime(2025, 12, 12, 0, 0, 0, DateTimeKind.Utc), 9
                    },
                    {
                        "elizabeth.martinez", "$2a$11$X5z4bB5J9L2d5V8vQ7zZ.e9v7U1wYb9X9z0bB5J9L2d5V8vQ7zZ.e", 4, true,
                        new DateTime(2025, 7, 1, 0, 0, 0, DateTimeKind.Utc), 
                        new DateTime(2025, 8, 13, 0, 0, 0, DateTimeKind.Utc), 10
                    }
                });

            // Service offers (no dependencies)
            migrationBuilder.InsertData(
                table: "service_offers",
                columns: new[] { "name", "description", "fees", "is_active" },
                values: new object[,]
                {
                    {
                        "Student Program Enrollment Service", "Service for enrolling students in academic programs",
                        60.00m, true
                    },
                    {
                        "Course Registration Service", "Service for registering students in courses each semester",
                        40.00m, true
                    },
                    { "Grade Management Service", "Service for managing and reporting student grades", 5.00m, true },
                    { "Financial Aid Application Service", "Service for applying for financial aid", 15.00m, true },
                    { "Transcript Issuance Service", "Service for issuing official transcripts", 25.00m, true },
                    {
                        "Student ID Card Issuance/Replacement Service",
                        "Service for issuing or replacing student ID cards", 30.00m, true
                    },
                    { "Degree Audit Service", "Service for conducting degree audits", 40.00m, true },
                    {
                        "Academic Advisory Appointment Service",
                        "Service for scheduling academic advising appointments", 0.00m, true
                    }
                });

            // Programs (no dependencies)
            migrationBuilder.InsertData(
                table: "programs",
                columns: new[] { "code", "name", "description", "minimum_age", "duration", "fees", "is_active" },
                values: new object[,]
                {
                    { "CS", "Computer Science", "Bachelor of Science in Computer Science", 18, 4, 15000.00m, true },
                    { "MATH", "Mathematics", "Bachelor of Science in Mathematics", 18, 4, 12000.00m, true },
                    { "PHYS", "Physics", "Bachelor of Science in Physics", 18, 4, 13000.00m, true },
                    { "CHEM", "Chemistry", "Bachelor of Science in Chemistry", 18, 4, 14000.00m, true },
                    { "BIO", "Biology", "Bachelor of Science in Biology", 18, 4, 13500.00m, true },
                    { "ENG", "Engineering", "Bachelor of Science in Engineering", 18, 4, 16000.00m, true },
                    { "PSY", "Psychology", "Bachelor of Arts in Psychology", 18, 4, 11000.00m, true },
                    { "HIST", "History", "Bachelor of Arts in History", 18, 4, 10000.00m, true },
                    {
                        "ENG-LIT", "English Literature", "Bachelor of Arts in English Literature", 18, 4, 10500.00m,
                        true
                    },
                    { "BUS", "Business Administration", "Bachelor of Business Administration", 18, 4, 14500.00m, true }
                });

            // Courses (no dependencies)
            migrationBuilder.InsertData(
                table: "courses",
                columns: new[] { "code", "title", "description", "credit_hours", "is_active" },
                values: new object[,]
                {
                    {
                        "CS101", "Introduction to Computer Science", "Fundamentals of computer science and programming",
                        4, true
                    },
                    { "CS201", "Data Structures", "Study of fundamental data structures and algorithms", 4, true },
                    { "CS301", "Database Systems", "Design and implementation of database systems", 3, true },
                    { "MATH101", "Calculus I", "Introduction to differential and integral calculus", 4, true },
                    { "MATH201", "Linear Algebra", "Vector spaces, linear transformations, matrices", 3, true },
                    { "PHYS101", "General Physics I", "Mechanics, heat, and waves", 4, true },
                    { "CHEM101", "General Chemistry I", "Fundamental principles of chemistry", 4, true },
                    { "BIO101", "General Biology I", "Introduction to cellular and molecular biology", 4, true },
                    { "ENG101", "English Composition", "Development of writing skills", 3, true },
                    {
                        "HIST101", "Western Civilization I", "Survey of Western civilization from ancient times", 3,
                        true
                    },
                    { "PSY101", "Introduction to Psychology", "Basic concepts and methods of psychology", 3, true },
                    { "ECON101", "Principles of Economics", "Introduction to micro and macro economics", 3, true },
                    { "ART101", "Art Appreciation", "Introduction to visual arts", 3, true },
                    { "MUS101", "Music Appreciation", "Introduction to music history and theory", 3, true },
                    { "PHIL101", "Introduction to Philosophy", "Basic problems and methods of philosophy", 3, true }
                });

            // Prerequisites (depends on courses)
            migrationBuilder.InsertData(
                table: "prerequisites",
                columns: new[] { "minimum_grade", "course_id", "prerequisite_course_id" },
                values: new object[,]
                {
                    { 60.00m, 2, 1 },
                    { 60.00m, 3, 1 },
                    { 60.00m, 3, 2 },
                    { 60.00m, 5, 4 },
                    { 65.00m, 6, 4 },
                    { 65.00m, 7, 4 },
                    { 65.00m, 8, 4 },
                    { 65.00m, 9, 1 },
                    { 65.00m, 10, 1 },
                    { 65.00m, 11, 1 },
                    { 70.00m, 12, 1 },
                    { 70.00m, 13, 1 },
                    { 70.00m, 14, 1 },
                    { 70.00m, 15, 1 }
                });

            // Semesters (no dependencies)
            migrationBuilder.InsertData(
                table: "semesters",
                columns: new[]
                {
                    "term_code", "term", "year", "start_date", "end_date", "registration_start_date",
                    "registration_end_date", "is_active"
                },
                values: new object[,]
                {
                    {
                        "SP24", "Spring", 2024, new DateTime(2024, 1, 16, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 5, 10, 0, 0, 0, DateTimeKind.Utc),
                        new DateTime(2023, 11, 1, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 12, 0, 0, 0, DateTimeKind.Utc), false
                    },
                    {
                        "SU24", "Summer", 2024, new DateTime(2024, 5, 20, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 8, 9, 0, 0, 0, DateTimeKind.Utc),
                        new DateTime(2024, 3, 15, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 5, 17, 0, 0, 0, DateTimeKind.Utc), false
                    },
                    {
                        "FA24", "Fall", 2024, new DateTime(2024, 8, 26, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 12, 13, 0, 0, 0, DateTimeKind.Utc),
                        new DateTime(2024, 4, 1, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 8, 23, 0, 0, 0, DateTimeKind.Utc), false
                    },
                    {
                        "SP25", "Spring", 2025, new DateTime(2025, 1, 13, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 5, 9, 0, 0, 0, DateTimeKind.Utc),
                        new DateTime(2024, 11, 1, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 1, 10, 0, 0, 0, DateTimeKind.Utc), true
                    },
                    {
                        "SU25", "Summer", 2025, new DateTime(2025, 5, 19, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 8, 8, 0, 0, 0, DateTimeKind.Utc),
                        new DateTime(2025, 3, 15, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 5, 16, 0, 0, 0, DateTimeKind.Utc), true
                    },
                    {
                        "FA25", "Fall", 2025, new DateTime(2025, 8, 25, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 12, 12, 0, 0, 0, DateTimeKind.Utc),
                        new DateTime(2025, 4, 1, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 8, 22, 0, 0, 0, DateTimeKind.Utc), true
                    },
                    {
                        "SP26", "Spring", 2026, new DateTime(2026, 1, 12, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 5, 8, 0, 0, 0, DateTimeKind.Utc),
                        new DateTime(2025, 11, 1, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 9, 0, 0, 0, DateTimeKind.Utc), true
                    },
                    {
                        "SU26", "Summer", 2026, new DateTime(2026, 5, 18, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 8, 7, 0, 0, 0, DateTimeKind.Utc),
                        new DateTime(2026, 3, 15, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 5, 15, 0, 0, 0, DateTimeKind.Utc), true
                    }
                });

            // Sections (depends on courses, semesters, professors)
            migrationBuilder.InsertData(
                table: "sections",
                columns: new[]
                {
                    "section_number", "meeting_days", "start_time", "end_time", "classroom", "max_capacity",
                    "current_enrollment", "course_id", "semester_id", "professor_id"
                },
                values: new object[,]
                {
                    {
                        "BA-101", "MWF", new TimeOnly(9, 0, 0), new TimeOnly(9, 50, 0), "Building A, Room 101", 30, 28,
                        1, 4, 1
                    },
                    {
                        "BA-102", "TTh", new TimeOnly(10, 0, 0), new TimeOnly(11, 15, 0), "Building A, Room 102", 25,
                        22, 1, 4, 1
                    },
                    {
                        "BB-201", "MWF", new TimeOnly(10, 0, 0), new TimeOnly(10, 50, 0), "Building B, Room 201", 35,
                        30, 2, 4, 1
                    },
                    {
                        "BC-301", "MWF", new TimeOnly(11, 0, 0), new TimeOnly(11, 50, 0), "Building C, Room 301", 40,
                        35, 3, 4, 2
                    },
                    {
                        "BD-401", "TTh", new TimeOnly(13, 0, 0), new TimeOnly(14, 15, 0), "Building D, Room 401", 30,
                        25, 4, 4, 2
                    },
                    {
                        "BD-502", "MWF", new TimeOnly(14, 0, 0), new TimeOnly(14, 50, 0), "Building D, Room 402", 30,
                        22, 4, 4, 2
                    },
                    {
                        "BE-601", "TTh", new TimeOnly(15, 0, 0), new TimeOnly(16, 15, 0), "Building E, Room 501", 25,
                        20, 5, 4, 3
                    },
                    {
                        "BF-001", "MWF", new TimeOnly(8, 0, 0), new TimeOnly(8, 50, 0), "Building F, Room 601", 30, 25,
                        6, 4, 3
                    },
                    {
                        "BG-701", "TTh", new TimeOnly(9, 30, 0), new TimeOnly(10, 45, 0), "Building G, Room 701", 35,
                        30, 7, 4, 4
                    },
                    {
                        "BH-801", "MWF", new TimeOnly(13, 0, 0), new TimeOnly(13, 50, 0), "Building H, Room 801", 30,
                        25, 8, 4, 5
                    },
                    {
                        "BA-301", "TTh", new TimeOnly(11, 0, 0), new TimeOnly(12, 15, 0), "Building A, Room 103", 40,
                        35, 9, 4, 6
                    },
                    {
                        "BB-202", "MWF", new TimeOnly(12, 0, 0), new TimeOnly(12, 50, 0), "Building B, Room 202", 30,
                        25, 10, 4, 7
                    },
                    {
                        "BC-302", "TTh", new TimeOnly(14, 0, 0), new TimeOnly(15, 15, 0), "Building C, Room 302", 25,
                        20, 11, 4, 8
                    },
                    {
                        "BD-403", "MWF", new TimeOnly(15, 0, 0), new TimeOnly(15, 50, 0), "Building D, Room 403", 30,
                        25, 12, 4, 1
                    },
                    {
                        "BE-502", "TTh", new TimeOnly(16, 0, 0), new TimeOnly(17, 15, 0), "Building E, Room 502", 25,
                        18, 13, 4, 2
                    }
                });

            // Service Applications (depends on people, services, users)
            migrationBuilder.InsertData(
                table: "service_applications",
                columns: new[]
                {
                    "application_date", "status", "paid_fees", "notes", "completed_date", "person_id", "service_id",
                    "processed_by_user_id"
                },
                values: new object[,]
                {
                    {
                        new DateTime(2025, 1, 15, 0, 0, 0, DateTimeKind.Utc), 3, 20.00m, "Initial enrollment application",
                        new DateTime(2025, 1, 20, 0, 0, 0, DateTimeKind.Utc), 1, 1, 1
                    },
                    {
                        new DateTime(2025, 1, 16, 0, 0, 0, DateTimeKind.Utc), 3, 10.00m, "Fall semester registration", new DateTime(2025, 1, 18, 0, 0, 0, DateTimeKind.Utc),
                        2, 2, 1
                    },
                    {
                        new DateTime(2025, 2, 1, 0, 0, 0, DateTimeKind.Utc), 3, 25.00m, "Transcript for job application", new DateTime(2025, 2, 3, 0, 0, 0, DateTimeKind.Utc),
                        3, 5, 3
                    },
                    {
                        new DateTime(2025, 2, 5, 0, 0, 0, DateTimeKind.Utc), 3, 30.00m, "Lost ID card replacement", new DateTime(2025, 2, 7, 0, 0, 0, DateTimeKind.Utc), 4, 6,
                        3
                    },
                    {
                        new DateTime(2025, 2, 10, 0, 0, 0, DateTimeKind.Utc), 3, 40.00m, "Degree audit for graduation", new DateTime(2025, 2, 12, 0, 0, 0, DateTimeKind.Utc),
                        5, 7, 1
                    },
                    {
                        new DateTime(2025, 3, 1, 0, 0, 0, DateTimeKind.Utc), 3, 15.00m, "Financial aid application", new DateTime(2025, 3, 5, 0, 0, 0, DateTimeKind.Utc), 6,
                        4, 3
                    },
                    {
                        new DateTime(2025, 3, 15, 0, 0, 0, DateTimeKind.Utc), 3, 10.00m, "Advising appointment", new DateTime(2025, 3, 16, 0, 0, 0, DateTimeKind.Utc), 7, 8, 2
                    },
                    { new DateTime(2025, 4, 1, 0, 0, 0, DateTimeKind.Utc), 3, 5.00m, "Grade report request", new DateTime(2025, 4, 2, 0, 0, 0, DateTimeKind.Utc), 8, 3, 1 },
                    {
                        new DateTime(2025, 4, 5, 0, 0, 0, DateTimeKind.Utc), 3, 20.00m, "Program change request", new DateTime(2025, 4, 7, 0, 0, 0, DateTimeKind.Utc), 9, 1, 1
                    },
                    {
                        new DateTime(2025, 4, 10, 0, 0, 0, DateTimeKind.Utc), 3, 10.00m, "Summer semester registration", new DateTime(2025, 4, 12, 0, 0, 0, DateTimeKind.Utc),
                        10, 2, 3
                    },
                    { new DateTime(2025, 5, 1, 0, 0, 0, DateTimeKind.Utc), 1, 25.00m, "Transcript request pending", null, 11, 5, null },
                    { new DateTime(2025, 5, 5, 0, 0, 0, DateTimeKind.Utc), 2, 30.00m, "ID replacement in progress", null, 12, 6, null },
                    { new DateTime(2025, 5, 10, 0, 0, 0, DateTimeKind.Utc), 1, 15.00m, "Financial aid application submitted", null, 13, 4, null },
                    { new DateTime(2025, 5, 15, 0, 0, 0, DateTimeKind.Utc), 2, 10.00m, "Advising appointment scheduled", null, 14, 8, null },
                    { new DateTime(2025, 6, 1, 0, 0, 0, DateTimeKind.Utc), 1, 5.00m, "Grade report requested", null, 15, 3, null }
                });

            // Document Verifications (depends on people, users)
            migrationBuilder.InsertData(
                table: "docs_verifications",
                columns: new[]
                {
                    "submission_date", "verification_date", "status", "is_approved", "rejected_reason", "paid_fees",
                    "notes", "person_id", "verified_by_user_id"
                },
                values: new object[,]
                {
                    {
                        new DateTime(2025, 1, 5, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 1, 10, 0, 0, 0, DateTimeKind.Utc), 3, true, null, 50.00m,
                        "All documents verified", 1, 1
                    },
                    {
                        new DateTime(2025, 1, 6, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 1, 11, 0, 0, 0, DateTimeKind.Utc), 3, true, null, 50.00m,
                        "Documents complete", 2, 1
                    },
                    {
                        new DateTime(2025, 1, 7, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 1, 12, 0, 0, 0, DateTimeKind.Utc), 3, true, null, 50.00m,
                        "Verified successfully", 3, 3
                    },
                    {
                        new DateTime(2025, 1, 8, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 1, 13, 0, 0, 0, DateTimeKind.Utc), 3, false, "Missing transcript", 50.00m,
                        "Need to resubmit transcript", 4, 3
                    },
                    {
                        new DateTime(2025, 1, 9, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 1, 14, 0, 0, 0, DateTimeKind.Utc), 3, true, null, 50.00m,
                        "All documents in order", 5, 1
                    },
                    {
                        new DateTime(2025, 2, 1, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 2, 5, 0, 0, 0, DateTimeKind.Utc), 3, true, null, 50.00m, "Documents verified",
                        6, 3
                    },
                    {
                        new DateTime(2025, 2, 2, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 2, 6, 0, 0, 0, DateTimeKind.Utc), 3, false, "ID photo unclear", 50.00m,
                        "Need better ID photo", 7, 1
                    },
                    { new DateTime(2025, 2, 3, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 2, 7, 0, 0, 0, DateTimeKind.Utc), 3, true, null, 50.00m, "Approved", 8, 3 },
                    {
                        new DateTime(2025, 2, 4, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 2, 8, 0, 0, 0, DateTimeKind.Utc), 3, true, null, 50.00m, "Documents complete",
                        9, 1
                    },
                    { new DateTime(2025, 2, 5, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 2, 9, 0, 0, 0, DateTimeKind.Utc), 3, true, null, 50.00m, "Verified", 10, 3 },
                    { new DateTime(2025, 3, 1, 0, 0, 0, DateTimeKind.Utc), null, 2, null, null, 50.00m, "Under review", 11, null },
                    { new DateTime(2025, 3, 2, 0, 0, 0, DateTimeKind.Utc), null, 1, null, null, 50.00m, "Submitted, pending review", 12, null },
                    { new DateTime(2025, 3, 3, 0, 0, 0, DateTimeKind.Utc), null, 2, null, null, 50.00m, "Verification in progress", 13, null },
                    { new DateTime(2025, 3, 4, 0, 0, 0, DateTimeKind.Utc), null, 1, null, null, 50.00m, "Awaiting review", 14, null },
                    { new DateTime(2025, 3, 5, 0, 0, 0, DateTimeKind.Utc), null, 1, null, null, 50.00m, "New submission", 15, null }
                });

            // Financial Holds (depends on students, users)
            migrationBuilder.InsertData(
                table: "financial_holds",
                columns: new[]
                {
                    "reason", "hold_amount", "date_placed", "date_resolved", "is_active", "resolution_notes",
                    "student_id", "placed_by_user_id", "resolved_by_user_id"
                },
                values: new object[,]
                {
                    {
                        "Unpaid tuition balance", 1500.00m, new DateTime(2025, 1, 10, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 1, 20, 0, 0, 0, DateTimeKind.Utc), false,
                        "Payment received", 1, 1, 1
                    },
                    {
                        "Library fine overdue", 50.00m, new DateTime(2025, 1, 15, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 1, 25, 0, 0, 0, DateTimeKind.Utc), false,
                        "Fine paid", 2, 1, 3
                    },
                    {
                        "Dorm damage charges", 300.00m, new DateTime(2025, 2, 1, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 2, 15, 0, 0, 0, DateTimeKind.Utc), false,
                        "Payment plan established", 3, 3, 1
                    },
                    {
                        "Unpaid parking tickets", 125.00m, new DateTime(2025, 2, 5, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 2, 20, 0, 0, 0, DateTimeKind.Utc), false,
                        "Paid in full", 4, 3, 3
                    },
                    {
                        "Outstanding tuition for Spring 2025", 2000.00m, new DateTime(2025, 2, 10, 0, 0, 0, DateTimeKind.Utc), null, true, null, 5,
                        1, null
                    },
                    { "Unreturned equipment", 500.00m, new DateTime(2025, 3, 1, 0, 0, 0, DateTimeKind.Utc), null, true, null, 6, 3, null },
                    {
                        "Unpaid health center fees", 75.00m, new DateTime(2025, 3, 5, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 3, 10, 0, 0, 0, DateTimeKind.Utc), false,
                        "Paid", 7, 1, 3
                    },
                    {
                        "Outstanding balance from previous semester", 1200.00m, new DateTime(2025, 3, 10, 0, 0, 0, DateTimeKind.Utc),
                        new DateTime(2025, 3, 25, 0, 0, 0, DateTimeKind.Utc), false, "Payment received", 8, 3, 1
                    },
                    { "Unpaid graduation fees", 100.00m, new DateTime(2025, 4, 1, 0, 0, 0, DateTimeKind.Utc), null, true, null, 9, 1, null },
                    {
                        "Unpaid lab fees", 250.00m, new DateTime(2025, 4, 5, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 4, 15, 0, 0, 0, DateTimeKind.Utc), false, "Paid",
                        10, 3, 3
                    },
                    {
                        "Unpaid study abroad deposit", 500.00m, new DateTime(2025, 4, 10, 0, 0, 0, DateTimeKind.Utc), null, true, null, 11, 1, null
                    },
                    {
                        "Outstanding meal plan balance", 350.00m, new DateTime(2025, 5, 1, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 5, 10, 0, 0, 0, DateTimeKind.Utc),
                        false, "Balance paid", 12, 3, 1
                    },
                    { "Unpaid technology fee", 200.00m, new DateTime(2025, 5, 5, 0, 0, 0, DateTimeKind.Utc), null, true, null, 13, 1, null },
                    {
                        "Unpaid activity fee", 150.00m, new DateTime(2025, 5, 10, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 5, 20, 0, 0, 0, DateTimeKind.Utc), false,
                        "Paid", 14, 3, 3
                    },
                    { "Unpaid transcript fee", 25.00m, new DateTime(2025, 6, 1, 0, 0, 0, DateTimeKind.Utc), null, true, null, 15, 1, null }
                });

            // Enrollments (depends on students, programs, service applications)
            migrationBuilder.InsertData(
                table: "enrollments",
                columns: new[]
                {
                    "enrollment_date", "actual_grad_date", "notes", "student_id", "program_id",
                    "service_application_id", "enrollment_status"
                },
                values: new object[,]
                {
                    { new DateTime(2025, 1, 20, 0, 0, 0, DateTimeKind.Utc), null, "Computer Science major", 1, 1, 1, 1 },
                    { new DateTime(2025, 1, 20, 0, 0, 0, DateTimeKind.Utc), null, "Mathematics major", 2, 2, 2, 1 },
                    { new DateTime(2025, 1, 20, 0, 0, 0, DateTimeKind.Utc), null, "Physics major", 3, 3, 3, 1 },
                    { new DateTime(2025, 1, 20, 0, 0, 0, DateTimeKind.Utc), null, "Chemistry major", 4, 4, 4, 1 },
                    { new DateTime(2025, 1, 20, 0, 0, 0, DateTimeKind.Utc), null, "Biology major", 5, 5, 5, 3 },
                    { new DateTime(2025, 1, 20, 0, 0, 0, DateTimeKind.Utc), null, "Engineering major", 6, 6, 6, 3 },
                    { new DateTime(2025, 1, 20, 0, 0, 0, DateTimeKind.Utc), null, "Psychology major", 7, 7, 7, 3 },
                    { new DateTime(2025, 1, 20, 0, 0, 0, DateTimeKind.Utc), null, "History major", 8, 8, 8, 3 },
                    { new DateTime(2025, 1, 20, 0, 0, 0, DateTimeKind.Utc), null, "English Literature major", 9, 9, 9, 3 },
                    { new DateTime(2025, 1, 20, 0, 0, 0, DateTimeKind.Utc), null, "Business Administration major", 10, 10, 10, 3 },
                    { new DateTime(2024, 1, 20, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 5, 15, 0, 0, 0, DateTimeKind.Utc), "Graduated early", 11, 1, 11, 2 },
                    { new DateTime(2025, 1, 20, 0, 0, 0, DateTimeKind.Utc), null, "Computer Science minor", 12, 1, 12, 2 },
                    { new DateTime(2025, 1, 20, 0, 0, 0, DateTimeKind.Utc), null, "Double major in Math and Physics", 13, 2, 13, 2 },
                    { new DateTime(2025, 1, 20, 0, 0, 0, DateTimeKind.Utc), null, "Pre-med track", 14, 5, 14, 2 },
                    { new DateTime(2025, 1, 20, 0, 0, 0, DateTimeKind.Utc), null, "Honors program", 15, 10, 15, 2 }
                });

            // Registrations (depends on students, sections, semesters, users)
            migrationBuilder.InsertData(
                table: "registrations",
                columns: new[]
                {
                    "registration_date", "registration_fees", "student_id", "section_id", "semester_id",
                    "processed_by_user_id"
                },
                values: new object[,]
                {
                    { new DateTime(2025, 4, 1, 0, 0, 0, DateTimeKind.Utc), 40.00m, 1, 1, 4, 1 },
                    { new DateTime(2025, 4, 1, 0, 0, 0, DateTimeKind.Utc), 40.00m, 1, 3, 4, 1 },
                    { new DateTime(2025, 4, 2, 0, 0, 0, DateTimeKind.Utc), 40.00m, 2, 2, 4, 1 },
                    { new DateTime(2025, 4, 2, 0, 0, 0, DateTimeKind.Utc), 40.00m, 2, 4, 4, 1 },
                    { new DateTime(2025, 4, 3, 0, 0, 0, DateTimeKind.Utc), 40.00m, 3, 1, 4, 3 },
                    { new DateTime(2025, 4, 3, 0, 0, 0, DateTimeKind.Utc), 40.00m, 3, 5, 4, 3 },
                    { new DateTime(2025, 4, 4, 0, 0, 0, DateTimeKind.Utc), 40.00m, 4, 6, 4, 3 },
                    { new DateTime(2025, 4, 4, 0, 0, 0, DateTimeKind.Utc), 40.00m, 4, 8, 4, 3 },
                    { new DateTime(2025, 4, 5, 0, 0, 0, DateTimeKind.Utc), 40.00m, 5, 7, 4, 1 },
                    { new DateTime(2025, 4, 5, 0, 0, 0, DateTimeKind.Utc), 40.00m, 5, 9, 4, 1 },
                    { new DateTime(2025, 4, 6, 0, 0, 0, DateTimeKind.Utc), 40.00m, 6, 10, 4, 3 },
                    { new DateTime(2025, 4, 6, 0, 0, 0, DateTimeKind.Utc), 40.00m, 6, 12, 4, 3 },
                    { new DateTime(2025, 4, 7, 0, 0, 0, DateTimeKind.Utc), 40.00m, 7, 11, 4, 1 },
                    { new DateTime(2025, 4, 7, 0, 0, 0, DateTimeKind.Utc), 40.00m, 7, 13, 4, 1 },
                    { new DateTime(2025, 4, 8, 0, 0, 0, DateTimeKind.Utc), 40.00m, 8, 14, 4, 3 },
                    { new DateTime(2025, 4, 8, 0, 0, 0, DateTimeKind.Utc), 40.00m, 8, 15, 4, 3 },
                    { new DateTime(2025, 4, 9, 0, 0, 0, DateTimeKind.Utc), 40.00m, 9, 1, 4, 1 },
                    { new DateTime(2025, 4, 9, 0, 0, 0, DateTimeKind.Utc), 40.00m, 9, 4, 4, 1 },
                    { new DateTime(2025, 4, 10, 0, 0, 0, DateTimeKind.Utc), 40.00m, 10, 2, 4, 3 },
                    { new DateTime(2025, 4, 10, 0, 0, 0, DateTimeKind.Utc), 40.00m, 10, 5, 4, 3 }
                });

            // Grades (depends on students, courses, semesters, registrations)
            migrationBuilder.InsertData(
                table: "grades",
                columns: new[]
                {
                    "score", "date_recorded", "comments", "student_id", "course_id", "semester_id", "registration_id"
                },
                values: new object[,]
                {
                    { 92.5m, new DateTime(2025, 5, 20, 0, 0, 0, DateTimeKind.Utc), "Excellent work", 1, 1, 4, 1 },
                    { 88.0m, new DateTime(2025, 5, 20, 0, 0, 0, DateTimeKind.Utc), "Good performance", 1, 2, 4, 2 },
                    { 85.5m, new DateTime(2025, 5, 20, 0, 0, 0, DateTimeKind.Utc), "Solid understanding", 2, 1, 4, 3 },
                    { 90.0m, new DateTime(2025, 5, 20, 0, 0, 0, DateTimeKind.Utc), "Excellent participation", 2, 3, 4, 4 },
                    { 78.5m, new DateTime(2025, 5, 20, 0, 0, 0, DateTimeKind.Utc), "Needs improvement", 3, 1, 4, 5 },
                    { 82.0m, new DateTime(2025, 5, 20, 0, 0, 0, DateTimeKind.Utc), "Average performance", 3, 4, 4, 6 },
                    { 95.0m, new DateTime(2025, 5, 20, 0, 0, 0, DateTimeKind.Utc), "Outstanding work", 4, 4, 4, 7 },
                    { 91.5m, new DateTime(2025, 5, 20, 0, 0, 0, DateTimeKind.Utc), "Very good", 4, 5, 4, 8 },
                    { 65.0m, new DateTime(2025, 5, 20, 0, 0, 0, DateTimeKind.Utc), "Needs significant improvement", 5, 5, 4, 9 },
                    { 72.5m, new DateTime(2025, 5, 20, 0, 0, 0, DateTimeKind.Utc), "Barely passing", 5, 6, 4, 10 },
                    { 89.0m, new DateTime(2025, 5, 20, 0, 0, 0, DateTimeKind.Utc), "Strong performance", 6, 6, 4, 11 },
                    { 93.5m, new DateTime(2025, 5, 20, 0, 0, 0, DateTimeKind.Utc), "Excellent work", 6, 7, 4, 12 },
                    { 87.5m, new DateTime(2025, 5, 20, 0, 0, 0, DateTimeKind.Utc), "Good effort", 7, 7, 4, 13 },
                    { 84.0m, new DateTime(2025, 5, 20, 0, 0, 0, DateTimeKind.Utc), "Satisfactory", 7, 8, 4, 14 },
                    { 96.0m, new DateTime(2025, 5, 20, 0, 0, 0, DateTimeKind.Utc), "Exceptional work", 8, 8, 4, 15 },
                    { 94.5m, new DateTime(2025, 5, 20, 0, 0, 0, DateTimeKind.Utc), "Outstanding", 8, 9, 4, 16 },
                    { 80.0m, new DateTime(2025, 5, 20, 0, 0, 0, DateTimeKind.Utc), "Meets expectations", 9, 1, 4, 17 },
                    { 77.5m, new DateTime(2025, 5, 20, 0, 0, 0, DateTimeKind.Utc), "Needs more effort", 9, 3, 4, 18 },
                    { 83.0m, new DateTime(2025, 5, 20, 0, 0, 0, DateTimeKind.Utc), "Good work", 10, 2, 4, 19 },
                    { 86.5m, new DateTime(2025, 5, 20, 0, 0, 0, DateTimeKind.Utc), "Very good performance", 10, 4, 4, 20 }
                });

            // Interviews (depends on professors)
            migrationBuilder.InsertData(
                table: "interviews",
                columns: new[]
                {
                    "scheduled_date_time", "actual_start_time", "actual_end_time", "is_approved", "paid_fees", "notes",
                    "recommendation", "professor_id"
                },
                values: new object[,]
                {
                    {
                        new DateTime(2025, 1, 10, 10, 0, 0, DateTimeKind.Utc), new DateTime(2025, 1, 10, 10, 5, 0, DateTimeKind.Utc), 
                        new DateTime(2025, 1, 10, 10, 45, 0, DateTimeKind.Utc), true, 50.00m, "Interview for admission",
                        "Strongly recommend admission", 1
                    },
                    {
                        new DateTime(2025, 1, 12, 14, 0, 0, DateTimeKind.Utc), new DateTime(2025, 1, 12, 14, 10, 0, DateTimeKind.Utc),
                        new DateTime(2025, 1, 12, 14, 50, 0, DateTimeKind.Utc), true, 50.00m, "Scholarship interview",
                        "Recommend for scholarship", 2
                    },
                    {
                        new DateTime(2025, 1, 15, 11, 0, 0, DateTimeKind.Utc), new DateTime(2025, 1, 15, 11, 0, 0, DateTimeKind.Utc),
                        new DateTime(2025, 1, 15, 11, 40, 0, DateTimeKind.Utc), false, 50.00m, "Admission interview", "Needs improvement",
                        3
                    },
                    {
                        new DateTime(2025, 1, 18, 13, 0, 0, DateTimeKind.Utc), new DateTime(2025, 1, 18, 13, 5, 0, DateTimeKind.Utc),
                        new DateTime(2025, 1, 18, 13, 45, 0, DateTimeKind.Utc), true, 50.00m, "Honors program interview",
                        "Excellent candidate", 4
                    },
                    {
                        new DateTime(2025, 1, 20, 9, 0, 0, DateTimeKind.Utc), new DateTime(2025, 1, 20, 9, 10, 0, DateTimeKind.Utc),
                        new DateTime(2025, 1, 20, 9, 50, 0, DateTimeKind.Utc), true, 50.00m, "Transfer student interview",
                        "Recommend admission", 5
                    },
                    { new DateTime(2025, 1, 25, 15, 0, 0, DateTimeKind.Utc), null, null, false, 50.00m, "Scheduled interview", null, 6 },
                    {
                        new DateTime(2025, 2, 1, 10, 0, 0, DateTimeKind.Utc), new DateTime(2025, 2, 1, 10, 0, 0, DateTimeKind.Utc),
                        new DateTime(2025, 2, 1, 10, 40, 0, DateTimeKind.Utc), true, 50.00m, "Graduate program interview",
                        "Strong candidate", 7
                    },
                    {
                        new DateTime(2025, 2, 5, 14, 0, 0, DateTimeKind.Utc), new DateTime(2025, 2, 5, 14, 5, 0, DateTimeKind.Utc),
                        new DateTime(2025, 2, 5, 14, 45, 0, DateTimeKind.Utc), true, 50.00m, "International student interview",
                        "Recommend with conditions", 8
                    },
                    { new DateTime(2025, 2, 10, 11, 0, 0, DateTimeKind.Utc), null, null, false, 50.00m, "Upcoming interview", null, 1 },
                    {
                        new DateTime(2025, 2, 15, 13, 0, 0, DateTimeKind.Utc), new DateTime(2025, 2, 15, 13, 0, 0, DateTimeKind.Utc),
                        new DateTime(2025, 2, 15, 13, 40, 0, DateTimeKind.Utc), false, 50.00m, "Interview for research position",
                        "Not qualified", 2
                    }
                });

            // Entrance Exams (no dependencies)
            migrationBuilder.InsertData(
                table: "entrance_exams",
                columns: new[]
                {
                    "exam_date", "score", "max_score", "passing_score", "is_passed", "paid_fees", "exam_status", "notes"
                },
                values: new object[,]
                {
                    { new DateTime(2025, 1, 5, 0, 0, 0, DateTimeKind.Utc), 85.50m, 100, 60, true, 50.00m, 3, "Passed with good score" },
                    { new DateTime(2025, 1, 5, 0, 0, 0, DateTimeKind.Utc), 92.00m, 100, 60, true, 50.00m, 3, "Excellent performance" },
                    { new DateTime(2025, 1, 5, 0, 0, 0, DateTimeKind.Utc), 68.00m, 100, 60, false, 50.00m, 3, "Did not pass, needs retake" },
                    { new DateTime(2025, 1, 12, 0, 0, 0, DateTimeKind.Utc), 75.50m, 100, 65, true, 50.00m, 3, "Passed" },
                    { new DateTime(2025, 1, 12, 0, 0, 0, DateTimeKind.Utc), 80.00m, 100, 65, true, 50.00m, 3, "Good performance" },
                    { new DateTime(2025, 1, 12, 0, 0, 0, DateTimeKind.Utc), 72.00m, 100, 65, true, 50.00m, 3, "Barely passed" },
                    { new DateTime(2025, 1, 19, 0, 0, 0, DateTimeKind.Utc), null, 100, 60, null, 50.00m, 5, "Cancelled by student" },
                    { new DateTime(2025, 1, 19, 0, 0, 0, DateTimeKind.Utc), null, 100, 60, null, 50.00m, 4, "No show" },
                    { new DateTime(2025, 1, 26, 0, 0, 0, DateTimeKind.Utc), 65.00m, 100, 70, false, 50.00m, 3, "Did not pass" },
                    { new DateTime(2025, 1, 26, 0, 0, 0, DateTimeKind.Utc), 88.00m, 100, 70, true, 50.00m, 3, "Very good score" },
                    { new DateTime(2025, 2, 2, 0, 0, 0, DateTimeKind.Utc), null, 100, 70, null, 50.00m, 1, "Scheduled exam" },
                    { new DateTime(2025, 2, 2, 0, 0, 0, DateTimeKind.Utc), null, 100, 70, null, 50.00m, 1, "Scheduled exam" },
                    { new DateTime(2025, 2, 9, 0, 0, 0, DateTimeKind.Utc), null, 100, 60, null, 50.00m, 2, "Exam in progress" },
                    { new DateTime(2025, 2, 9, 0, 0, 0, DateTimeKind.Utc), null, 100, 60, null, 50.00m, 2, "Exam in progress" },
                    { new DateTime(2025, 2, 16, 0, 0, 0, DateTimeKind.Utc), null, 100, 65, null, 50.00m, 1, "Upcoming exam" }
                });
            
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_sections_course_semester_number",
                table: "sections");

            migrationBuilder.DropIndex(
                name: "ix_registrations_student_section_semester",
                table: "registrations");

            migrationBuilder.DropIndex(
                name: "ix_grades_student_course_semester",
                table: "grades");

            migrationBuilder.DropIndex(
                name: "ix_financial_holds_active",
                table: "financial_holds");

            migrationBuilder.DropIndex(
                name: "ix_enrollments_student_program",
                table: "enrollments");

            migrationBuilder.CreateIndex(
                name: "IX_sections_course_id",
                table: "sections",
                column: "course_id");

            migrationBuilder.CreateIndex(
                name: "ix_sections_section_number",
                table: "sections",
                column: "section_number",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_registrations_student_id",
                table: "registrations",
                column: "student_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_grades_student",
                table: "grades",
                column: "student_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_enrollments_student_id",
                table: "enrollments",
                column: "student_id",
                unique: true);
            
            // Delete all data in dependency order
migrationBuilder.DeleteData(
    table: "entrance_exams",
    keyColumn: "Id",
    keyValues: null); 

migrationBuilder.DeleteData(
    table: "interviews",
    keyColumn: "Id",
    keyValues: null);

migrationBuilder.DeleteData(
    table: "grades",
    keyColumn: "Id",
    keyValues: null);

migrationBuilder.DeleteData(
    table: "registrations",
    keyColumn: "Id",
    keyValues: null);

migrationBuilder.DeleteData(
    table: "enrollments",
    keyColumn: "Id",
    keyValues: null);

migrationBuilder.DeleteData(
    table: "financial_holds",
    keyColumn: "Id",
    keyValues: null);

migrationBuilder.DeleteData(
    table: "docs_verifications",
    keyColumn: "Id",
    keyValues: null);

migrationBuilder.DeleteData(
    table: "service_applications",
    keyColumn: "Id",
    keyValues: null);

migrationBuilder.DeleteData(
    table: "sections",
    keyColumn: "Id",
    keyValues: null);

migrationBuilder.DeleteData(
    table: "prerequisites",
    keyColumn: "Id",
    keyValues: null);

migrationBuilder.DeleteData(
    table: "users",
    keyColumn: "Id",
    keyValues: null);

migrationBuilder.DeleteData(
    table: "professors",
    keyColumn: "Id",
    keyValues: null);

migrationBuilder.DeleteData(
    table: "students",
    keyColumn: "Id",
    keyValues: null);

migrationBuilder.DeleteData(
    table: "semesters",
    keyColumn: "Id",
    keyValues: null);

migrationBuilder.DeleteData(
    table: "courses",
    keyColumn: "Id",
    keyValues: null);

migrationBuilder.DeleteData(
    table: "programs",
    keyColumn: "Id",
    keyValues: null);

migrationBuilder.DeleteData(
    table: "service_offers",
    keyColumn: "Id",
    keyValues: null);

migrationBuilder.DeleteData(
    table: "people",
    keyColumn: "Id",
    keyValues: null);

migrationBuilder.DeleteData(
    table: "countries",
    keyColumn: "Id",
    keyValues: null);

            migrationBuilder.Sql("DBCC CHECKIDENT ('countries', RESEED, 0)");
            migrationBuilder.Sql("DBCC CHECKIDENT ('people', RESEED, 0)");
            migrationBuilder.Sql("DBCC CHECKIDENT ('professors', RESEED, 0)");
            migrationBuilder.Sql("DBCC CHECKIDENT ('students', RESEED, 0)");
            migrationBuilder.Sql("DBCC CHECKIDENT ('users', RESEED, 0)");
            migrationBuilder.Sql("DBCC CHECKIDENT ('service_offers', RESEED, 0)");
            migrationBuilder.Sql("DBCC CHECKIDENT ('programs', RESEED, 0)");
            migrationBuilder.Sql("DBCC CHECKIDENT ('courses', RESEED, 0)");
            migrationBuilder.Sql("DBCC CHECKIDENT ('semesters', RESEED, 0)");
            migrationBuilder.Sql("DBCC CHECKIDENT ('sections', RESEED, 0)");
            migrationBuilder.Sql("DBCC CHECKIDENT ('service_applications', RESEED, 0)");
            migrationBuilder.Sql("DBCC CHECKIDENT ('docs_verifications', RESEED, 0)");
            migrationBuilder.Sql("DBCC CHECKIDENT ('financial_holds', RESEED, 0)");
            migrationBuilder.Sql("DBCC CHECKIDENT ('enrollments', RESEED, 0)");
            migrationBuilder.Sql("DBCC CHECKIDENT ('registrations', RESEED, 0)");
            migrationBuilder.Sql("DBCC CHECKIDENT ('grades', RESEED, 0)");
            migrationBuilder.Sql("DBCC CHECKIDENT ('interviews', RESEED, 0)");
            migrationBuilder.Sql("DBCC CHECKIDENT ('entrance_exams', RESEED, 0)");
        }
    }
}
