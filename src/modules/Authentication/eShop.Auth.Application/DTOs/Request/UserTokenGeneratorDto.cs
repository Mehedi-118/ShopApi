namespace eShop.Auth.Application.DTOs.Request;

public class UserTokenGeneratorDto
{
    public long UserId { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string UserRole { get; set; }
}