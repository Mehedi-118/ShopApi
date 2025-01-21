using eShop.Auth.Domain.Entities.MetaInfo;

namespace eShop.Auth.Application.DTOs.Request;

public class RefreashTokenDto : ICreationMetadata
{
    public string RefreshToken { get; set; } = String.Empty;
    public string PreviousRefreshToken { get; set; } = String.Empty;
    public long UserId { get; set; }
    public DateTime ExpiresOnUtc { get; set; }
    public bool IsRevoked { get; set; }
    public long CreatedBy { get; set; }
    public DateTime CreatedAt { get; init; }
   
}