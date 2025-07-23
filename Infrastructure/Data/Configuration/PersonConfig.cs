using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configuration;

public class PersonConfig : IEntityTypeConfiguration<Person>
{
    public void Configure(EntityTypeBuilder<Person> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
            .ValueGeneratedOnAdd()
            .HasColumnName("id");

        builder.Property(p => p.FirstName)
            .HasColumnName("first_name")
            .HasColumnType("varchar")
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(p => p.LastName)
            .HasColumnName("last_name")
            .HasColumnType("varchar")
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(p => p.DOB)
            .HasColumnName("date_of_birth")
            .HasColumnType("date")
            .IsRequired();

        builder.Property(p => p.Address)
            .HasColumnName("address")
            .HasColumnType("varchar")
            .HasMaxLength(300);

        builder.Property(p => p.PhoneNumber)
            .HasColumnName("phone_number")
            .HasColumnType("varchar")
            .HasMaxLength(20);

        builder.Property(p => p.Email)
            .HasColumnName("email")
            .HasColumnType("varchar")
            .HasMaxLength(100);

        builder.Property(p => p.ImagePath)
            .HasColumnName("image_path")
            .HasColumnType("varchar")
            .HasMaxLength(500);

        builder.Property(p => p.CountryId)
            .HasColumnName("country_id")
            .HasColumnType("integer");

        builder.HasOne(p => p.Country)
            .WithMany()
            .HasForeignKey(p => p.CountryId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("fk_persons_country");

        builder.HasIndex(p => p.LastName)
            .HasDatabaseName("ix_persons_last_name");

        builder.ToTable("people");
    }
}