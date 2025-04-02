using DemoApi.Models;

namespace DemoApi.Rules.Implementation;

public class NonEmptyTitleRule : ITitleValidity
{
    public bool IsSatisfiedBy(BookTitle title) =>
        !string.IsNullOrWhiteSpace(title.Value);

    public BookTitle ApplyTo(BookTitle title) =>
        IsSatisfiedBy(title) ? title : new(string.Empty, title.Culture);
}

