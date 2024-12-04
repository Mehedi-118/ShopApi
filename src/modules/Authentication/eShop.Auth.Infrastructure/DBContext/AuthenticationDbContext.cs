using eShop.Auth.Domain.Entities;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace eShop.Auth.Infrastructure.DBContext;

public class AuthenticationDbContext(DbContextOptions<AuthenticationDbContext> options)
    :   IdentityDbContext<User, Role, long, 
        IdentityUserClaim<long>, UserRole, 
        IdentityUserLogin<long>,
        IdentityRoleClaim<long>, 
        IdentityUserToken<long>>(options)
{
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfigurationsFromAssembly(typeof(AuthenticationDbContext).Assembly);
    }
}