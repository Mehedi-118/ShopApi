namespace Common.Helpers;

public class CommitResult
{
    public bool IsSuccess { get; }
    public string? ErrorMessage { get; }
    public int AffectedRows { get; }

    private CommitResult(bool isSuccess, string? errorMessage, int affectedRows)
    {
        IsSuccess = isSuccess;
        ErrorMessage = errorMessage;
        AffectedRows = affectedRows;
    }

    public static CommitResult Success(int affectedRows) => new CommitResult(true, null, affectedRows);
    public static CommitResult Failure(string errorMessage) => new CommitResult(false, errorMessage, 0);
}