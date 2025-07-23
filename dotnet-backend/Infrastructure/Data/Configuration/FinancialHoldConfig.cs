using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configuration;

public class FinancialHoldConfig : IEntityTypeConfiguration<FinancialHold>
{
    public void Configure(EntityTypeBuilder<FinancialHold> builder)
    {
        builder.ToTable("financial_holds");

        builder.HasKey(fh => fh.Id);

        builder.Property(fh => fh.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd();

        builder.Property(fh => fh.Reason)
            .HasColumnName("reason")
            .HasMaxLength(300)
            .IsRequired();

        builder.Property(fh => fh.HoldAmount)
            .HasColumnName("hold_amount")
            .HasColumnType("decimal(10,2)");

        builder.Property(fh => fh.DatePlaced)
            .HasColumnName("date_placed")
            .HasColumnType("timestamp with time zone")
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.Property(fh => fh.DateResolved)
            .HasColumnName("date_resolved")
            .HasColumnType("timestamp with time zone");

        builder.Property(fh => fh.IsActive)
            .HasColumnName("is_active")
            .HasDefaultValue(true);

        builder.Property(fh => fh.ResolutionNotes)
            .HasColumnName("resolution_notes")
            .HasColumnType("varchar")
            .HasMaxLength(500);

        builder.Property(fh => fh.StudentId)
            .HasColumnName("student_id");

        builder.Property(fh => fh.PlacedByUserId)
            .HasColumnName("placed_by_user_id");

        builder.Property(fh => fh.ResolvedByUserId)
            .HasColumnName("resolved_by_user_id");

        builder.HasOne(fh => fh.Student)
            .WithMany()
            .HasForeignKey(fh => fh.StudentId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(fh => fh.PlacedByUser)
            .WithMany()
            .HasForeignKey(fh => fh.PlacedByUserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(fh => fh.ResolvedByUser)
            .WithMany()
            .HasForeignKey(fh => fh.ResolvedByUserId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasIndex(fh => fh.StudentId)
            .HasDatabaseName("ix_financial_holds_student");

        builder.HasIndex(fh => fh.IsActive)
            .HasDatabaseName("ix_financial_holds_active");
    }
}