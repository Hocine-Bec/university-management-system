using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configuration;

public class SemesterConfig : IEntityTypeConfiguration<Semester>
    {
        public void Configure(EntityTypeBuilder<Semester> builder)
        {
            builder.ToTable("semesters");

            builder.HasKey(s => s.Id);

            builder.Property(s => s.Id)
                .HasColumnName("id")
                .ValueGeneratedOnAdd();

            builder.Property(s => s.TermCode)
                .HasColumnName("term_code")
                .HasColumnType("varchar")
                .HasMaxLength(8)
                .IsRequired();

            builder.Property(s => s.Year)
                .HasColumnName("year")
                .HasColumnType("integer")
                .IsRequired();

            builder.Property(s => s.Term)
                .HasColumnName("term")
                .HasMaxLength(20)
                .IsRequired();

            builder.Property(s => s.StartDate)
                .HasColumnName("start_date")
                .HasColumnType("date");

            builder.Property(s => s.EndDate)
                .HasColumnName("end_date")
                .HasColumnType("date");

            builder.Property(s => s.RegStartsAt)
                .HasColumnName("registration_start_date")
                .HasColumnType("date");

            builder.Property(s => s.RegEndsAt)
                .HasColumnName("registration_end_date")
                .HasColumnType("date");

            builder.Property(s => s.IsActive)
                .HasColumnName("is_active")
                .HasDefaultValue(true);

            builder.HasIndex(s => new { s.Year, s.Term })
                .IsUnique()
                .HasDatabaseName("ix_semesters_year_term");
        }
    }
