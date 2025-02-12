using eShop.Auth.Domain.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eShop.Auth.Infrastructure.ModelBuilderConfigurations.AuthenticationConfig;

public class RoleEntityConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.ToTable("Roles", "Authentication");
        builder.HasKey(x => x.Id);
        builder.HasIndex(x => x.Id);
        builder.HasMany(a => a.UserRoles).WithOne(b => b.Role).HasForeignKey(c => c.RoleId);

    }
}