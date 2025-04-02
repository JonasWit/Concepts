using DemoApi.Data.Queries;
using DemoApi.Models;

namespace DemoApi.Endpoints.Responses;

public static class SimpleBookResponseTransforms
{
    public static SimpleBookResponse ToSimpleResponse(this Book book, UriHelper uriHelper) =>
        new SimpleBookResponse(
            book.Title.Value,
            uriHelper.FormatBookUrl(book).AbsoluteUri);

    public static SimpleBookResponse ToSimpleResponse(this AuthorQueries.BookTitleQueryResult result, UriHelper uriHelper) =>
        new SimpleBookResponse(
            result.Title,
            uriHelper.FormatBookUrl(result.Handle).AbsoluteUri);
}