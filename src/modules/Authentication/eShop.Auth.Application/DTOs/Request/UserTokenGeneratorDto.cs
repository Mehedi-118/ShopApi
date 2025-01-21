namespace eShop.Auth.Application.DTOs.Request;

public class UserTokenGeneratorDto
{
    public long UserId { get; set; }
    public string? Username { get; set; } = string.Empty;
    public string? Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string UserRole { get; set; } = string.Empty;
}