using System.Net.Http.Headers;

using Common.Helpers;

using eShop.Auth.Application.DTOs.Request;
using eShop.Auth.Application.DTOs.Response;
using eShop.Auth.Application.Interfaces;
using eShop.Auth.Helpers;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

using Serilog;

namespace eShop.Auth.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class AuthController(IEShopAuthService eShopAuthService) : ControllerBase
{
    [HttpPost]
    [AllowAnonymous]
    [Route("register")]
    public async
        Task<Results<Ok<ApiResponse<UserRegisterResponseDto>>, JsonHttpResult<ApiResponse<UserRegisterResponseDto>>>>
        Register([FromBody] UserRegisterDto userRegisterDto, CancellationToken cancellationToken)
    {
        Result<UserRegisterResponseDto> response = await eShopAuthService.Register(userRegisterDto, cancellationToken);
        return response.IsSuccess
            ? ApiResponseResult<UserRegisterResponseDto>.Success(response.Data, response.Message)
            : ApiResponseResult<UserRegisterResponseDto>.Problem<UserRegisterResponseDto>(response.Error);
    }

    [HttpPost]
    [AllowAnonymous]
    [Route("login")]
    public async
        Task<Results<Ok<ApiResponse<UserLoginResponseDto>>, JsonHttpResult<ApiResponse<UserLoginResponseDto>>>>
        Login([FromBody] UserLoginDto userLoginDto, CancellationToken cancellationToken)
    {
        Result<UserLoginResponseDto> response = await eShopAuthService.Login(userLoginDto, cancellationToken);
        return response.IsSuccess
            ? ApiResponseResult<UserLoginResponseDto>.Success(response.Data, response.Message)
            : ApiResponseResult<UserLoginResponseDto>.Problem<UserLoginResponseDto>(response.Error);
    }

    [HttpPost]
    [AllowAnonymous]
    [Route("refresh-token")]
    public async
        Task<Results<Ok<ApiResponse<UserTokenResponse>>, JsonHttpResult<ApiResponse<UserTokenResponse>>>>
        RefreashToken([FromBody] UserTokenRequest userTokenResponse, CancellationToken cancellationToken)
    {
        Result<UserTokenResponse> response = await eShopAuthService.RefreashToken(userTokenResponse, cancellationToken);
        return response.IsSuccess
            ? ApiResponseResult<UserTokenResponse>.Success(response.Data, response.Message)
            : ApiResponseResult<UserTokenResponse>.Problem<UserTokenResponse>(response.Error);
    }

    [HttpGet]
    [Route("user-list")]
    public async Task<Results<Ok<ApiResponse<List<UserListDto>>>,
        JsonHttpResult<ApiResponse<List<UserListDto>>>>> UserList(
        CancellationToken cancellationToken)
    {
        Result<IEnumerable<UserListDto>> response = await eShopAuthService.UserList(cancellationToken);
        return response.IsSuccess
            ? ApiResponseResult<List<UserListDto>>.Success(response.Data?.ToList(), response.Message)
            : ApiResponseResult<List<UserListDto>>.Problem<List<UserListDto>>(response.Error);
    }

    [HttpPost]
    [Route("revoke-token")]

    public async Task<Results<Ok<ApiResponse<EmptyResponse>>, JsonHttpResult<ApiResponse<EmptyResponse>>>> RevokeToken(long userId, CancellationToken cancellationToken)
    {
        var response = await eShopAuthService.RevokeToken(userId, cancellationToken);
        if (!response.IsSuccess)
        {
            return ApiResponseResult<Result<EmptyResponse>>.Problem<EmptyResponse>(response.Error);
        }
        Ok<ApiResponse<EmptyResponse>> apiResponse = ApiResponseResult<EmptyResponse>.Success(message: "Token revoked successfully.");
        return apiResponse;
    }
    [HttpPost]
    [Route("role")]

    public async Task<Results<Ok<ApiResponse<UserRoleDto>>, JsonHttpResult<ApiResponse<UserRoleDto>>>> CreateRole([FromBody] UserRoleDto userRoleDto, CancellationToken cancellationToken)
    {
        var response = await eShopAuthService.CreateRole(userRoleDto, cancellationToken);
        if (!response.IsSuccess)
        {
            return ApiResponseResult<Result<UserRoleDto>>.Problem<UserRoleDto>(response.Error);
        }
        Ok<ApiResponse<UserRoleDto>> apiResponse = ApiResponseResult<UserRoleDto>.Success(response.Data, response.Message);
        return apiResponse;
    }
    [HttpPost]
    [AllowAnonymous]
    [Route("user/{id}")]
    public async
        Task<Results<Ok<ApiResponse<UserResponse>>, JsonHttpResult<ApiResponse<UserResponse>>>>
        UserById(long id, CancellationToken cancellationToken)
    {
        Result<UserResponse> response = await eShopAuthService.GetUserInfo(id, cancellationToken);
        return response.IsSuccess
            ? ApiResponseResult<UserResponse>.Success(response.Data, response.Message)
            : ApiResponseResult<UserResponse>.Problem<UserResponse>(response.Error);
    }
    [HttpPost]

    [HttpGet]
    [Route("user-details")]
    public async
        Task<Results<Ok<ApiResponse<UserResponse>>, JsonHttpResult<ApiResponse<UserResponse>>>>UserDetails(CancellationToken cancellationToken)
    {
        string token = string.Empty;
        if (Request.Headers.TryGetValue("Authorization", out var authHeader))
        {
            var authHeaderValue = authHeader.FirstOrDefault();
            if (!string.IsNullOrEmpty(authHeaderValue) && authHeaderValue.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
            {
                // Extract the token by removing the "Bearer " prefix
                token = authHeaderValue.Substring("Bearer ".Length).Trim();
            }
        }
        Result<UserResponse> response = await eShopAuthService.GetUserByToken(token, cancellationToken);
        return response.IsSuccess
            ? ApiResponseResult<UserResponse>.Success(response.Data, response.Message)
            : ApiResponseResult<UserResponse>.Problem<UserResponse>(response.Error);
    }

}