using Common.Helpers;

using eShop.Auth.Domain.Entities;
using eShop.Auth.Domain.Interfaces;
using eShop.Auth.Infrastructure.DBContext;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

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

        var isPasswordMatched = await userManager.CheckPasswordAsync(user, entity.PasswordHash ?? string.Empty);

        if (isPasswordMatched)
        {
            user.LastLogin = DateTime.UtcNow;
            var updatededUser = await userManager.UpdateAsync(user);
            if (!updatededUser.Succeeded)
            {
                return Result<User>.Failure(Error.ValidationError(new Dictionary<string, List<string>>()
                {
                    { "User Last Login", ["Failed to Update User's Last Login "] }
                }));
            }
            return Result<User>.Success(user, "User logged in successfully");

        }
        return Result<User>.Failure(Error.ValidationError(new Dictionary<string, List<string>>()
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

    public async Task<Result<bool>> StoreRefreashToken(RefreashToken entity, CancellationToken cancellationToken)
    {
        var existingRefreashTokenObj = await readOnlyDbContext.RefreashTokens.FirstOrDefaultAsync(
                a => a.Token == entity.PreviousToken && !a.IsRevoked, cancellationToken)
            .ConfigureAwait(false);
        if (existingRefreashTokenObj is not null)
        {
            existingRefreashTokenObj.IsRevoked = true;
            existingRefreashTokenObj.UpdatedBy = entity.CreatedBy;
            existingRefreashTokenObj.UpdatedAt = DateTime.UtcNow;


            EntityEntry<RefreashToken>? updateResult = dbContext.RefreashTokens.Update(existingRefreashTokenObj);

            if (updateResult.State != EntityState.Modified)
            {
                return Result<bool>.Failure(Error.DatabaseError(nameof(RefreashToken),
                    ["Failed to update refreash token in Dbcontext"]));
            }
        }


        EntityEntry<RefreashToken>? result = await dbContext.RefreashTokens.AddAsync(entity, cancellationToken);
        return (result is null)
            ? Result<bool>.Failure(Error.DatabaseError(nameof(RefreashToken),
                ["Failed to add refreash token in Dbcontext"]))
            : Result<bool>.Success(true, "Refreash token added into context successfully");
    }
    public async Task<Result<bool>> RevokeToken(long userId, CancellationToken cancellationToken)
    {
        var refreashTokens = readOnlyDbContext.RefreashTokens.Where(x => x.UserId == userId && !x.IsRevoked).AsEnumerable();
        if (!refreashTokens.Any())
        {
            return Result<bool>.Success(false, "No access token found with this user");

        }
        foreach (var refreashToken in refreashTokens)
        {
            refreashToken.IsRevoked = true;
            refreashToken.UpdatedAt = DateTime.UtcNow;
        }

        dbContext.RefreashTokens.UpdateRange(refreashTokens);

        return Result<bool>.Success(true, "Refreash tokens revoked");
    }

    public async Task<Result<User>> GetUserByRefreashToken(string refreshToken, CancellationToken cancellationToken)
    {
        User? user = await readOnlyDbContext.RefreashTokens
            .Where(a => a.Token == refreshToken && a.ExpiresOnUtc > DateTime.UtcNow)
            .Include(a => a.User)
            .Select(a => a.User).FirstOrDefaultAsync(cancellationToken: cancellationToken);

        if (user is null or { Id: 0 })
        {
            return Result<User>.Failure(Error.ValidationError(new Dictionary<string, List<string>>()
            {
                { "RefreshToken", ["Invalid Refresh Token"] }
            }));
        }

        return Result<User>.Success(user, "User retrieved successfully by refreash token");
    }
}