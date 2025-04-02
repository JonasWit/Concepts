using DemoApi.Models;

namespace DemoApi.Endpoints.Responses;

public record SimpleAuthorResponse
(
    string FullName,
    string Url
);

public static class SimpleAuthorResponseTransforms
{
    public static SimpleAuthorResponse ToSimpleResponse(this Author author, UriHelper uriHelper) =>
        new SimpleAuthorResponse(
            author.FullName,
            uriHelper.FormatAuthorUrl(author).AbsoluteUri);
}