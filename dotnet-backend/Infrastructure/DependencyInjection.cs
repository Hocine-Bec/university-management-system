using System.Text;
using Applications.Interfaces.Auth;
using Applications.Interfaces.Logging;
using Applications.Interfaces.Repositories;
using Applications.Interfaces.UnitOfWorks;
using Infrastructure.Authentication;
using Infrastructure.Data;
using Infrastructure.Logging;
using Infrastructure.Repositories;
using Infrastructure.Settings;
using Infrastructure.UnitOfWorks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.Configure<JwtSettings>(options =>
            {
                options.Secret = Environment.GetEnvironmentVariable("JWT_SECRET_KEY") ?? throw new InvalidOperationException("JWT_SECRET_KEY environment variable is not set");
                options.Issuer = Environment.GetEnvironmentVariable("JWT_ISSUER") ?? throw new InvalidOperationException("JWT_ISSUER environment variable is not set");
                options.Audience = Environment.GetEnvironmentVariable("JWT_AUDIENCE") ?? throw new InvalidOperationException("JWT_AUDIENCE environment variable is not set");
                options.LifeTime = int.Parse(Environment.GetEnvironmentVariable("JWT_ACCESS_TOKEN_LIFETIME_MINUTES") ?? "15");
                options.RefreshTokenLifeTime = int.Parse(Environment.GetEnvironmentVariable("JWT_REFRESH_TOKEN_LIFETIME_DAYS") ?? "7");
            });

            var connectionString = Environment.GetEnvironmentVariable("DefaultConnection");
            services.AddDbContext<AppDbContext>(options => options.UseNpgsql(connectionString));
            
            
            services.AddAuthentication(options =>
                {
                   options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                   options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                }).AddJwtBearer(options =>
                {
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidAudience = Environment.GetEnvironmentVariable("JWT_AUDIENCE"),
                        ValidIssuer = Environment.GetEnvironmentVariable("JWT_ISSUER"),
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("JWT_SECRET")!)),
                        ClockSkew = TimeSpan.Zero
                    };
                });
            
            services.AddScoped<IJwtTokenService, JwtTokenService>();
            services.AddScoped<IPasswordHasher, PasswordHasher>();
            services.AddScoped<IMyLogger, MyLogger>();
            
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
            services.AddScoped<IPrerequisiteRepository, PrerequisiteRepository>();
            services.AddScoped<ISectionRepository, SectionRepository>();
            services.AddScoped<IRegistrationRepository, RegistrationRepository>();
            services.AddScoped<IGradeRepository, GradeRepository>();
            services.AddScoped<IFinancialHoldRepository, FinancialHoldRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IUserRoleRepository, UserRoleRepository>();
            services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
            
            //Unit of work pattern
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}
