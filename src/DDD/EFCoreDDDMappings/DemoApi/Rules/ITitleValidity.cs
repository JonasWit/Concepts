using DemoApi.Models;

namespace DemoApi.Rules;

public interface ITitleValidity
{
    bool IsSatisfiedBy(BookTitle title);
    BookTitle ApplyTo(BookTitle title);
}