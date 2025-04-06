using DemoApi.Models;

namespace DemoApi.Endpoints.Responses;

public record AuthorResponse
(
    string FirstName,
    string LastName,
    string FullName,
    string Culture,
    string Url
);

public static class AuthorResponseTransforms
{
    public static AuthorResponse ToAuthorResponse(this Author author, UriHelper uriHelper) =>
        new AuthorResponse(
            author.Name.First,
            author.Name.Last,
            author.FullName,
            author.Culture.Name,
            uriHelper.FormatAuthorUrl(author).AbsoluteUri);
}