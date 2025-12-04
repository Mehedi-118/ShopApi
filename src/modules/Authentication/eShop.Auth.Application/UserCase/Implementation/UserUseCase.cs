using Common.Helpers;

using eShop.Auth.Application.DTOs.Request;
using eShop.Auth.Application.DTOs.Response;
using eShop.Auth.Application.UserCase.Interfaces;
using eShop.Auth.Domain.Entities;
using eShop.Auth.Domain.Interfaces;

namespace eShop.Auth.Application.UserCase.Implementation;

public class UserUseCase(IUnitOfWork unitOfWork)
    : IUserUseCase
{
    public async Task<Result<UserRegisterResponseDto>> RegisterHandler(UserRegisterDto userRegisterDto,
        CancellationToken cancellationToken)
    {
        if (userRegisterDto.Password != userRegisterDto.ConfirmPassword)
        {
            return Result<UserRegisterResponseDto>.Failure(Error.ValidationError(
                new Dictionary<string, List<string>>()
                {
                    { "Password", ["Password and Confirm Password do not match"] }
                }));
        }

        User user = User.CreateBuilder()
            .WithUserName(userRegisterDto.UserName)
            .WithEmail(userRegisterDto.Email)
            .WithPhoneNumber(userRegisterDto.PhoneNumber)
            .WithPasswordHash(userRegisterDto.Password)
            .Build();
        if (user.ValidationErrors.Count > 0)
        {
            return Result<UserRegisterResponseDto>.Failure(Error.ValidationError(user.ValidationErrors));
        }


        Result<User> result = await unitOfWork.EShopAuthRepository.Register(user, cancellationToken);
        var userRegisterResponseDto =
            new UserRegisterResponseDto { UserName = userRegisterDto.UserName, Email = userRegisterDto.Email };

        if (!result.IsSuccess)
        {
            return Result<UserRegisterResponseDto>.Failure(result.Error, result.Message);
        }


        return result.IsSuccess
            ? Result<UserRegisterResponseDto>.Success(userRegisterResponseDto, result.Message)
            : Result<UserRegisterResponseDto>.Failure(result.Error);
    }
    public async Task<Result<UserRoleDto>> CreateRoleHandler(UserRoleDto userRoleDto, CancellationToken cancellationToken)
    {


        var role = new Role
        {
            Name = userRoleDto.Name
        };

        Result<Role> result = await unitOfWork.EShopAuthRepository.CreateRole(role, cancellationToken);

        if (!result.IsSuccess)
        {
            return Result<UserRoleDto>.Failure(result.Error, result.Message);
        }
        return result.IsSuccess
            ? Result<UserRoleDto>.Success(userRoleDto, result.Message)
            : Result<UserRoleDto>.Failure(result.Error);
    }

    public async Task<Result<UserLoginResponseDto>> LoginHandler(UserLoginDto userLoginDto,
        CancellationToken cancellationToken)
    {
        User user = User.CreateBuilder()
            .WithLogInCredential(userLoginDto.UserName.Trim(), userLoginDto.Password)
            .Build();
        if (user.ValidationErrors.Count > 0)
        {
            return Result<UserLoginResponseDto>.Failure(Error.ValidationError(user.ValidationErrors));
        }

        Result<User> loginResult = await unitOfWork.EShopAuthRepository.Login(user, cancellationToken);
        if (!loginResult.IsSuccess)
        {
            return Result<UserLoginResponseDto>.Failure(loginResult.Error, loginResult.Message);
        }

        if (loginResult.Data is null)
        {
            return Result<UserLoginResponseDto>.Failure(loginResult.Error, loginResult.Message);
        }

        var userLoginResponseDto = new UserLoginResponseDto
        {
            UserId = loginResult.Data.Id,
            UserName = loginResult.Data.UserName ?? string.Empty,
            Email = loginResult.Data.Email ?? string.Empty,
            PhoneNumber = loginResult.Data.PhoneNumber ?? string.Empty,
        };
        return Result<UserLoginResponseDto>.Success(userLoginResponseDto, "User logged in successfully");
    }

    public async Task<Result<IEnumerable<UserListDto>>> UserList(CancellationToken cancellationToken)
    {
        Result<IEnumerable<User>> result = await unitOfWork.EShopAuthRepository.UserList(cancellationToken);
        if (!result.IsSuccess)
        {
            return Result<IEnumerable<UserListDto>>.Failure(result.Error, result.Message);
        }

        var userListDto = new List<UserListDto>();
        if (result.Data is not null || result.Data?.Count() > 0)
        {
            userListDto.AddRange(result.Data.Select(user => new UserListDto
            {
                Username = user.UserName ?? string.Empty,
                Email = user.Email ?? string.Empty,
                Phonenumber = user.PhoneNumber ?? string.Empty,
            }));
        }

        return Result<IEnumerable<UserListDto>>.Success(userListDto);
    }

    public async Task<Result<bool>> StoreRefreashToken(RefreashTokenDto requestDto, CancellationToken cancellationToken)
    {
        var entity = RefreashToken.CreateBuilder()
            .WithRefreshToken(requestDto.RefreshToken)
            .WithUserId(requestDto.UserId)
            .WithExpiresOnUtc(requestDto.ExpiresOnUtc)
            .WithIsRevoked(requestDto.IsRevoked)
            .WithCreatedBy(requestDto.CreatedBy)
            .WithCreatedAt(requestDto.CreatedAt)
            .WithPreviousRefreashToken(requestDto.PreviousRefreshToken)
            .Build();


        Result<bool> result = await unitOfWork.EShopAuthRepository.StoreRefreashToken(entity, cancellationToken);
        if (!result.IsSuccess)
        {
            return Result<bool>.Failure(result.Error, result.Message);
        }

        var commitResult = await unitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        return commitResult.IsSuccess is not true
            ? Result<bool>.Failure(commitResult.Error, commitResult.Message)
            : Result<bool>.Success(true, "Refreash Token stored successfully");
    }

    public async Task<Result<UserLoginResponseDto>> ValidateRefreashToken(string refreshToken,
        CancellationToken cancellationToken)
    {
        var getUserByRefreashToken =
            await unitOfWork.EShopAuthRepository.GetUserByRefreashToken(refreshToken, cancellationToken);
        if (getUserByRefreashToken is not { IsSuccess: true, Data: not null })
        {
            return Result<UserLoginResponseDto>.Failure(getUserByRefreashToken.Error, getUserByRefreashToken.Message);
        }

        return Result<UserLoginResponseDto>.Success(
            new UserLoginResponseDto
            {
                UserId = getUserByRefreashToken.Data?.Id ?? 0,
                UserName = getUserByRefreashToken.Data?.UserName ?? string.Empty,
                Email = getUserByRefreashToken.Data?.Email ?? string.Empty,
            }, "User Retrieved Successfully");
    }
    public async Task<Result<bool>> RevokeToken(long userId,
        CancellationToken cancellationToken)
    {
        if (userId == 0)
        {
            return Result<bool>.Failure(Error.ValidationError(new Dictionary<string, List<string>> { { "user Id", new List<string> { "User Id Cannot be 0" } } }));

        }
        var revokeTokenStatus = await unitOfWork.EShopAuthRepository.RevokeToken(userId, cancellationToken);
        if (revokeTokenStatus is not { IsSuccess: true })
        {
            return Result<bool>.Failure(revokeTokenStatus.Error, revokeTokenStatus.Message);
        }

        var commitResult = await unitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        return commitResult.IsSuccess is not true
            ? Result<bool>.Failure(commitResult.Error, commitResult.Message)
            : Result<bool>.Success(true, "Refreash Token revokation successful");
    }

    public async Task<Result<UserResponse>> GetUserInfo(long id, CancellationToken cancellationToken)
    {
        var getUserById = await unitOfWork.EShopAuthRepository.GetUserInfo(id, cancellationToken);
        if (getUserById is not { IsSuccess: true, Data: not null })
        {
            return Result<UserResponse>.Failure(getUserById.Error, getUserById.Message);
        }

        return Result<UserResponse>.Success(
            new UserResponse
            {
                UserId = getUserById.Data?.Id ?? 0,
                UserName = getUserById.Data?.UserName ?? string.Empty,
                Email = getUserById.Data?.Email ?? string.Empty,
            }, "User Retrieved Successfully");
    }
    public async Task<Result<UserResponse>> GetUserByToken(string token, CancellationToken cancellationToken)
    {
        var getUserByToken = await unitOfWork.EShopAuthRepository.GetUserByToken(token, cancellationToken);
        if (getUserByToken is not { IsSuccess: true, Data: not null })
        {
            return Result<UserResponse>.Failure(getUserByToken.Error, getUserByToken.Message);
        }

        return Result<UserResponse>.Success(
            new UserResponse
            {
                UserId = getUserByToken.Data?.Id ?? 0,
                UserName = getUserByToken.Data?.UserName ?? string.Empty,
                Email = getUserByToken.Data?.Email ?? string.Empty,
            }, "User Retrieved Successfully");
    }
}