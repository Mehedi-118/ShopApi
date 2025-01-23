using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

using Common.Helpers;

using eShop.Auth.Application.DTOs.Request;
using eShop.Auth.Application.DTOs.Response;
using eShop.Auth.Application.Helpers;
using eShop.Auth.Application.Interfaces;

using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace eShop.Auth.Application.Services;

public class JwtService(IOptions<JwtOptions> options) : IJwtService
{
    private readonly JwtOptions _jwtOptions = options.Value;

    public async Task<Result<UserTokenResponse>> GenerateToken(UserTokenGeneratorDto entity,
        CancellationToken cancellationToken)
    {
        try
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SigningCredentials));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Name, entity.Username),
                new Claim(JwtRegisteredClaimNames.Email, entity.Email),
                new Claim(JwtRegisteredClaimNames.Sid, entity.UserId.ToString()),
            };
            var token = new JwtSecurityToken(
                issuer: _jwtOptions.Issuer,
                audience: _jwtOptions.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtOptions.ExpiryMinutes),
                signingCredentials: credentials
            );

            var result = new JwtSecurityTokenHandler().WriteToken(token);
            if (result is null)
            {
                return Result<UserTokenResponse>.Failure(Error.Failure(description: "Failed to generate token"));
            }

            var refreashToken = await GenerateRefreashToken();
            if (string.IsNullOrWhiteSpace(refreashToken))
            {
                return Result<UserTokenResponse>.Failure(Error.Failure(description: "Failed to generate refreash token"));
            }

            UserTokenResponse userTokenResponseObj = new UserTokenResponse
            {
                AccessToken = result,
                RefreashToken = refreashToken,
                TokenExpiresOn = DateTime.UtcNow.AddMinutes(_jwtOptions.ExpiryMinutes),
                RefreashTokenExpiresOn = DateTime.UtcNow.AddMinutes(_jwtOptions.RefreshExpiryMinutes)
            };
            return Result<UserTokenResponse>.Success(userTokenResponseObj);
        }
        catch (Exception e)
        {
            return Result<UserTokenResponse>.Failure(Error.Failure());
        }
    }

    public async Task<Result<UserTokenGeneratorDto>> GetPrincipalFromExpiredToken(string accessToken,
        CancellationToken cancellationToken)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
        JwtSecurityTokenHandler.DefaultInboundClaimTypeMap["email"] = JwtRegisteredClaimNames.Email;
        var key = Encoding.ASCII.GetBytes(_jwtOptions.SigningCredentials);
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = false,
            ClockSkew = TimeSpan.Zero
        };
        try
        {
            var principal = tokenHandler.ValidateToken(accessToken, tokenValidationParameters, out var securityToken);
            if (principal is null || principal.Identity?.IsAuthenticated is false)
            {
                return await Task.FromResult(
                    Result<UserTokenGeneratorDto>.Failure(
                        Error.Failure(description: "Failed to get principal from token")));
            }

            return principal.Claims.Any()
                ? Result<UserTokenGeneratorDto>.Success(new UserTokenGeneratorDto
                {
                    UserId = Convert.ToUInt32(principal.Claims?
                        .FirstOrDefault(claim => claim.Type == JwtRegisteredClaimNames.Sid)?.Value),
                    Username =
                        principal.Claims?.FirstOrDefault(claim => claim.Type == JwtRegisteredClaimNames.Name)?.Value,
                    Email = principal.Claims?.FirstOrDefault(claim => claim.Type == JwtRegisteredClaimNames.Email)?.Value
                })
                : Result<UserTokenGeneratorDto>.Failure(
                    Error.Failure(description: "Failed to get principal from token"));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return Result<UserTokenGeneratorDto>.Failure(
                Error.Failure(description: "Failed to get principal from token", errorType: ErrorType.Unauthorized));
        }
    }

    public Task<string> GenerateRefreashToken()
    {
        var randomNumber = new byte[64];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomNumber);
        }

        // Return the refresh token as a Base64 string
        var refreashToken = Convert.ToBase64String(randomNumber);

        return Task.FromResult(refreashToken ?? string.Empty);
    }


}