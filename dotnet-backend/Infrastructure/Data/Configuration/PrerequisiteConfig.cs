using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configuration;

public class PrerequisiteConfig: IEntityTypeConfiguration<Prerequisite>
{
    public void Configure(EntityTypeBuilder<Prerequisite> builder)
    {
        builder.ToTable("prerequisites");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd();

        builder.Property(p => p.MinimumGrade)
            .HasColumnName("minimum_grade")
            .HasColumnType("decimal(4,2)");

        builder.Property(p => p.CourseId)
            .HasColumnName("course_id");

        builder.Property(p => p.PrerequisiteCourseId)
            .HasColumnName("prerequisite_course_id");

        builder.HasOne(p => p.Course)
            .WithMany()
            .HasForeignKey(p => p.CourseId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("fk_prerequisites_course");

        builder.HasOne(p => p.PrerequisiteCourse)
            .WithMany()
            .HasForeignKey(p => p.PrerequisiteCourseId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("fk_prerequisites_prerequisite_course");

        builder.HasIndex(p => new { p.CourseId, p.PrerequisiteCourseId })
            .IsUnique()
            .HasDatabaseName("ix_prerequisites_course_prerequisite");
    }
}
