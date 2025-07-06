using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configuration;

public class RoleConfig : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.ToTable("roles");
        
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .HasColumnName("Id")
            .ValueGeneratedOnAdd();

        builder.Property(x => x.Name)
            .HasColumnName("role_name")
            .HasConversion<int>();

        builder.Property(x => x.Description)
            .HasColumnName("description")
            .HasColumnType("varchar")
            .HasMaxLength(250);
        
        builder.HasIndex(x => x.Name)
            .IsUnique()
            .HasDatabaseName("idx_role_name");
    }
}