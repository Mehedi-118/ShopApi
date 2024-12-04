using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace eShop.Auth.Application.Helpers;

public interface Injector
{
    void RegisterServices(IServiceCollection services, IConfiguration configuration);
}