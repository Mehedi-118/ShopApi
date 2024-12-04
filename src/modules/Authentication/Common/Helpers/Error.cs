using System.Net;

namespace Common.Helpers;

public sealed class Error
{
    public string Tittle { get; set; }
    public string Description { get; set; }
    public ErrorType ErrorType { get; set; }
    public HttpStatusCode StatusCode { get; set; }
    public Dictionary<string, List<string>>? ErrorList { get; private init; }

    private Error(string tittle, string description, ErrorType errorType,
        HttpStatusCode statusCode = HttpStatusCode.InternalServerError,
        Dictionary<string, List<string>>? errorsList = default)
    {
        Tittle = tittle;
        Description = description;
        ErrorType = errorType;
        StatusCode = statusCode;
        ErrorList = errorsList;
    }

    public static Error Failure(string title = "Failed", string description = "Something went wrong",
        ErrorType errorType = ErrorType.InternalServerError)
    {
        return new Error(title, description, errorType);
    }

    public static Error NotFound(string title = "Not Found", string description = "Not found",
        ErrorType errorType = ErrorType.NotFound)
    {
        return new Error(title, description, errorType);
    }

    public static Error ValidationError(Dictionary<string, List<string>> errors)
    {
        return new Error("Validation", "Validation Error Occured", ErrorType.ValidationError, HttpStatusCode.BadRequest,
            errors);
    }

    public static Error DatabaseError(string entityName, List<string>? errors, string description = "Database Error")
    {
        return new Error(entityName, description, ErrorType.DbError, HttpStatusCode.InternalServerError,
            new Dictionary<string, List<string>> { { entityName, errors ?? new List<string> { description } } });
    }

    public static Error DatabasePersistError(string description = "Error occured while saving data into database")
    {
        return new Error(tittle: "Database", description: description, ErrorType.DbError);
    }

    public static Error ExceptionError(string description = "Something went wrong")
    {
        return new Error(tittle: "Exception", description: description, ErrorType.Exception);
    }
}