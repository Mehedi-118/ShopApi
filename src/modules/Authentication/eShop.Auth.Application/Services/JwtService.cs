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

    public async Task<Result<UserToken>> GenerateToken(UserTokenGeneratorDto entity,
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
                return Result<UserToken>.Failure(Error.Failure(description: "Failed to generate token"));
            }

            var refreashToken = await GenerateRefreashToken();
            if (string.IsNullOrWhiteSpace(refreashToken))
            {
                return Result<UserToken>.Failure(Error.Failure(description: "Failed to generate refreash token"));
            }

            UserToken userTokenObj = new UserToken
            {
                AccessToken = result,
                RefreshToken = refreashToken,
                TokenExpiresIn = DateTime.Now.AddMinutes(_jwtOptions.ExpiryMinutes),
                RefreashTokenExpiresIn = DateTime.Now.AddMinutes(_jwtOptions.RefreshExpiryMinutes)
            };
            return Result<UserToken>.Success(userTokenObj);
        }
        catch (Exception e)
        {
            return Result<UserToken>.Failure(Error.Failure());
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