namespace eShop.Auth.Domain.Entities.MetaInfo;

public interface ICreationMetadata
{
    public long CreatedBy { get; set; }
    public DateTime CreatedAt { get; init; }
}