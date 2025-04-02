using System.Security.Claims;
using DemoApi.Models;
using DemoApi.Rules;

namespace DemoApi.Endpoints.Rules;

public class UserRoleTitleRule(Func<ClaimsPrincipal> getUser, string role) : ITitleValidity
{
    public bool IsSatisfiedBy(BookTitle title) =>
        getUser().IsInRole(role);

    public BookTitle ApplyTo(BookTitle title) =>
        title;
}

