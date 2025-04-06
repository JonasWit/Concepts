using DemoApi.Data;
using DemoApi.Data.Queries;
using DemoApi.Endpoints.Requests;
using DemoApi.Endpoints.Responses;
using DemoApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DemoApi.Endpoints;

static class PublishersHandlers
{
    public static RouteGroupBuilder MapPublishers(this RouteGroupBuilder routes)
    {
        routes.MapGet("/publishers", GetPublishers);
        routes.MapGet("/publishers/{handle}", GetPublisher).WithName(UriHelper.QuerySinglePublisherRouteName);
        routes.MapPost("/publishers", PostBook);
        return routes;
    }

    public static async Task<IResult> GetPublishers(BookstoreDbContext dbContext, UriHelper uriHelper) =>
        Results.Json((await dbContext.Publishers.ToListAsync()).Select(publisher => publisher.ToResponse(uriHelper)));

    public static async Task<IResult> GetPublisher(BookstoreDbContext dbContext, UriHelper uriHelper, [FromRoute] string handle) =>
        await dbContext.Publishers.WithKey(handle).FirstOrDefaultAsync() is Publisher publisher
            ?  Results.Json(publisher.ToResponse(uriHelper))
            : Results.NotFound();

    public static async Task<IResult> PostBook(BookstoreDbContext dbContext, UriHelper uriHelper, PublisherNameToSlug nameToSlug, [FromBody] PostPublisherRequest publisher)
    {
        ValidationErrorResponse validationErrors = new(uriHelper.FormatDocumentationUrl<PostPublisherRequest>());

        string? name = null;
        string? handle = null;

        if (string.IsNullOrWhiteSpace(publisher.Name))
        {
            validationErrors.AddFieldValidationError(nameof(publisher.Name), "Name is required");
        }
        else
        {
            name = publisher.Name;
            handle = await dbContext.Publishers.TryGetUniqueHandle(nameToSlug, publisher.Name, publisher.Handle);
            if (handle is null) validationErrors.AddFieldValidationError(nameof(publisher.Handle), "Handle is already in use");
        }

        if (validationErrors.ContainsErrors()) return Results.BadRequest(validationErrors);
        
        var newPublisher = Publisher.CreateNew(name ?? throw new InvalidOperationException(),   
                                            handle ?? throw new InvalidOperationException());

        dbContext.Publishers.Add(newPublisher);
        await dbContext.SaveChangesAsync();

        return Results.Created(
            uriHelper.FormatPublisherUrl(newPublisher).AbsoluteUri,
            newPublisher.ToResponse(uriHelper));
    }
}