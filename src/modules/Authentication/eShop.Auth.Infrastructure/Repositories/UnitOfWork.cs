using Common.Helpers;

using eShop.Auth.Domain.Entities;
using eShop.Auth.Domain.Interfaces;
using eShop.Auth.Infrastructure.DBContext;

using Microsoft.AspNetCore.Identity;

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


    public async Task<Result<CommitResult>> SaveChangesAsync(CancellationToken cancellationToken)
    {
        try
        {
            return Result<CommitResult>.Success(
                CommitResult.Success(await dbContext.SaveChangesAsync(cancellationToken)));
        }
        catch (Exception ex)
        {
            return Result<CommitResult>.Failure(Error.ExceptionError());
        }
    }

    public async ValueTask DisposeAsync()
    {
        await dbContext.DisposeAsync();
    }
}