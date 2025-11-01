namespace PatternsAndStuff;

public readonly record struct Result<T>
{
    public bool IsSuccess { get; }
    public T? Value { get; }
    public string? Error { get; }

    private Result(T value)
    {
        IsSuccess = true;
        Value = value;
        Error = null;
    }

    private Result(string error)
    {
        IsSuccess = false;
        Value = default;
        Error = error;
    }

    public static Result<T> Success(T value) => new(value);
    public static Result<T> Failure(string error) => new(error);
}

public sealed record EmailSmartConstructor
{
    public string Value { get; }

    private EmailSmartConstructor(string value) => Value = value;

    public static Result<EmailSmartConstructor> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return Result<EmailSmartConstructor>.Failure("Email cannot be empty.");

        if (!IsValidEmail(value))
            return Result<EmailSmartConstructor>.Failure("Invalid email format.");

        return Result<EmailSmartConstructor>.Success(new EmailSmartConstructor(value));
    }

    private static bool IsValidEmail(string email)
    {
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }
}

public sealed record EmailTryCreate
{
    public string Value { get; }

    private EmailTryCreate(string value) => Value = value;

    public static bool TryCreate(string value, out EmailTryCreate? email)
    {
        email = null;

        if (string.IsNullOrWhiteSpace(value))
            return false;

        if (!IsValidEmail(value))
            return false;

        email = new EmailTryCreate(value);
        return true;
    }

    private static bool IsValidEmail(string email)
    {
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }
}

