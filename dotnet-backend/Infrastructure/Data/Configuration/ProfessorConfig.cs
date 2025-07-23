using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configuration;

public class ProfessorConfig : IEntityTypeConfiguration<Professor>
{
    public void Configure(EntityTypeBuilder<Professor> builder)
    {
        builder.ToTable("professors");
        
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd();
        
        builder.Property(p => p.EmployeeNumber)
            .HasColumnName("employee_number")
            .HasColumnType("varchar")
            .HasMaxLength(10)
            .IsRequired();

        builder.Property(p => p.AcademicRank)
            .HasColumnName("academic_rank")
            .HasConversion<int>();
        
        builder.Property(p => p.HireDate)
            .HasColumnName("hire_date")
            .HasColumnType("date");

        builder.Property(p => p.Specialization)
            .HasColumnName("specialization")
            .HasColumnType("varchar")
            .HasMaxLength(50);

        builder.Property(p => p.OfficeLocation)
            .HasColumnName("office_location")
            .HasColumnType("varchar")
            .HasMaxLength(70);

        builder.Property(p => p.Salary)
            .HasColumnName("salary")
            .HasColumnType("decimal(10,2)");

        builder.Property(p => p.IsActive)
            .HasColumnName("is_active")
            .HasDefaultValue(true);

        builder.Property(p => p.PersonId)
            .HasColumnName("person_id");

        builder.HasOne(x => x.Person)
            .WithOne()
            .HasForeignKey<Professor>(p => p.PersonId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(p => p.EmployeeNumber)
            .IsUnique()
            .HasDatabaseName("ix_professors_employee_number");
    }
}