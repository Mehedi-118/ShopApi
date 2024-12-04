using System.IdentityModel.Tokens.Jwt;
using System.Text;

using eShop.Auth.Application.Helpers;
using eShop.Auth.Application.Interfaces;

using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace eShop.Auth.Middleware;

public class JwtMiddleware(RequestDelegate next, IOptions<JwtOptions> options, IConfiguration configuration)
{
    private readonly IConfiguration _configuration = configuration;
    private JwtOptions _jwtOptions = options.Value;

    public async Task Invoke(HttpContext context, IJwtService jwtService)
    {
        var endpoint = context.GetEndpoint();

        // Skip this middleware if the endpoint has [AllowAnonymous] attribute
        if (endpoint?.Metadata?.GetMetadata<AllowAnonymousAttribute>() != null)
        {
            await next(context);
            return;
        }

        try
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            if (token is not null)
            {
                var key = Encoding.UTF8.GetBytes(_jwtOptions.SigningCredentials);
                var tokenHandler = new JwtSecurityTokenHandler();
                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidIssuer = _jwtOptions.Issuer,
                    ValidAudience = _jwtOptions.Audience,
                    ClockSkew = TimeSpan.Zero
                };
                var principal = tokenHandler.ValidateToken(token, validationParameters, out _);
                context.User = principal;
                await next(context);
            }
            else
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return;
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsync($"Invalid or expired token");
            return;
        }

        
    }
}