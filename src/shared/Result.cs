namespace SimpleMDB;

public class Result<T>
{
    public bool IsValid { get; }
    public T? Value { get; }
    public Exception? Error { get; }

    public Result(T value)
    {
        IsValid = true;
        Value = value;
        Error = null;
    }

    public Result(Exception error)
    {
        IsValid = false;
        Error = error;
        Value = default;
    }

    public static Result<T> Ok(T value) => new Result<T>(value);

    public static Result<T> Fail(Exception error) => new Result<T>(error);
}
