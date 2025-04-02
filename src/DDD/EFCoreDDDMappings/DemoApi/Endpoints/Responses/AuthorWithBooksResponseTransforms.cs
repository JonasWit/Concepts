using DemoApi.Data.Queries;
using DemoApi.Models;

namespace DemoApi.Endpoints.Responses;

public static class AuthorWithBooksResponseTransforms
{
    public static AuthorWithBooksResponse ToAuthorWithBooksResponse(this (Author author, IEnumerable<Book> books) tuple, UriHelper uriHelper) =>
        new AuthorWithBooksResponse(
            tuple.author.Name.First,
            tuple.author.Name.Last,
            tuple.author.FullName,
            tuple.author.Culture.Name,
            uriHelper.FormatAuthorUrl(tuple.author.Key).AbsoluteUri,
            tuple.books.Select(book => book.ToSimpleResponse(uriHelper)).ToArray());

    public static AuthorWithBooksResponse ToAuthorWithBooksResponse(this AuthorQueries.AuthorWithBookTitlesQueryResult result, UriHelper uriHelper) =>
        new AuthorWithBooksResponse(
            result.Author.Name.First,
            result.Author.Name.Last,
            result.Author.FullName,
            result.Author.Culture.Name,
            uriHelper.FormatAuthorUrl(result.Author.Key).AbsoluteUri,
            result.BookTitles.Select(book => book.ToSimpleResponse(uriHelper)).ToArray());
}