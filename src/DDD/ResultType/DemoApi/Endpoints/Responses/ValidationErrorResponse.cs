namespace DemoApi.Endpoints.Responses;

public class ValidationErrorResponse : ErrorResponse
{
    public ValidationErrorResponse(Uri documentation)
        : base(documentation, "Validation failed", 400, "One or more validation errors occurred.") { }

    public void AddFieldValidationError(string fieldName, string message) =>
        AddError(ToLowerFirstLetter(fieldName), message);

    private static string ToLowerFirstLetter(string fieldName) =>
        fieldName switch
        {
            [] => string.Empty,
            [char single] => char.ToLowerInvariant(single).ToString(),
            [char first, .. var rest] => char.ToLowerInvariant(first) + rest
        };
}