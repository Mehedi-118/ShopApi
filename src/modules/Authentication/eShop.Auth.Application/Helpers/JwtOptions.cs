namespace eShop.Auth.Application.Helpers;

public class JwtOptions
{
    public string Issuer { get; set; } = string.Empty;
    public string Audience { get; set; } = string.Empty;
    public string SigningCredentials { get; set; } = string.Empty;
    public int ExpiryMinutes { get; set; }
    public int RefreshExpiryMinutes { get; set; }
}