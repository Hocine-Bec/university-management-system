using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configuration;

public class EnrollmentConfig: IEntityTypeConfiguration<Enrollment>
    {
        public void Configure(EntityTypeBuilder<Enrollment> builder)
        {
            builder.ToTable("enrollments");

            builder.HasKey(e => e.Id);

            builder.Property(e => e.Id)
                .HasColumnName("id")
                .ValueGeneratedOnAdd();

            builder.Property(e => e.EnrollmentDate)
                .HasColumnName("enrollment_date")
                .HasColumnType("date");

            builder.Property(e => e.ActualGradDate)
                .HasColumnName("actual_grad_date")
                .HasColumnType("date");

            builder.Property(e => e.Status)
                .HasColumnName("enrollment_status")
                .HasConversion<int>();
            
            builder.Property(e => e.Notes)
                .HasColumnName("notes")
                .HasColumnType("varchar")
                .HasMaxLength(500);

            builder.Property(e => e.StudentId)
                .HasColumnName("student_id");

            builder.Property(e => e.ProgramId)
                .HasColumnName("program_id");

            builder.HasOne(e => e.Student)
                .WithMany()
                .HasForeignKey(e => e.StudentId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.Program)
                .WithMany()
                .HasForeignKey(e => e.ProgramId)
                .OnDelete(DeleteBehavior.Restrict);
            
            builder.Property(e => e.ServiceApplicationId)
                .HasColumnName("service_application_id");

            builder.HasOne(e => e.ServiceApplication)
                .WithOne()
                .HasForeignKey<Enrollment>(e => e.ServiceApplicationId)
                .OnDelete(DeleteBehavior.Restrict);
            
            builder.HasIndex(e => new { e.StudentId, e.ProgramId })
                .IsUnique()
                .HasDatabaseName("ix_enrollments_student_program");
        }
    }