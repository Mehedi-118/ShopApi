using Common.Helpers;

namespace eShop.Auth.Application.DTOs.Response;

public class UserRegisterResponseDto
{
    public string UserName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}