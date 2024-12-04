using System.Net;

namespace Common.Helpers;

public class Result<T>
{
    public T? Data { get; } = default;
    public bool IsSuccess { get; }
    public string Message { get; } = string.Empty;
    public readonly Error Error;
    public HttpStatusCode StatusCode { get; set; }

    private Result(T data, bool isSuccess, string message)
    {
        Data = data;
        IsSuccess = isSuccess;
        Message = message;
    }

    private Result(Error error)
    {
        Error = error;
        IsSuccess = false;
        Message = "Error";
    }

    private Result(T? data, Error error, string message = "Error")
    {
        Data = data;
        Error = error;
        IsSuccess = false;
    }

    public static Result<T> Success(T? data = default, string message = "Success")
    {
        return new Result<T>(data: data, isSuccess: true, message: message);
    }

    public static Result<T> Failure(Error error, string message = "Failure", T? data = default)
    {
        return data is not null
            ? new Result<T>(data: data, error: error, message: message)
            : new Result<T>(error);
    }
}