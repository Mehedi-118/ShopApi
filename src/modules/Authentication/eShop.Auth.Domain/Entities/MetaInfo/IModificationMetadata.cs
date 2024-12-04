namespace eShop.Auth.Domain.Entities.MetaInfo;

public interface IModificationMetadata
{
    public long? UpdatedBy { get; set; }
    public DateTime? UpdatedAt { get; set; }
}