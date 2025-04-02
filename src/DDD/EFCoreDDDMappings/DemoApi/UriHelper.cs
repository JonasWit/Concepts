using DemoApi.Models;

namespace DemoApi;

public class UriHelper(IHttpContextAccessor httpContextAccessor, LinkGenerator linkGenerator)
{
    public static string QuerySingleAuthorRouteName => "QueryAuthor";
    public static string QuerySingleBookRouteName => "QueryBook";
    public static string QuerySinglePublisherRouteName => "QueryPublisher";
    public static string Documentation => "Documentation";

    private HttpContext HttpContext { get; } = httpContextAccessor.HttpContext ?? throw new ArgumentNullException(nameof(httpContextAccessor));
    private LinkGenerator LinkGenerator { get; } = linkGenerator;

    public Uri FormatAuthorUrl(Author author) =>
        FormatAuthorUrl(author.Key);

    public Uri FormatAuthorUrl(string handle) =>
        new Uri(ResolveRawUri(QuerySingleAuthorRouteName, handle));

    public Uri FormatBookUrl(Book book) =>
        FormatBookUrl(book.Key);

    public Uri FormatBookUrl(string handle) =>
        new Uri(ResolveRawUri(QuerySingleBookRouteName, handle));
    
    public Uri FormatPublisherUrl(Publisher publisher) =>
        new Uri(ResolveRawUri(QuerySinglePublisherRouteName, publisher.Key));

    public Uri FormatDocumentationUrl<TEntity>() =>
        new Uri(LinkGenerator.GetUriByName(HttpContext, Documentation, new { entity = HyphenateName(typeof(TEntity)) }) ?? throw new InvalidOperationException("Route not found"));

    private string ResolveRawUri(string routeName, string handle) =>
        LinkGenerator.GetUriByName(HttpContext, routeName, new { handle }) ?? throw new InvalidOperationException("Route not found");

    private static string HyphenateName(Type type) =>
        string.Concat(type.Name.Select((c, i) => i > 0 && char.IsUpper(c) ? "-" + c.ToString() : c.ToString())).ToLowerInvariant();
}