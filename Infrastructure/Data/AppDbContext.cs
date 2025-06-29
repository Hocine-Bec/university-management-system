using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Student> Students { get; set; }
        public DbSet<Person> People { get; set; }
        public DbSet<Country> Countries { get; set; }  
        public DbSet<User> Users { get; set; }
        public DbSet<Professor> Professors { get; set; }
        public DbSet<ServiceOffer> ServiceOffers { get; set; }
        public DbSet<Program> Programs { get; set; }
        public DbSet<ServiceApplication> ServiceApplications { get; set; }
        public DbSet<DocsVerification> DocsVerifications { get; set; }
        public DbSet<EntranceExam> EntranceExams { get; set; }
        public DbSet<Interview> Interviews { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        }
    }
}
