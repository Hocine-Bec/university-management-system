using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configuration;

public class InterviewConfig : IEntityTypeConfiguration<Interview>
{
    public void Configure(EntityTypeBuilder<Interview> builder)
    {
        builder.ToTable("interviews");

        builder.HasKey(i => i.Id);

        builder.Property(i => i.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd();

        builder.Property(i => i.ScheduledDate)
            .HasColumnName("scheduled_date_time")
            .HasColumnType("timestamp with time zone");

        builder.Property(i => i.StartTime)
            .HasColumnName("actual_start_time")
            .HasColumnType("timestamp with time zone")
            .IsRequired(false);

        builder.Property(i => i.EndTime)
            .HasColumnName("actual_end_time")
            .HasColumnType("timestamp with time zone")
            .IsRequired(false);

        builder.Property(i => i.IsApproved)
            .HasColumnName("is_approved")
            .HasDefaultValue(false);

        builder.Property(i => i.PaidFees)
            .HasColumnName("paid_fees")
            .HasColumnType("decimal(8,2)");

        builder.Property(i => i.Notes)
            .HasColumnName("notes")
            .HasMaxLength(500);

        builder.Property(i => i.Recommendation)
            .HasColumnName("recommendation")
            .HasMaxLength(200);

        builder.Property(i => i.ProfessorId)
            .HasColumnName("professor_id");

        builder.HasOne(i => i.Professor)
            .WithMany()
            .HasForeignKey(i => i.ProfessorId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}