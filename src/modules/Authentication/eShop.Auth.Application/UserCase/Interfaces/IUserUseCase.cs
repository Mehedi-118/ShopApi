using Common.Helpers;

using eShop.Auth.Application.DTOs;
using eShop.Auth.Application.DTOs.Request;
using eShop.Auth.Application.DTOs.Response;

namespace eShop.Auth.Application.UserCase.Interfaces;

public interface IUserUseCase
{
    Task<Result<UserRegisterResponseDto>> RegisterHandler(UserRegisterDto userRegisterDto,
        CancellationToken cancellationToken);

    Task<Result<UserLoginResponseDto>> LoginHandler(UserLoginDto userLoginDto,
        CancellationToken cancellationToken);

    Task<Result<IEnumerable<UserListDto>>> UserList(CancellationToken cancellationToken);
}