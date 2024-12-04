using Common.Helpers;

using eShop.Auth.Application.DTOs.Request;
using eShop.Auth.Application.DTOs.Response;

namespace eShop.Auth.Application.Interfaces;

public interface IJwtService
{
    Task<Result<UserToken>>GenerateToken(UserTokenGeneratorDto entity, CancellationToken cancellationToken);
    Task<string> GenerateRefreashToken();
}