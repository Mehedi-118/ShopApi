using eShop.Auth.Domain.Entities;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace eShop.Auth.Infrastructure.DBContext;

public class AuthenticationReadOnlyDbContext(
    DbContextOptions<AuthenticationReadOnlyDbContext> options)
    : IdentityDbContext<User, Role, long,
        IdentityUserClaim<long>, UserRole,
        IdentityUserLogin<long>,
        IdentityRoleClaim<long>,
        IdentityUserToken<long>>(options)
{
    public DbSet<RefreashToken> RefreashTokens { get; set; }
    public DbSet<PasswordPolicy> PasswordPolicySet { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<User> Users { get; set; }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfigurationsFromAssembly(typeof(AuthenticationReadOnlyDbContext).Assembly);
    }
}