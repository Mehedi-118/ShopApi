namespace eShop.Auth.Application.DTOs.Request;

public class UserLoginDto
{
    public string UserName { get; set; } = String.Empty;
    public string UserEmail { get; set; } = String.Empty;
    public string Password { get; set; } = String.Empty;
}