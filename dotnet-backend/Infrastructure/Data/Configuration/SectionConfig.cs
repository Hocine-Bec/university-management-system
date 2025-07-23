using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configuration;


public class SectionConfig : IEntityTypeConfiguration<Section>
{
    public void Configure(EntityTypeBuilder<Section> builder)
    {
        builder.ToTable("sections");

        builder.HasKey(s => s.Id);

        builder.Property(s => s.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd();

        builder.Property(s => s.SectionNumber)
            .HasColumnName("section_number")
            .HasColumnType("varchar")
            .HasMaxLength(10)
            .IsRequired();

        builder.Property(s => s.MeetingDays)
            .HasColumnName("meeting_days")
            .HasColumnType("varchar")
            .HasMaxLength(20);

        builder.Property(s => s.StartTime)
            .HasColumnName("start_time")
            .HasColumnType("time without time zone");

        builder.Property(s => s.EndTime)
            .HasColumnName("end_time")
            .HasColumnType("time without time zone");

        builder.Property(s => s.Classroom)
            .HasColumnName("classroom")
            .HasColumnType("varchar")
            .HasMaxLength(30);

        builder.Property(s => s.MaxCapacity)
            .HasColumnName("max_capacity")
            .HasColumnType("integer")
            .IsRequired();

        builder.Property(s => s.CurrentEnrollment)
            .HasColumnName("current_enrollment")
            .HasColumnType("integer");

        builder.Property(s => s.CourseId)
            .HasColumnName("course_id");

        builder.Property(s => s.SemesterId)
            .HasColumnName("semester_id");

        builder.Property(s => s.ProfessorId)
            .HasColumnName("professor_id");

        builder.HasOne(s => s.Course)
            .WithMany()
            .HasForeignKey(s => s.CourseId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(s => s.Semester)
            .WithMany()
            .HasForeignKey(s => s.SemesterId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(s => s.Professor)
            .WithMany()
            .HasForeignKey(s => s.ProfessorId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasIndex(s => new { s.CourseId, s.SemesterId, s.SectionNumber })
            .IsUnique()
            .HasDatabaseName("ix_sections_course_semester_number");
    }
}
