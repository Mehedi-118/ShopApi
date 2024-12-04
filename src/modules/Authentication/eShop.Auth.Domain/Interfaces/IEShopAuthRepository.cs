using Common.Helpers;

using eShop.Auth.Domain.Entities;

using Microsoft.AspNetCore.Identity;

namespace eShop.Auth.Domain.Interfaces;

public interface IEShopAuthRepository
{
    Task<Result<User>> Register(User entity, CancellationToken cancellationToken);
    Task<Result<User>> Login(User entity, CancellationToken cancellationToken);
    Task<Result<IEnumerable<User>>> UserList(CancellationToken cancellationToken);
}