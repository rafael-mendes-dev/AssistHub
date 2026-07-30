using IdentityService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IdentityService.Infrastructure.Data.Configurations;

public sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users");
        builder.HasKey(u => u.Id);
        builder.Property(u => u.Email).HasMaxLength(255).IsRequired();
        builder.Property(u => u.NormalizedEmail).HasMaxLength(255).IsRequired();
        builder.Property(u => u.FirstName).HasMaxLength(150).IsRequired();
        builder.Property(u => u.LastName).HasMaxLength(150).IsRequired();
        builder.Property(u => u.PasswordHash).HasMaxLength(512).IsRequired();
        builder.Property(u => u.PhoneNumber).HasMaxLength(30);
        builder.Property(u => u.CreatedAt).HasColumnType("timestamp with time zone").IsRequired();
        builder.Property(u => u.UpdatedAt).HasColumnType("timestamp with time zone");
        builder.Property(u => u.DeletedAt).HasColumnType("timestamp with time zone");
        builder.HasIndex(u => u.NormalizedEmail)
            .IsUnique()
            .HasDatabaseName("ux_users_normalized_email");
        builder.HasIndex(u => u.DeletedAt)
            .HasDatabaseName("ix_users_deleted_at")
            .HasFilter("\"IsDeleted\" = true AND \"DeletedAt\" IS NOT NULL");
        builder.HasQueryFilter(u => !u.IsDeleted);
    }
}
