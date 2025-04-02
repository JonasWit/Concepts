using System.Globalization;

namespace DemoApi.Models;

public class BookTitle
{
    public string Value { get; }
    public CultureInfo Culture { get; }

    public BookTitle(string value, CultureInfo culture)
    {
        Value = !string.IsNullOrWhiteSpace(value) ? value : throw new ArgumentException("Title is required");
        Culture = culture;
    }
}