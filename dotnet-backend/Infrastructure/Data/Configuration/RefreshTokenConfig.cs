using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configuration;

public class RefreshTokenConfig : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.ToTable("refresh_tokens");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("id")
            .UseIdentityColumn();

        builder.Property(x => x.Token)
            .HasColumnName("token")
            .HasColumnType("varchar")
            .HasMaxLength(200)
            .IsRequired();

        builder.HasIndex(x => x.Token)
            .IsUnique()
            .HasDatabaseName("ix_refresh_tokens_token");

        builder.Property(x => x.CreatedAt)
            .HasColumnName("created_at")
            .HasColumnType("timestamp with time zone")
            .IsRequired();

        builder.Property(x => x.ExpiresAt)
            .HasColumnName("expires_at")
            .HasColumnType("timestamp with time zone")
            .IsRequired();

        builder.Property(x => x.IsRevoked)
            .HasColumnName("is_revoked")
            .HasColumnType("boolean")
            .HasDefaultValue(false)
            .IsRequired();

        builder.Property(x => x.RevokedReason)
            .HasColumnName("revoked_reason")
            .HasColumnType("varchar")
            .HasMaxLength(200)
            .IsRequired(false);

        builder.Property(x => x.RevokedAt)
            .HasColumnName("revoked_at")
            .HasColumnType("timestamp with time zone")
            .IsRequired(false);

        builder.Property(x => x.UserId)
            .HasColumnName("user_id")
            .IsRequired();

        // Navigation property
        builder.HasOne(x => x.User)
            .WithMany(u => u.RefreshTokens)
            .HasForeignKey(x => x.UserId)
            .HasConstraintName("fk_refresh_tokens_users_user_id")
            .OnDelete(DeleteBehavior.Cascade);

        // Ignore computed property
        builder.Ignore(x => x.IsActive);

        builder.HasIndex(x => x.UserId)
            .HasDatabaseName("ix_refresh_tokens_user_id");

        builder.HasIndex(x => x.ExpiresAt)
            .HasDatabaseName("ix_refresh_tokens_expires_at");

        builder.HasIndex(x => new { x.UserId, x.IsRevoked, x.ExpiresAt })
            .HasDatabaseName("ix_refresh_tokens_user_active");
    }
}