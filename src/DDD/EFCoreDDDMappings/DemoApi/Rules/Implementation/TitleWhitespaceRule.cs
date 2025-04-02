using DemoApi.Models;

namespace DemoApi.Rules.Implementation;

public class TitleWhitespaceRule : ITitleValidity
{
    public bool IsSatisfiedBy(BookTitle title) =>
        !title.Value.StartsWith(' ') && !title.Value.EndsWith(' ') &&
        !title.Value.Contains('\t') && !title.Value.Contains("  ");
    
    public BookTitle ApplyTo(BookTitle title) =>
        IsSatisfiedBy(title) ? title : new(Fix(title.Value), title.Culture);

    private string Fix(string value) =>
        string.Join(" ", value.Replace('\t', ' ').Split(' ', StringSplitOptions.RemoveEmptyEntries));
}