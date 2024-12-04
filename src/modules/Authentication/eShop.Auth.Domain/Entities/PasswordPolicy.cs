using eShop.Auth.Domain.Entities.MetaInfo;

namespace eShop.Auth.Domain.Entities;

public class PasswordPolicy
{
    public int MinLength { get; set; }
    public bool RequireUppercase { get; set; } = true;
    public bool RequireLowercase { get; set; } = true;
    public bool RequireDigit { get; set; } = true;
    public bool RequireSpecialCharacter { get; set; } = true;
}