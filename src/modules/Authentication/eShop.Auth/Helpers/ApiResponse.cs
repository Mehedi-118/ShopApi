using System.Net;

namespace eShop.Auth.Helpers;

public class ApiResponse<T>
{
    public ApiResponse(bool success, HttpStatusCode statusCode, string message, Dictionary<string, List<string>> errors)
    {
        Success = success;
        StatusCode = statusCode;
        Message = message;
        Errors = errors;
    }

    public ApiResponse(T? data, bool success, HttpStatusCode statusCode, string message,
        Dictionary<string, List<string>> errors)
    {
        Data = data;
        Success = success;
        StatusCode = statusCode;
        Message = message;
        Errors = errors;
    }

    public T? Data { get; set; }
    public bool Success { get; set; }
    public string Message { get; set; }
    public IDictionary<string, List<string>> Errors { get; init; }
    public HttpStatusCode StatusCode { get; set; }
}