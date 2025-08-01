﻿using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
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
    public DbSet<Semester> Semesters { get; set; }
    public DbSet<Course> Courses { get; set; }
    public DbSet<Prerequisite> Prerequisites { get; set; }
    public DbSet<Section> Sections { get; set; }
    public DbSet<Registration> Registrations { get; set; }
    public DbSet<Grade> Grades { get; set; }
    public DbSet<FinancialHold> FinancialHolds { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}