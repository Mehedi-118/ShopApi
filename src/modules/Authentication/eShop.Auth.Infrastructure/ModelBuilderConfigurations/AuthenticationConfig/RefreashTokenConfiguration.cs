using eShop.Auth.Domain.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eShop.Auth.Infrastructure.ModelBuilderConfigurations.AuthenticationConfig;

public class RefreashTokenConfiguration : IEntityTypeConfiguration<RefreashToken>
{
    public void Configure(EntityTypeBuilder<RefreashToken> builder)
    {
        builder.ToTable("RefreashToken", "Authentication");
        builder.HasOne(a => a.User).WithMany().HasForeignKey(b => b.UserId);
    }
}