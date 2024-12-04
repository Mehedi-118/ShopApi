using eShop.Auth.Application.Helpers;
using eShop.Auth.Application.Interfaces;
using eShop.Auth.Application.Services;
using eShop.Auth.Application.UserCase.Implementation;
using eShop.Auth.Application.UserCase.Interfaces;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace eShop.Auth.Application;

public class DependencyResolver : Injector
{
    public void RegisterServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IUserUseCase, UserUseCase>();
        services.AddScoped<IEShopAuthService, EShopAuthAuthService>();
        services.AddScoped<IJwtService, JwtService>();
        // Use Action<JwtOptions> for dynamic configuration
        services.Configure<JwtOptions>(options =>
        {
            var jwtSection = configuration.GetSection("JwtOptions");
            options.Issuer = jwtSection["Issuer"];
            options.Audience = jwtSection["Audience"];
            options.SigningCredentials = jwtSection["SigningCredentials"];
            
            if (int.TryParse(jwtSection["ExpiryMinutes"], out int expiryMinutes))
            {
                options.ExpiryMinutes = expiryMinutes;
            }
            if (int.TryParse(jwtSection["RefreshExpiryMinutes"], out int refreshExpiryMinutes))
            {
                options.RefreshExpiryMinutes = refreshExpiryMinutes;
            }
            
            
        });
    }
}