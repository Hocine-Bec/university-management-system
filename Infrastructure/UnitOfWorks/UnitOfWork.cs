using Applications.Interfaces.Repositories;
using Applications.Interfaces.UnitOfWorks;
using Infrastructure.Data;
using Infrastructure.Repositories;

namespace Infrastructure.UnitOfWorks;

public class UnitOfWork(AppDbContext context) : IUnitOfWork
{
    // Lazy initialization of repositories
    private ICountryRepository? _countries;
    private IDocsVerificationRepository? _docsVerifications;
    private IEnrollmentRepository? _enrollments;
    private IEntranceExamRepository? _entranceExams;
    private IInterviewRepository? _interviews;
    private IPersonRepository? _people;
    private IProfessorRepository? _professors;
    private IProgramRepository? _programs;
    private IServiceApplicationRepository? _serviceApplications;
    private IServiceOfferRepository? _serviceOffers;
    private IStudentRepository? _students;
    private IUserRepository? _users;
    private ISemesterRepository? _semesters;
    private ICourseRepository? _courses;
    private IPrerequisiteRepository? _prerequisites;
    private ISectionRepository? _sections;
    private IRegistrationRepository? _registrations;
    private IGradeRepository? _grades;
    private IFinancialHoldRepository? _financialHolds;
    private IRoleRepository? _roles;
    private IUserRoleRepository? _userRoles;

    // Properties with lazy initialization - all repositories share the same context
    public ICountryRepository Countries => _countries ??= new CountryRepository(context);
    public IDocsVerificationRepository DocsVerifications => _docsVerifications ??= new DocsVerificationRepository(context);
    public IEnrollmentRepository Enrollments => _enrollments ??= new EnrollmentRepository(context);
    public IEntranceExamRepository EntranceExams => _entranceExams ??= new EntranceExamRepository(context);
    public IInterviewRepository Interviews => _interviews ??= new InterviewRepository(context);
    public IPersonRepository People => _people ??= new PersonRepository(context);
    public IProfessorRepository Professors => _professors ??= new ProfessorRepository(context);
    public IProgramRepository Programs => _programs ??= new ProgramRepository(context);
    public IServiceApplicationRepository ServiceApplications => _serviceApplications ??= new ServiceApplicationRepository(context);
    public IServiceOfferRepository ServiceOffers => _serviceOffers ??= new ServiceOfferRepository(context);
    public IStudentRepository Students => _students ??= new StudentRepository(context);
    public IUserRepository Users => _users ??= new UserRepository(context);
    public ISemesterRepository Semesters => _semesters ??= new SemesterRepository(context);
    public ICourseRepository Courses => _courses ??= new CourseRepository(context);
    public IPrerequisiteRepository Prerequisites => _prerequisites ??= new PrerequisiteRepository(context);
    public ISectionRepository Sections => _sections ??= new SectionRepository(context);
    public IRegistrationRepository Registrations => _registrations ??= new RegistrationRepository(context);
    public IGradeRepository Grades => _grades ??= new GradeRepository(context);
    public IFinancialHoldRepository FinancialHolds => _financialHolds ??= new FinancialHoldRepository(context);
    public IRoleRepository Roles => _roles ??= new RoleRepository(context);
    public IUserRoleRepository UserRoles => _userRoles ??= new UserRoleRepository(context);

    public async Task<int> CompleteAsync() => await context.SaveChangesAsync();

    public void Dispose()
    {
        context?.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        await context.DisposeAsync();
    }
}