using Microsoft.AspNetCore.Mvc;

namespace DemoApi.Endpoints;

static class DocumentationHandlers
{
    public static RouteGroupBuilder MapDocumentation(this RouteGroupBuilder routes)
    {
        routes.MapGet("/docs/{entity}", GetDocumentation).WithName(UriHelper.Documentation);
        return routes;
    }

    public static IResult GetDocumentation([FromRoute] string entity) =>
        Results.Ok("Documentation goes here");
}