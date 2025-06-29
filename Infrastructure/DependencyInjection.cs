using Applications.Interfaces.Repositories;
using Applications.Interfaces.UnitOfWorks;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Infrastructure.UnitOfWorks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services,
            IConfiguration configuration)
        {
            var connectionString = configuration["DefaultConnection"];

            services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(connectionString));

            services.AddScoped<ICountryRepository, CountryRepository>();
            services.AddScoped<IPersonRepository, PersonRepository>();
            services.AddScoped<IStudentRepository, StudentRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IProfessorRepository, ProfessorRepository>();
            services.AddScoped<IServiceOfferRepository, ServiceOfferRepository>();
            services.AddScoped<IProgramRepository, ProgramRepository>();
            services.AddScoped<IServiceApplicationRepository, ServiceApplicationRepository>();
            services.AddScoped<IDocsVerificationRepository, DocsVerificationRepository>();
            services.AddScoped<IEntranceExamRepository, EntranceExamRepository>();
            services.AddScoped<IInterviewRepository, InterviewRepository>();
            services.AddScoped<IEnrollmentRepository, EnrollmentRepository>();
            services.AddScoped<ISemesterRepository, SemesterRepository>();
            services.AddScoped<ICourseRepository, CourseRepository>();
            
            //Unit of work pattern
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            
            
            return services;
        }
    }
}
