using System.Globalization;
using DemoApi.Common;

namespace DemoApi.Models;

public class BookTitle
{
    public string Value { get; }
    public CultureInfo Culture { get; }

    private BookTitle(string value, CultureInfo culture) =>
        (Value, Culture) = (value, culture);

    public static Result<BookTitle, string> TryCreate(string value, CultureInfo culture) => 
        string.IsNullOrWhiteSpace(value) ? Result<BookTitle, string>.Failure("Title is required")
        : Result<BookTitle, string>.Success(new BookTitle(value, culture));
}