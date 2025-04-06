using DemoApi.Models;
using DemoApi.Models.Editions;

namespace DemoApi.Endpoints.Responses;

public record BookResponse
(
    string Title,
    string Url,
    string Culture,
    string? PublishedOn,
    string Edition,
    PublisherResponse Publisher,
    SimpleAuthorResponse[] Authors
);

public static class BookResponseTransforms
{
    public static BookResponse ToResponse(this Book book, UriHelper uriHelper) =>
        new BookResponse(
            book.Title.Value,
            uriHelper.FormatBookUrl(book).AbsoluteUri,
            book.Culture.Name,
            book.Release.Publication.ToResponse(),
            book.Release.Edition.ToResponse(),
            book.Release.Publisher.ToResponse(uriHelper),
            book.Authors.Select(author => author.ToSimpleResponse(uriHelper)).ToArray());
        
    private static string? ToResponse(this PublicationInfo publication) => publication switch
    {
        Published published => published.PublishedOn.ToResponse(),
        Planned planned => planned.PlannedFor.ToResponse(),
        _ => null
    };

    private static string ToResponse(this PublicationDate date) => date switch
    {
        FullDate full => full.Date.ToString("yyyy-MM-dd"),
        YearMonth yearMonth => $"{yearMonth.Year}-{yearMonth.Month:D2}",
        Year year => year.Number.ToString(),
        _ => throw new InvalidOperationException($"Unexpected publication date type: {date.GetType().Name}")
    };

    private static string ToResponse(this IEdition edition) => edition switch
    {
        OrdinalEdition ordinal => ordinal.Number.ToString(),
        SeasonalEdition seasonal => $"{Enum.GetName<YearSeason>(seasonal.Season)} {seasonal.Year}",
        _ => throw new InvalidOperationException($"Unexpected edition type: {edition.GetType().Name}")
    };
}