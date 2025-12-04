using Common.Helpers;

using eShop.Auth.Application.DTOs;
using eShop.Auth.Application.DTOs.Request;
using eShop.Auth.Application.DTOs.Response;

namespace eShop.Auth.Application.Interfaces;

public interface IEShopAuthService
{
    Task<Result<UserRegisterResponseDto>> Register(UserRegisterDto userRegisterDto, CancellationToken cancellationToken);

    Task<Result<UserLoginResponseDto>> Login(UserLoginDto userLoginDto, CancellationToken cancellationToken);

    Task<Result<UserTokenResponse>> RefreashToken(UserTokenRequest userLoginDto, CancellationToken cancellationToken);

    Task<Result<UserRoleDto>> CreateRole(UserRoleDto userRoleDto, CancellationToken cancellationToken);
    Task<Result<UserResponse>> GetUserInfo(long id, CancellationToken cancellationToken);
    Task<Result<UserResponse>> GetUserByToken(string token, CancellationToken cancellationToken);

    Task<Result<IEnumerable<UserListDto>>> UserList(CancellationToken cancellationToken);
    Task<Result<bool>> RevokeToken(long userId, CancellationToken cancellationToken);
}