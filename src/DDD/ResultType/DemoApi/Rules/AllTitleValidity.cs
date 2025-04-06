using DemoApi.Models;

namespace DemoApi.Rules;

public class AllTitleValidity(params ITitleValidity[] rules) : ITitleValidity
{
    public bool IsSatisfiedBy(BookTitle title) =>
        rules.All(rule => rule.IsSatisfiedBy(title));

    public BookTitle ApplyTo(BookTitle title) =>
        rules.Aggregate(title, (result, rule) => rule.ApplyTo(result));
}

