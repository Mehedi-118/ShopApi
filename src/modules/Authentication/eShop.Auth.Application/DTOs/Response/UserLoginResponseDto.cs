namespace eShop.Auth.Application.DTOs.Response;

public class UserLoginResponseDto
{
    public long UserId { get; set; }
    public string UserName { get; set; } = String.Empty;
    public string PhoneNumber { get; set; } = String.Empty;
    
    public string Email { get; set; } = String.Empty;
    public UserToken? UserToken { get; set; } = default;
    public string[] Roles { get; set; }
    public string[] Permissions { get; set; }
    public string[] Claims { get; set; }
    public string[] Errors { get; set; }
}