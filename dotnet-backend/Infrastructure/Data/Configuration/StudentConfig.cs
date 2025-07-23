using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configuration
{
    public class StudentConfig : IEntityTypeConfiguration<Student>
    {
        public void Configure(EntityTypeBuilder<Student> builder)
        {
            builder.HasKey(x => x.Id);
            
            builder.Property(x => x.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id");

            builder.Property(s => s.StudentNumber)
                .HasColumnName("student_number")
                .HasColumnType("varchar")
                .HasMaxLength(10)
                .IsRequired();

            builder.Property(s => s.StudentStatus)
                .HasColumnName("student_status")
                .HasConversion<int>();

            builder.Property(s => s.Notes)
                .HasColumnName("notes")
                .HasColumnType("varchar")
                .HasMaxLength(1000);

            builder.Property(s => s.PersonId)
                .HasColumnName("person_id")
                .HasColumnType("integer")
                .IsRequired();

            builder.HasOne(p => p.Person)
                .WithOne()
                .HasForeignKey<Student>(x => x.PersonId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(s => s.PersonId)
                .IsUnique()
                .HasDatabaseName("ix_students_person_id");

            builder.HasIndex(s => s.StudentNumber)
                .IsUnique()
                .HasDatabaseName("ix_students_student_number");

            builder.ToTable("students");
        }
    }
}
