namespace eShop.Auth.Application.DTOs.Response;

public class UserTokenResponse
{
    public string AccessToken { get; set; } = String.Empty;
    public string RefreashToken { get; set; } = String.Empty;
    public DateTime TokenExpiresOn { get; set; }
    public DateTime RefreashTokenExpiresOn { get; set; }
}