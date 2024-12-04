using Common.Helpers;

namespace eShop.Auth.Domain.Interfaces;

public interface IUnitOfWork : IAsyncDisposable
{
    IEShopAuthRepository EShopAuthRepository { get; }
    
    Task<Result<CommitResult>> SaveChangesAsync(CancellationToken cancellationToken);
}