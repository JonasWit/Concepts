using DemoApi.Rules;
using System.Globalization;
using DemoApi.Data;
using DemoApi.Data.Queries;
using DemoApi.Endpoints.Requests;
using DemoApi.Endpoints.Responses;
using DemoApi.Models;
using DemoApi.Models.Editions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DemoApi.Common;

namespace DemoApi.Endpoints;

static class BooksHandlers
{
    public static RouteGroupBuilder MapBooks(this RouteGroupBuilder routes)
    {
        routes.MapGet("/books", GetBooks);
        routes.MapGet("/books/{handle}", GetBook).WithName(UriHelper.QuerySingleBookRouteName);
        routes.MapPost("/books", PostBook);
        return routes;
    }

    public static async Task<IResult> GetBooks(BookstoreDbContext dbContext, UriHelper uriHelper,
                                               [FromQuery] string? culture) =>
        Results.Json((await dbContext.Books.QueryAggregates()
            .WithOptionalCultureName(culture)
            .ToListAsync())
            .Select(book => book.ToResponse(uriHelper)));

    public static async Task<IResult> GetBook(BookstoreDbContext dbContext, UriHelper uriHelper, [FromRoute] string handle) =>
        await dbContext.Books.QueryAggregates().WithKey(handle).FirstOrDefaultAsync() is Book book
            ? Results.Json(book.ToResponse(uriHelper))
            :  Results.NotFound();

    public static async Task<IResult> PostBook(BookstoreDbContext dbContext, UriHelper uriHelper,
                             BookTitleToSlug titleToSlug, [FromBody] PostBookRequest book,
                             ITitleValidity titleValidityRule)
    {
        ValidationErrorResponse validationErrors = new(uriHelper.FormatDocumentationUrl<PostBookRequest>());

        Result<Publisher, string> publisher = await LoadPublisher(dbContext, book);
        if (!publisher.IsSuccess) validationErrors.AddFieldValidationError(nameof(book.PublisherHandle), publisher.Error);

        Result<List<Author>, string> authors = await LoadAuthors(dbContext, book);
        if (!authors.IsSuccess) validationErrors.AddFieldValidationError(nameof(book.AuthorHandles), authors.Error);

        Result<CultureInfo, string> titleCulture = book.TitleCulture.TryParseCultureName();
        if (!titleCulture.IsSuccess) validationErrors.AddFieldValidationError(nameof(book.TitleCulture), titleCulture.Error);

        Result<CultureInfo, string> culture = book.Culture.TryParseCultureName();
        if (!culture.IsSuccess) validationErrors.AddFieldValidationError(nameof(book.Culture), culture.Error);

        if (validationErrors.ContainsErrors()) return Results.BadRequest(validationErrors);

        Result<BookTitle, string> title = BookTitle.TryCreate(book.Title, titleCulture.Value);
        if (!title.IsSuccess) validationErrors.AddFieldValidationError(nameof(book.Title), title.Error);

        if (validationErrors.ContainsErrors()) return Results.BadRequest(validationErrors);
        
        Result<string, string> handle = await dbContext.Books.TryGetUniqueHandle(titleToSlug, title.Value, book.Handle);
        if (!handle.IsSuccess) validationErrors.AddFieldValidationError(nameof(book.Handle), handle.Error);

        Result<DateOnly, string> publishedOn = book.PublishedOn.HasValue ? Result<DateOnly, string>.Success(book.PublishedOn.Value)
            : Result<DateOnly, string>.Failure("Publishing date is required");
        if (!publishedOn.IsSuccess) validationErrors.AddFieldValidationError(nameof(book.PublishedOn), publishedOn.Error);

        if (validationErrors.ContainsErrors()) return Results.BadRequest(validationErrors);

        IEdition edition = new OrdinalEdition(book.Edition);
        PublicationInfo pub = new Published(new FullDate(publishedOn.Value));
        Release release = new(publisher.Value, edition, pub);

        Isbn? isbn = book.Isbn is null ? null : new(book.Isbn);

        Book newBook = Book.CreateNew(title.Value, culture.Value, isbn, authors.Value, release, handle.Value);

        dbContext.Books.Add(newBook);
        await dbContext.SaveChangesAsync();

        return Results.Created(
            uriHelper.FormatBookUrl(newBook).AbsoluteUri,
            newBook.ToResponse(uriHelper));
    }

    private static async Task<Result<Publisher, string>> LoadPublisher(BookstoreDbContext dbContext, PostBookRequest book) => 
        string.IsNullOrWhiteSpace(book.PublisherHandle) ? Result<Publisher, string>.Failure("Publisher is required")
            : await dbContext.Publishers.TryFindByKey(book.PublisherHandle) is Publisher existingPublisher ? Result<Publisher, string>.Success(existingPublisher)
            : Result<Publisher, string>.Failure("Publisher not found");

    private static async Task<Result<List<Author>, string>> LoadAuthors(BookstoreDbContext dbContext, PostBookRequest book)
    {
        List<Author> authors = new();
        if (book.AuthorHandles is null) return Result<List<Author>, string>.Failure("Missing author handles");

        foreach (var authorHandle in book.AuthorHandles.Select((handle, index) => (handle, index)))
        {
            Author? author = await dbContext.Authors.TryFindByKey(authorHandle.handle);
            if (author is null) return Result<List<Author>, string>.Failure($"Author #{authorHandle.index + 1} not found");
            authors.Add(author);
        }

        return Result<List<Author>, string>.Success(authors);
    }
}
