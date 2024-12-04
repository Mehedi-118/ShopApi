using Common.Helpers;

using eShop.Auth.Application.DTOs;
using eShop.Auth.Application.DTOs.Request;
using eShop.Auth.Application.DTOs.Response;
using eShop.Auth.Application.Interfaces;
using eShop.Auth.Application.UserCase.Interfaces;

namespace eShop.Auth.Application.Services;

public class EShopAuthAuthService(IUserUseCase userUseCase, IJwtService jwtService)
    : IEShopAuthService
{
    public async Task<Result<UserRegisterResponseDto>> Register(UserRegisterDto userRegisterDto,
        CancellationToken cancellationToken)
    {
        Result<UserRegisterResponseDto> result =
            await userUseCase.RegisterHandler(userRegisterDto, cancellationToken);
        return !result.IsSuccess ? Result<UserRegisterResponseDto>.Failure(result.Error) : result;
    }

    public async Task<Result<UserLoginResponseDto>> Login(UserLoginDto userLoginDto,
        CancellationToken cancellationToken)
    {
        Result<UserLoginResponseDto> result =
            await userUseCase.LoginHandler(userLoginDto, cancellationToken);
        if (result is not { IsSuccess: true, Data: not null })
        {
            return !result.IsSuccess ? Result<UserLoginResponseDto>.Failure(result.Error) : result;
        }

        UserTokenGeneratorDto userTokenGeneratorDto = new UserTokenGeneratorDto
        {
            UserId = result.Data.UserId,
            Username = result.Data.UserName,
            Email = result.Data.Email,
            PhoneNumber = result.Data.PhoneNumber,
        };
        var userTokenResult = await jwtService.GenerateToken(userTokenGeneratorDto, cancellationToken);
        if (!userTokenResult.IsSuccess)
        {
            return Result<UserLoginResponseDto>.Failure(Error.Failure(description: "Failed to generate token"));
        }

        result.Data.UserToken = userTokenResult.Data;
        return result;
    }

    public async Task<Result<IEnumerable<UserListDto>>> UserList(CancellationToken cancellationToken)
    {
        Result<IEnumerable<UserListDto>> result = await userUseCase.UserList(cancellationToken);
        return !result.IsSuccess ? Result<IEnumerable<UserListDto>>.Failure(result.Error) : result;
    }
}