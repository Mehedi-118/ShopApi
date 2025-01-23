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
        // Login Operation
        Result<UserLoginResponseDto> result =
            await userUseCase.LoginHandler(userLoginDto, cancellationToken);
        if (result is not { IsSuccess: true, Data: not null })
        {
            return !result.IsSuccess ? Result<UserLoginResponseDto>.Failure(result.Error) : result;
        }

        // Token Generation
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
        // Insert Refreash Token Generation in DB

        var refreashTokenObj = new RefreashTokenDto
        {
            RefreshToken = userTokenResult.Data.RefreashToken,
            UserId = userTokenGeneratorDto.UserId,
            ExpiresOnUtc = userTokenResult.Data.RefreashTokenExpiresOn,
            IsRevoked = false,
            CreatedBy = userTokenGeneratorDto.UserId,
            CreatedAt = DateTime.UtcNow
        };

        var storeRefreashToken =
            await userUseCase.StoreRefreashToken(refreashTokenObj, cancellationToken: cancellationToken);

        if (!storeRefreashToken.IsSuccess)
        {
            return Result<UserLoginResponseDto>.Failure(
                Error.Failure(description: "Failed to store refreash token in db"));
        }

        result.Data.UserToken = userTokenResult.Data;
        return result;
    }

    public async Task<Result<UserTokenResponse>> RefreashToken(UserTokenRequest userTokenRequestDto,
        CancellationToken cancellationToken)
    {
        // Get User By Refreash Token upon validating the refreash token
        var validateRefreashToken = await userUseCase.ValidateRefreashToken(userTokenRequestDto.RefreashToken,
            cancellationToken);

        if (validateRefreashToken is not { IsSuccess: true, Data: not null })
        {
            return Result<UserTokenResponse>.Failure(validateRefreashToken.Error);
        }

        var userTokenGeneratorDto = new UserTokenGeneratorDto
        {
            UserId = validateRefreashToken?.Data?.UserId ?? 0,
            Username = validateRefreashToken?.Data?.UserName ?? string.Empty,
            Email = validateRefreashToken?.Data?.Email ?? string.Empty,
        };
        var newTokenResult = await jwtService.GenerateToken(userTokenGeneratorDto, cancellationToken);

        var refreashTokenObj = new RefreashTokenDto
        {
            RefreshToken = newTokenResult.Data.RefreashToken,
            PreviousRefreshToken = userTokenRequestDto.RefreashToken,
            UserId = validateRefreashToken.Data.UserId,
            ExpiresOnUtc = newTokenResult.Data.RefreashTokenExpiresOn,
            IsRevoked = false,
            CreatedBy = validateRefreashToken.Data.UserId,
            CreatedAt = DateTime.UtcNow
        };

        var storeRefreashToken =
            await userUseCase.StoreRefreashToken(refreashTokenObj, cancellationToken: cancellationToken);

        if (!storeRefreashToken.IsSuccess)
        {
            return Result<UserTokenResponse>.Failure(
                Error.Failure(description: "Failed to generate refreash token"));
        }


        return !newTokenResult.IsSuccess
            ? Result<UserTokenResponse>.Failure(Error.Failure(description: "Failed to generate token"))
            : newTokenResult;
    }

    public async Task<Result<IEnumerable<UserListDto>>> UserList(CancellationToken cancellationToken)
    {
        Result<IEnumerable<UserListDto>> result = await userUseCase.UserList(cancellationToken);
        return !result.IsSuccess ? Result<IEnumerable<UserListDto>>.Failure(result.Error) : result;
    }

    public async Task<Result<bool>> RevokeToken(long userId, CancellationToken cancellationToken)
    {      
        var result = await userUseCase.RevokeToken(userId, cancellationToken);
        if (!result.IsSuccess)
        {

            return result;
        }
        return result;

    }
}