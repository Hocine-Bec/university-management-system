using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configuration;

public class ServiceApplicationConfig : IEntityTypeConfiguration<ServiceApplication>
{
    public void Configure(EntityTypeBuilder<ServiceApplication> builder)
    {
        builder.ToTable("service_applications");
        
        builder.HasKey(sa => sa.Id);
        
        builder.Property(sa => sa.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd();
        
        builder.Property(sa => sa.ApplicationDate)
            .HasColumnName("application_date")
            .HasColumnType("date")
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .IsRequired();

        builder.Property(sa => sa.Status)
            .HasColumnName("status")
            .HasConversion<int>()
            .HasDefaultValue(ApplicationStatus.New);
        
        builder.Property(sa => sa.PaidFees)
            .HasColumnName("paid_fees")
            .HasColumnType("decimal(8,2)")
            .IsRequired();
        
        builder.Property(sa => sa.Notes)
            .HasColumnName("notes")
            .HasColumnType("varchar")
            .HasMaxLength(500);
        
        builder.Property(sa => sa.CompletedDate)
            .HasColumnName("completed_date")
            .HasColumnType("date");
        
        builder.Property(sa => sa.PersonId)
            .HasColumnName("person_id");
        
        builder.Property(sa => sa.ServiceOfferId)
            .HasColumnName("service_id");
        
        builder.Property(sa => sa.ProcessedByUserId)
            .HasColumnName("processed_by_user_id");
        
        builder.HasOne(sa => sa.Person)
            .WithMany()
            .HasForeignKey(sa => sa.PersonId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasOne(sa => sa.ServiceOffer)
            .WithMany()
            .HasForeignKey(sa => sa.ServiceOfferId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasOne(sa => sa.ProcessedByUser)
            .WithMany()
            .HasForeignKey(sa => sa.ProcessedByUserId)
            .OnDelete(DeleteBehavior.SetNull);
        
        builder.HasIndex(sa => sa.Status)
            .HasDatabaseName("ix_service_applications_status");
    }
}
