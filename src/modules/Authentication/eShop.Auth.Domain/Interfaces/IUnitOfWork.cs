using System.Data;
using System.Data.Common;
using System.Transactions;

using Common.Helpers;

namespace eShop.Auth.Domain.Interfaces;

public interface IUnitOfWork : IAsyncDisposable, IDbTransaction
{
    IEShopAuthRepository EShopAuthRepository { get; }
    Task<DbTransaction> BeginTransaction();
    Task<Result<CommitResult>> SaveChangesAsync(CancellationToken cancellationToken, DbTransaction? transaction = null);
}