using System.Net;

using Common.Helpers;

using eShop.Auth.Domain.Entities;
using eShop.Auth.Domain.Interfaces;
using eShop.Auth.Infrastructure.DBContext;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace eShop.Auth.Infrastructure.Repositories;

public class EShopRepository(
    AuthenticationDbContext dbContext,
    AuthenticationReadOnlyDbContext readOnlyDbContext,
    UserManager<User> userManager)
    : IEShopAuthRepository
{
    public async Task<Result<User>> Register(User entity, CancellationToken cancellationToken)
    {
        try
        {
            IdentityResult result = await userManager.CreateAsync(entity, entity.PasswordHash!);
            return !result.Succeeded
                ? Result<User>.Failure(Error.DatabaseError(nameof(User),
                    result.Errors.Select(a => a.Description).ToList()))
                : Result<User>.Success(entity, "User registered successfully");
        }
        catch (Exception e)
        {
            return Result<User>.Failure(Error.ExceptionError());
        }
    }

    public async Task<Result<User>> Login(User entity, CancellationToken cancellationToken)
    {
        if (entity.UserName is null)
        {
            return Result<User>.Failure(Error.ValidationError(new Dictionary<string, List<string>>()
            {
                { "Password", ["Invalid UserName or Password"] }
            }));
        }

        var user = await userManager.FindByNameAsync(entity.UserName);
        if (user is null)
        {
            return Result<User>.Failure(Error.ValidationError(new Dictionary<string, List<string>>()
            {
                { "UserName", ["Invalid UserName or Password"] }
            }));
        }

        var result = await userManager.CheckPasswordAsync(user, entity.PasswordHash ?? string.Empty);
        return result
            ? Result<User>.Success(user, "User logged in successfully")
            : Result<User>.Failure(Error.ValidationError(new Dictionary<string, List<string>>()
            {
                { "Password", ["Invalid UserName or Password"] }
            }));
    }

    public async Task<Result<IEnumerable<User>>> UserList(CancellationToken cancellationToken)
    {
        try
        {
            IEnumerable<User> users = await userManager.Users.ToListAsync(cancellationToken);
            return Result<IEnumerable<User>>.Success(users, "User list retrieved successfully");
        }
        catch (Exception e)
        {
            return Result<IEnumerable<User>>.Failure(Error.ExceptionError());
        }
    }
}