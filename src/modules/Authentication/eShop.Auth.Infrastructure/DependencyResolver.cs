using eShop.Auth.Application.Helpers;
using eShop.Auth.Domain.Interfaces;
using eShop.Auth.Infrastructure.Repositories;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace eShop.Auth.Infrastructure;

public class DependencyResolver : Injector

{
    public void RegisterServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IEShopAuthRepository, EShopRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
    }
}