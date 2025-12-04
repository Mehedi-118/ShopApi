using Common.Helpers;

using eShop.Auth.Application.DTOs.Request;
using eShop.Auth.Application.DTOs.Response;

namespace eShop.Auth.Application.Interfaces;

public interface IJwtService
{
    Task<Result<UserTokenResponse>> GenerateToken(UserTokenGeneratorDto entity, CancellationToken cancellationToken);

    Task<Result<UserTokenGeneratorDto>> GetPrincipalFromExpiredToken(string accessToken,
        CancellationToken cancellationToken);
    
    Task<Result<long>> GetUserIdByToken(string accessToken,
        CancellationToken cancellationToken);

    Task<string> GenerateRefreashToken();
}