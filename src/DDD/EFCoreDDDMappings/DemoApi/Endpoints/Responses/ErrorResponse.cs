namespace DemoApi.Endpoints.Responses;

public class ErrorResponse
{
    public string Type { get; set; }
    public string Title { get; set; }
    public int Status { get; set; }
    public string Detail { get; set; }
    public Dictionary<string, List<string>> Errors { get; set; }

    public bool ContainsErrors() => Errors.Count > 0;

    public ErrorResponse(Uri documentation, string title, int status, string detail)
    {
        Type = documentation.AbsoluteUri;
        Title = title;
        Status = status;
        Detail = detail;
        Errors = new();
    }

    public void AddError(string key, string message)
    {
        if (!Errors.ContainsKey(key)) Errors[key] = new();
        Errors[key].Add(message);
    }
}