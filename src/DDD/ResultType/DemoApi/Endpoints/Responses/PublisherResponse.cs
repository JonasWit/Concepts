using DemoApi.Models;

namespace DemoApi.Endpoints.Responses;

public record PublisherResponse
(
    string Name,
    string Url
);

public static class PublisherResponseTransforms
{
    public static PublisherResponse ToResponse(this Publisher publisher, UriHelper uriHelper) =>
        new PublisherResponse(
            publisher.Name,
            uriHelper.FormatPublisherUrl(publisher).AbsoluteUri);
}