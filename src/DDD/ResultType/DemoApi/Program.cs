using DemoApi;
using DemoApi.Configuration;
using DemoApi.Endpoints;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services.AddLogging();

builder.Services.AddDomainServices();
builder.Services.AddDataServices(builder.Configuration);
builder.Services.AddWebServices();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseStaticFiles();

    app.MapOpenApi();
    
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "Demo API");
        options.InjectStylesheet("/swagger-ui/swagger-dark.css");
        options.RoutePrefix = string.Empty;
    });
}

app.MapGet("/seed", SeedHandlers.Get);

app.MapGet("/authors", AuthorsHandlers.GetAuthors);
app.MapGet("/authors/{handle}", AuthorsHandlers.GetAuthor).WithName(UriHelper.QuerySingleAuthorRouteName);
app.MapPost("/authors", AuthorsHandlers.PostAuthor);

app.MapGet("/books", BooksHandlers.GetBooks);
app.MapGet("/books/{handle}", BooksHandlers.GetBook).WithName(UriHelper.QuerySingleBookRouteName);
app.MapPost("/books", BooksHandlers.PostBook);

app.MapGet("/publishers", PublishersHandlers.GetPublishers);
app.MapGet("/publishers/{handle}", PublishersHandlers.GetPublisher).WithName(UriHelper.QuerySinglePublisherRouteName);
app.MapPost("/publishers", PublishersHandlers.PostBook);

app.MapGet("/docs/{entity}", DocumentationHandlers.GetDocumentation).WithName(UriHelper.Documentation);

app.Run();
