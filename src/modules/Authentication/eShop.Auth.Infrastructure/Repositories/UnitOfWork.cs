using System.Data;
using System.Data.Common;
using System.Transactions;

using Common.Helpers;

using eShop.Auth.Domain.Entities;
using eShop.Auth.Domain.Interfaces;
using eShop.Auth.Infrastructure.DBContext;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

using IsolationLevel = System.Data.IsolationLevel;

namespace eShop.Auth.Infrastructure.Repositories;

public class UnitOfWork(
    AuthenticationDbContext dbContext,
    AuthenticationReadOnlyDbContext readOnlyDbContext,
    UserManager<User> userManager,
    IEShopAuthRepository eShopAuthRepository)
    : IUnitOfWork
{
    private IEShopAuthRepository _eShopAuthRepository = eShopAuthRepository;


    public IEShopAuthRepository EShopAuthRepository =>
        _eShopAuthRepository ??= new EShopRepository(dbContext, readOnlyDbContext, userManager);


    public async Task<Result<CommitResult>> SaveChangesAsync(CancellationToken cancellationToken,
        DbTransaction? transaction = null)
    {
        try
        {
            var result = await dbContext.SaveChangesAsync(cancellationToken);

            if (result == 0)
            {
                return Result<CommitResult>.Failure(Error.DatabasePersistError());
            }


            return Result<CommitResult>.Success(CommitResult.Success(result));
        }
        catch (Exception ex)
        {
            return Result<CommitResult>.Failure(Error.DatabasePersistError());
        }
    }

    public async Task<DbTransaction> BeginTransaction()
    {
        var transaction = await dbContext.Database.BeginTransactionAsync();

        return transaction.GetDbTransaction();
    }

    public async void Commit()
    {
        await dbContext.Database.CommitTransactionAsync();
    }

    public async void Rollback()
    {
        await dbContext.Database.RollbackTransactionAsync();
    }

    public IDbConnection? Connection { get; }
    public IsolationLevel IsolationLevel { get; }


    public void Dispose()
    {
        dbContext.Dispose();
        readOnlyDbContext.Dispose();
        userManager.Dispose();
        Connection?.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        await dbContext.DisposeAsync();
        await readOnlyDbContext.DisposeAsync();
    }
}