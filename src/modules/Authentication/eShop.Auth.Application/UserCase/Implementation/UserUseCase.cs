using Common.Helpers;

using eShop.Auth.Application.DTOs;
using eShop.Auth.Application.DTOs.Request;
using eShop.Auth.Application.DTOs.Response;
using eShop.Auth.Application.Helpers;
using eShop.Auth.Application.UserCase.Interfaces;
using eShop.Auth.Domain.Entities;
using eShop.Auth.Domain.Interfaces;

using Microsoft.Extensions.Options;

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

        Result<CommitResult> commitResult = await unitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        return commitResult.IsSuccess
            ? Result<UserRegisterResponseDto>.Success(userRegisterResponseDto, result.Message)
            : Result<UserRegisterResponseDto>.Failure(result.Error);
    }

    public async Task<Result<UserLoginResponseDto>> LoginHandler(UserLoginDto userLoginDto,
        CancellationToken cancellationToken)
    {
        User user = User.CreateBuilder()
            .WithLogInCredential(userLoginDto.UserName.Trim(), userLoginDto.Password)
            .Build();
        if (user.ValidationErrors?.Count > 0)
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
}