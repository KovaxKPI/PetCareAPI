namespace PetCareAPI.Domain.Results;

public record Result(bool IsSuccess, string? Message = null)
{
    public static Result Success() => new(true);

    public static Result Failure(string message) => new(false, message);

    public static Result Failure(Exception exception) =>
        Failure(exception.Message);
}

public sealed record Result<T> : Result
{
    public T? Value { get; }

    private Result(bool isSuccess, T? value, string? message)
        : base(isSuccess, message)
    {
        Value = value;
    }

    public static Result<T> Success(T value) =>
        new(true, value, null);

    public new static Result<T> Failure(string message) =>
        new(false, default, message);

    public new static Result<T> Failure(Exception exception) =>
        Failure(exception.Message);
}