using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configuration;

public class GradeConfig : IEntityTypeConfiguration<Grade>
{
    public void Configure(EntityTypeBuilder<Grade> builder)
    {
        builder.ToTable("grades");

        builder.HasKey(g => g.Id);

        builder.Property(g => g.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd();

        builder.Property(g => g.Score)
            .HasColumnName("score")
            .HasColumnType("numeric(5,2)");

        builder.Property(g => g.DateRecorded)
            .HasColumnName("date_recorded")
            .HasColumnType("timestamp with time zone")
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.Property(g => g.Comments)
            .HasColumnName("comments")
            .HasColumnType("varchar")
            .HasMaxLength(200);

        builder.Property(g => g.StudentId)
            .HasColumnName("student_id");

        builder.Property(g => g.CourseId)
            .HasColumnName("course_id");

        builder.Property(g => g.SemesterId)
            .HasColumnName("semester_id");

        builder.Property(g => g.RegistrationId)
            .HasColumnName("registration_id");

        builder.HasOne(g => g.Student)
            .WithMany()
            .HasForeignKey(g => g.StudentId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(g => g.Course)
            .WithMany()
            .HasForeignKey(g => g.CourseId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(g => g.Semester)
            .WithMany()
            .HasForeignKey(g => g.SemesterId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(g => g.Registration)
            .WithMany()
            .HasForeignKey(g => g.RegistrationId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasIndex(g => new { g.StudentId, g.CourseId, g.SemesterId })
            .IsUnique()
            .HasDatabaseName("ix_grades_student_course_semester");
    }
}