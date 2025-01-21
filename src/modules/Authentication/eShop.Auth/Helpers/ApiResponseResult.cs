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
            statusCode: error.ErrorType switch
            {
                ErrorType.NotFound => HttpStatusCode.NotFound,
                ErrorType.ValidationError => HttpStatusCode.BadRequest,
                ErrorType.Unauthorized => HttpStatusCode.Unauthorized,
                ErrorType.Forbidden => HttpStatusCode.Forbidden,
                ErrorType.BadRequest => HttpStatusCode.BadRequest,
                ErrorType.UnprocessableEntity => HttpStatusCode.UnprocessableEntity,
                ErrorType.Conflict => HttpStatusCode.Conflict,
                ErrorType.NoContent => HttpStatusCode.NoContent,
                ErrorType.UnsupportedMediaType => HttpStatusCode.UnsupportedMediaType,
                ErrorType.MethodNotAllowed => HttpStatusCode.MethodNotAllowed,
                ErrorType.NotAcceptable => HttpStatusCode.NotAcceptable,
                ErrorType.RequestTimeout => HttpStatusCode.RequestTimeout,
                ErrorType.LengthRequired => HttpStatusCode.LengthRequired,
                ErrorType.TooManyRequests => HttpStatusCode.TooManyRequests,
                ErrorType.NotImplemented => HttpStatusCode.NotImplemented,
                ErrorType.BadGateway => HttpStatusCode.BadGateway,
                ErrorType.ServiceUnavailable => HttpStatusCode.ServiceUnavailable,
                ErrorType.HttpVersionNotSupported => HttpStatusCode.HttpVersionNotSupported,
                ErrorType.InsufficientStorage => HttpStatusCode.InsufficientStorage,
                _ => HttpStatusCode.InternalServerError
            },
            success: false,
            errors: error.ErrorList ?? new Dictionary<string, List<string>>(),
            data: data
        ));
    }
}