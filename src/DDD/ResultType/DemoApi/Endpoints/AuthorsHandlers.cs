using System.Globalization;
using DemoApi.Data;
using DemoApi.Data.Queries;
using DemoApi.Endpoints.Requests;
using DemoApi.Endpoints.Responses;
using DemoApi.Models;
using Microsoft.EntityFrameworkCore;

namespace DemoApi.Endpoints;

class AuthorsHandlers
{
    public static async Task<IResult> GetAuthors(BookstoreDbContext dbContext, UriHelper uriHelper) =>
        Results.Json(
            (await dbContext.Authors.OrderBy(author => author.Name.Last).QueryWithBookTitles().ToListAsync())
                .Select(result => result.ToAuthorWithBooksResponse(uriHelper)));

    public static async Task<IResult> GetAuthor(BookstoreDbContext dbContext, UriHelper uriHelper, string handle) =>
        (await dbContext.Authors.Where(author => author.Key == handle).QueryWithBookTitles().ToListAsync())
            .Select(result => result.ToAuthorWithBooksResponse(uriHelper))
            .Select(response => Results.Json(response))
            .DefaultIfEmpty(Results.NotFound())
            .Single();

    public static async Task<IResult> PostAuthor(ILogger<AuthorsHandlers> logger, BookstoreDbContext dbContext,
                                                 UriHelper uriHelper, PersonalNameToSlug nameToSlug,
                                                 PostAuthorRequest author)
    {
        ValidationErrorResponse validationErrors = new(uriHelper.FormatDocumentationUrl<PostAuthorRequest>());

        if (string.IsNullOrWhiteSpace(author.FullName))
            validationErrors.AddFieldValidationError(nameof(author.FullName), "Full name is required");

        if (string.IsNullOrWhiteSpace(author.FirstName))
            validationErrors.AddFieldValidationError(nameof(author.FirstName), "First name is required");

        if (string.IsNullOrWhiteSpace(author.LastName))
            validationErrors.AddFieldValidationError(nameof(author.LastName), "Last name is required");

        CultureInfo? culture = null;
        if (string.IsNullOrWhiteSpace(author.Culture))
        {
            validationErrors.AddFieldValidationError(nameof(author.Culture), "Culture is required");
        }
        else
        {
            culture = CultureInfo.GetCultureInfo(author.Culture, true);
        }

        PersonalName name = new(author.FirstName, author.MiddleNames ?? string.Empty, author.LastName);
        string fullName = author.FullName;

        string? handle = null;
        if (culture is not null)
        {
            handle = await dbContext.Authors.TryGetUniqueHandle(nameToSlug, name, culture, author.Handle);
            if (author.Handle is not null && handle is null)
            {
                validationErrors.AddFieldValidationError(nameof(author.Handle), "Handle is already in use");
            }
        }

        if (validationErrors.ContainsErrors()) return Results.BadRequest(validationErrors);

        var newAuthor = Author.CreateNew(culture ?? throw new InvalidOperationException(), name,
                                        fullName, handle!);     // TODO: Implement unique slug generation

        dbContext.Authors.Add(newAuthor);
        await dbContext.SaveChangesAsync();

        return Results.Json(newAuthor.ToAuthorResponse(uriHelper));
    }
}