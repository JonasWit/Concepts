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

        Publisher? publisher = null;
        if (string.IsNullOrWhiteSpace(book.PublisherHandle))
        {
            validationErrors.AddFieldValidationError(nameof(book.PublisherHandle), "Publisher is required");
        }
        else
        {
            publisher = await dbContext.Publishers.TryFindByKey(book.PublisherHandle);
            if (publisher is null) validationErrors.AddFieldValidationError(nameof(book.PublisherHandle), "Publisher not found");
        } 

        List<Author> authors = new();
        IEnumerable<string> authorHandles = [];
        if (book.AuthorHandles is null) validationErrors.AddFieldValidationError(nameof(book.AuthorHandles), "Missing author handles");
        else authorHandles = book.AuthorHandles;

        foreach (var authorHandle in authorHandles.Select((handle, index) => (handle, index)))
        {
            Author? author = await dbContext.Authors.TryFindByKey(authorHandle.handle);
            if (author is null) validationErrors.AddFieldValidationError(nameof(book.AuthorHandles), $"Author #{authorHandle.index + 1} not found");
            else authors.Add(author);
        }

        CultureInfo? titleCulture = null;
        try
        {
            titleCulture = CultureInfo.GetCultureInfo(book.TitleCulture, true);
        }
        catch
        {
            validationErrors.AddFieldValidationError(nameof(book.TitleCulture), "Invalid title culture");
        }

        BookTitle? title = null;
        if (titleCulture != null)
        {
            try
            {
                title = new(book.Title, titleCulture);
            }
            catch
            {
                validationErrors.AddFieldValidationError(nameof(book.Title), "Invalid title");
            }
        }

        CultureInfo? culture = null;
        try
        {
            culture = CultureInfo.GetCultureInfo(book.Culture, true);
        }
        catch
        {
            validationErrors.AddFieldValidationError(nameof(book.TitleCulture), "Invalid title culture");
        }

        string? handle = null;
        if (titleCulture is not null && title != null)
        {
            if (await dbContext.Books.TryGetUniqueHandle(titleToSlug, titleCulture, title.Value, book.Handle) is string uniqueHandle)
            {
                handle = uniqueHandle;
            }
            else
            {
                validationErrors.AddFieldValidationError(nameof(book.Handle), "Handle is already in use");
            }
        }

        if (book.PublishedOn is null) validationErrors.AddFieldValidationError(nameof(book.PublishedOn), "Publishing date is required");

        if (validationErrors.ContainsErrors()) return Results.BadRequest(validationErrors);

        IEdition edition = new OrdinalEdition(book.Edition);
        PublicationInfo pub = new Published(new FullDate(book.PublishedOn ?? throw new InvalidOperationException()));
        Release release = new(publisher ?? throw new InvalidOperationException(), edition, pub);

        Isbn? isbn = book.Isbn is null ? null : new(book.Isbn);

        Book newBook = Book.CreateNew(title ?? throw new InvalidOperationException(),
                                      culture ?? throw new InvalidOperationException(),
                                      isbn, authors, release,
                                      handle ?? throw new InvalidOperationException());

        dbContext.Books.Add(newBook);
        await dbContext.SaveChangesAsync();

        return Results.Created(
            uriHelper.FormatBookUrl(newBook).AbsoluteUri,
            newBook.ToResponse(uriHelper));
    }
}
