using eShop.Auth.Domain.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eShop.Auth.Infrastructure.ModelBuilderConfigurations.AuthenticationConfig;

public class UserEntityConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users", "Authentication");
        builder.HasKey(x => x.Id);
        builder.HasIndex(a => a.PhoneNumber);
        builder.HasIndex((a => a.Email));
        builder.HasMany(a => a.UserRoles).WithOne(b => b.User).HasForeignKey(c => c.UserId);
    }
}