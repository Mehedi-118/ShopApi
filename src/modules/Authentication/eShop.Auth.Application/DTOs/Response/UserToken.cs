namespace eShop.Auth.Application.DTOs.Response;

public class UserToken
{
    public string AccessToken { get; set; } = String.Empty;
    public string RefreshToken { get; set; } = String.Empty;
    public DateTime TokenExpiresIn { get; set; }
    public DateTime RefreashTokenExpiresIn { get; set; }
}