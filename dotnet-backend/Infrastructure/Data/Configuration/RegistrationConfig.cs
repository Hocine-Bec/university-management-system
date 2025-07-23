using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configuration;

public class RegistrationConfig: IEntityTypeConfiguration<Registration>
    {
        public void Configure(EntityTypeBuilder<Registration> builder)
        {
            builder.ToTable("registrations");

            builder.HasKey(r => r.Id);

            builder.Property(r => r.Id)
                .HasColumnName("id")
                .ValueGeneratedOnAdd();

            builder.Property(r => r.RegistrationDate)
                .HasColumnName("registration_date")
                .HasColumnType("timestamp with time zone")
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.Property(r => r.RegistrationFees)
                .HasColumnName("registration_fees")
                .HasColumnType("decimal(8,2)");

            builder.Property(r => r.StudentId)
                .HasColumnName("student_id");

            builder.Property(r => r.SectionId)
                .HasColumnName("section_id");

            builder.Property(r => r.SemesterId)
                .HasColumnName("semester_id");

            builder.Property(r => r.ProcessedByUserId)
                .HasColumnName("processed_by_user_id");

            builder.HasOne(r => r.Student)
                .WithMany()
                .HasForeignKey(r => r.StudentId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(r => r.Section)
                .WithMany()
                .HasForeignKey(r => r.SectionId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(r => r.Semester)
                .WithMany()
                .HasForeignKey(r => r.SemesterId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(r => r.ProcessedByUser)
                .WithMany()
                .HasForeignKey(r => r.ProcessedByUserId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasIndex(r => new { r.StudentId, r.SectionId, r.SemesterId })
                .IsUnique()
                .HasDatabaseName("ix_registrations_student_section_semester");
        }
    }