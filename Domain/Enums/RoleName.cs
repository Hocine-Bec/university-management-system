namespace Domain.Enums;

public enum SystemRole
{
    Admin = 1,                  // Full system access, user management, system configuration
    DepartmentHead = 2,         // Department courses, faculty management, student records (department) 
    Faculty = 3,                // Own courses, student grades, course materials
    TeachingAssistant = 4,      // Limited course access, grading assistance
    Advisor = 5,                // Student records, academic planning, course recommendations
    AdmissionsOfficer = 6,      // Application processing, student enrollment
    Student = 7,                // Own records, course enrollment, grade viewing
    StudentLeader = 8,          // Student organization management, event planning
    ItSupport = 9,              // Technical support, account management
    Librarian = 10              // Library resources, research assistance
}