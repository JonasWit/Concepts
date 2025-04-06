using DemoApi.Models;

namespace DemoApi.Endpoints.Responses;

public record SimplePublisherResponse
(
    string Name,
    string Url
);

public static class SimplePublisherResponseTransforms
{
    public static SimplePublisherResponse ToSimpleResponse(this Publisher publisher, UriHelper uriHelper) =>
        new SimplePublisherResponse(
            publisher.Name,
            uriHelper.FormatPublisherUrl(publisher).AbsoluteUri);
}