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
}