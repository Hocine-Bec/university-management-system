using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configuration;

public class DocsVerificationConfig : IEntityTypeConfiguration<DocsVerification>
{
   public void Configure(EntityTypeBuilder<DocsVerification> builder)
    {
        builder.ToTable("docs_verifications");

        builder.HasKey(dv => dv.Id);

        builder.Property(dv => dv.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd();
        
        builder.Property(dv => dv.SubmissionDate)
            .HasColumnName("submission_date")
            .HasColumnType("timestamp with time zone")
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.Property(dv => dv.VerificationDate)
            .HasColumnName("verification_date")
            .HasColumnType("timestamp with time zone");

        builder.Property(x => x.Status)
            .HasColumnName("status")
            .HasConversion<int>()
            .HasDefaultValue(VerificationStatus.Pending);

        builder.Property(dv => dv.IsApproved)
            .HasColumnName("is_approved")
            .HasDefaultValue(null);

        builder.Property(x => x.RejectedReason)
            .HasColumnName("rejected_reason")
            .HasColumnType("varchar")
            .HasMaxLength(200)
            .IsRequired(false);

        builder.Property(dv => dv.PaidFees)
            .HasColumnName("paid_fees")
            .HasColumnType("decimal(10,2)");

        builder.Property(dv => dv.Notes)
            .HasColumnName("notes")
            .HasMaxLength(500)
            .HasColumnType("varchar");

        builder.Property(dv => dv.VerifiedByUserId)
            .HasColumnName("verified_by_user_id")
            .HasColumnType("integer");

        builder.HasOne(dv => dv.VerifiedByUser)
            .WithMany()
            .HasForeignKey(e => e.VerifiedByUserId)
            .OnDelete(DeleteBehavior.SetNull);
        
        builder.Property(dv => dv.PersonId)
            .HasColumnName("person_id")
            .HasColumnType("integer");

        builder.HasOne(dv => dv.Person)
            .WithMany()
            .HasForeignKey(e => e.PersonId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}