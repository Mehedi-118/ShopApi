using System.Net;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.JavaScript;

using Common.Helpers;

using Microsoft.AspNetCore.Http.HttpResults;

namespace eShop.Auth.Helpers;

public class ApiResponseResult<T> where T : class
{
    public static Ok<ApiResponse<T>> Success(T? data, string message = "Success")
    {
        return TypedResults.Ok(new ApiResponse<T>(
            data: data,
            message: message,
            statusCode: HttpStatusCode.OK,
            success: true,
            errors: new Dictionary<string, List<string>>()
        ));
    }

    public static Ok<ApiResponse<T>> Fail(string message = "Failed")
    {
        return TypedResults.Ok(new ApiResponse<T>(message: message, statusCode: HttpStatusCode.InternalServerError,
            success: false, errors: new Dictionary<string, List<string>>()
        ));
    }

    public static JsonHttpResult<ApiResponse<T>> Problem<T>(Error error, T? data = default)
    {
        return TypedResults.Json(new ApiResponse<T>(
            message: error.Description,
            statusCode: error.StatusCode,
            success: false,
            errors: error.ErrorList ?? new Dictionary<string, List<string>>(),
            data: data
        ));
    }
}