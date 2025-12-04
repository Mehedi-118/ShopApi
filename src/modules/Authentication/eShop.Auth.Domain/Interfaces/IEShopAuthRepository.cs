using Common.Helpers;

using eShop.Auth.Domain.Entities;

using Microsoft.AspNetCore.Identity;

namespace eShop.Auth.Domain.Interfaces;

public interface IEShopAuthRepository
{
    Task<Result<User>> Register(User entity, CancellationToken cancellationToken);
    Task<Result<Role>> CreateRole(Role entity, CancellationToken cancellationToken);
    Task<Result<User>> Login(User entity, CancellationToken cancellationToken);
    Task<Result<IEnumerable<User>>> UserList(CancellationToken cancellationToken);
    Task<Result<bool>> StoreRefreashToken(RefreashToken entity, CancellationToken cancellationToken);
    Task<Result<bool>> RevokeToken(long userId, CancellationToken cancellationToken);
    Task<Result<User>> GetUserByRefreashToken(string refreshToken, CancellationToken cancellationToken);
    Task<Result<User>> GetUserInfo(long id, CancellationToken cancellationToken);
    Task<Result<User>> GetUserByToken(string token, CancellationToken cancellationToken);
}