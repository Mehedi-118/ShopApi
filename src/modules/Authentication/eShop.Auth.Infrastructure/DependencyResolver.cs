using eShop.Auth.Application.Helpers;
using eShop.Auth.Domain.Entities;
using eShop.Auth.Domain.Interfaces;
using eShop.Auth.Infrastructure.DBContext;
using eShop.Auth.Infrastructure.Repositories;
using eShop.Auth.Infrastructure.Settings;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace eShop.Auth.Infrastructure;

public class DependencyResolver : Injector

{
    public void RegisterServices(IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("eShop.Auth.SqlServer");
        services.AddScoped<IEShopAuthRepository, EShopRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddDbContextPool<AuthenticationDbContext>(options =>
        {
            options.UseSqlServer(connectionString, sqlServerOptions =>
            {
                sqlServerOptions.MigrationsAssembly(typeof(IAuthInfrastructureAssemblyMarker).Assembly.FullName);
                sqlServerOptions.EnableRetryOnFailure(15, TimeSpan.FromSeconds(30), null);
            });
        }, 2048);

        services.AddDbContextPool<AuthenticationReadOnlyDbContext>(options =>
        {
            options.UseSqlServer(connectionString, sqlServerOptions =>
            {
                sqlServerOptions.EnableRetryOnFailure(15, TimeSpan.FromSeconds(30), null);
            }).UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        }, 2048);
        services.AddIdentityCore<User>(options =>
            {
                options.Password.RequiredLength = 6; // Customize as needed
                options.Lockout.MaxFailedAccessAttempts = 5; // Customize as needed
            })
            .AddEntityFrameworkStores<AuthenticationDbContext>();
    }
}