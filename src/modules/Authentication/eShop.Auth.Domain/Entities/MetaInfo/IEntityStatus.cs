namespace eShop.Auth.Domain.Entities.MetaInfo;

public interface IEntityStatus
{
    public bool IsActive { get; set; }
    public bool IsDeleted { get; set; }
}