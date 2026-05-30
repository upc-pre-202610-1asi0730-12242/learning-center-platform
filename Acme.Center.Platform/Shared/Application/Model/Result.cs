namespace Acme.Center.Platform.Shared.Application.Model;

/// <summary>
/// Generic Result class for Command Handlers in the Application Layer.
/// </summary>
/// <typeparam name="T">The type of the result value.</typeparam>
public class Result<T>
{
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public T Value { get; }
    public string Message { get; }

    protected Result(bool isSuccess, T value, string message)
    {
        IsSuccess = isSuccess;
        Value = value;
        Message = message;
    }

    public static Result<T> Success(T value) => new(true, value, string.Empty);
    public static Result<T> Failure(string message) => new(false, default!, message);
}

/// <summary>
/// Non-generic Result class for Command Handlers.
/// </summary>
public class Result : Result<object>
{
    private Result(bool isSuccess, string message) : base(isSuccess, null!, message) { }

    public static Result Success() => new(true, string.Empty);
    public static new Result Failure(string message) => new(false, message);
}
