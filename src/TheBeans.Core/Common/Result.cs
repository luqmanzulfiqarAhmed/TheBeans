namespace TheBeans.Core.Common;

public class Result<T>
{
    public bool IsSuccess { get; }
    public T? Value { get; }
    public IEnumerable<string> Errors { get; }

    private Result(T? value, bool isSuccess, IEnumerable<string> errors)
    {
        Value = value;
        IsSuccess = isSuccess;
        Errors = errors;
    }

    public static Result<T> Success(T value)
    {
        return new Result<T>(value, true, Array.Empty<string>());
    }

    public static Result<T> Failure(IEnumerable<string> errors)
    {
        return new Result<T>(default, false, errors);
    }

    public static Result<T> Failure(string error)
    {
        return new Result<T>(default, false, new[] { error });
    }
}