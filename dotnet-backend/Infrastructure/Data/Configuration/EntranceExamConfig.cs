using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configuration;

public class EntranceExamConfig : IEntityTypeConfiguration<EntranceExam>
{
    public void Configure(EntityTypeBuilder<EntranceExam> builder)
    {
        builder.ToTable("entrance_exams");

        builder.HasKey(ee => ee.Id);

        builder.Property(ee => ee.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd();

        builder.Property(ee => ee.ExamDate)
            .HasColumnName("exam_date")
            .HasColumnType("date");

        builder.Property(ee => ee.Score)
            .HasColumnName("score")
            .HasColumnType("numeric(5,2)");

        builder.Property(ee => ee.MaxScore)
            .HasColumnName("max_score")
            .HasColumnType("integer")
            .IsRequired();

        builder.Property(ee => ee.PassingScore)
            .HasColumnName("passing_score")
            .HasColumnType("integer");
        
        builder.Property(ee => ee.IsPassed)
            .HasColumnName("is_passed")
            .HasDefaultValue(null);

        builder.Property(ee => ee.PaidFees)
            .HasColumnName("paid_fees")
            .HasColumnType("decimal(10,2)")
            .HasDefaultValue(50.00m);

        builder.Property(ee => ee.ExamStatus)
            .HasColumnName("exam_status")
            .HasConversion<int>();

        builder.Property(ee => ee.Notes)
            .HasColumnName("notes")
            .HasMaxLength(500);
    }
}
