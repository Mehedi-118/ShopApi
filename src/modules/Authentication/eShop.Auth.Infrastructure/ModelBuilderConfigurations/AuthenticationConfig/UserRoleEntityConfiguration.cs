using eShop.Auth.Domain.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eShop.Auth.Infrastructure.ModelBuilderConfigurations.AuthenticationConfig;

public class UserRoleEntityConfiguration : IEntityTypeConfiguration<UserRole>
{
    public void Configure(EntityTypeBuilder<UserRole> builder)
    {
        builder.ToTable("UserRoles", "Authentication");
        //builder.HasKey(x => new { x.RoleId, x.UserId });
        //builder.HasOne(a => a.User)
        //       .WithMany(b => b.UserRoles)
        //       .HasForeignKey(v => v.UserId);
    }
}