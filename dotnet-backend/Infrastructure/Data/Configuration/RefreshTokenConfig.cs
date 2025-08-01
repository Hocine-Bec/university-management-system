using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configuration;

public class RefreshTokenConfig : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.HasKey(rt => rt.Id);
        
        builder.Property(rt => rt.Token)
            .IsRequired()
            .HasMaxLength(200);
            
        builder.HasIndex(rt => rt.Token)
            .IsUnique();

        builder.Property(x => x.RevokedReason)
            .HasColumnName("revoked_reason")
            .HasColumnType("varchar")
            .HasMaxLength(200);
        
        builder.Property(x=>x.RevokedAt)
            .HasColumnName("revoked_at")
            .HasColumnType("datetime");
        
        builder.Property(x => x.UserId)
            .HasColumnName("user_id");
        
        builder.HasOne(rt => rt.User)
            .WithMany(u => u.RefreshTokens)
            .HasForeignKey(rt => rt.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}