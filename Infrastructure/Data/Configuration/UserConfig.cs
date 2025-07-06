using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Configuration
{
    public class UserConfig : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(u => u.Id);

            builder.Property(u => u.Id)
                .HasColumnName("id")
                .ValueGeneratedOnAdd();

            builder.Property(u => u.Username)
                .HasColumnName("username")
                .HasColumnType("varchar")
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(u => u.Password)
                .HasColumnName("password")
                .HasColumnType("varchar")
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(u => u.IsActive)
                .HasColumnName("is_active")
                .HasDefaultValue(true)
                .IsRequired();

            builder.Property(u => u.CreatedAt)
                .HasColumnName("created_at")
                .HasColumnType("timestamp with time zone")
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.Property(u => u.LastLoginAt)
                .HasColumnName("last_login_at")
                .HasColumnType("timestamp with time zone");

            builder.Property(u => u.PersonId)
                .HasColumnName("person_id")
                .HasColumnType("integer")
                .IsRequired();

            builder.HasOne(u => u.Person)
                .WithMany()
                .HasForeignKey(u => u.PersonId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_users_person");

            builder.HasIndex(u => u.Username)
                .IsUnique()
                .HasDatabaseName("ix_users_username");

            builder.HasIndex(u => u.PersonId)
                .IsUnique()
                .HasDatabaseName("ix_users_person_id");

            builder.ToTable("users");
        }
    }
}
