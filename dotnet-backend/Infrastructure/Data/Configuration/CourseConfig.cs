using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configuration;

public class CourseConfig : IEntityTypeConfiguration<Course>
{
    public void Configure(EntityTypeBuilder<Course> builder)
    {
        builder.ToTable("courses");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd();

        builder.Property(c => c.Code)
            .HasColumnName("code")
            .HasColumnType("varchar")
            .HasMaxLength(10)
            .IsRequired();

        builder.Property(c => c.Title)
            .HasColumnName("title")
            .HasColumnType("varchar")
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(c => c.Description)
            .HasColumnName("description")
            .HasColumnType("varchar")
            .HasMaxLength(1000);

        builder.Property(c => c.CreditHours)
            .HasColumnName("credit_hours")
            .HasColumnType("integer");

        builder.Property(c => c.IsActive)
            .HasColumnName("is_active")
            .HasDefaultValue(true);

        builder.HasIndex(c => c.Code)
            .IsUnique()
            .HasDatabaseName("ix_courses_code");
    }
}