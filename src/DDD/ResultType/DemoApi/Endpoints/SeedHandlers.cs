using DemoApi.Data;

static class SeedHandlers
{
    public static RouteGroupBuilder MapRoot(this RouteGroupBuilder routes)
    {
        routes.MapGet("/", Get);
        return routes;
    }

    public static async Task<IResult> Get(DataSeed dataSeed)
    {
        await dataSeed.SeedBooks();
        return Results.Ok("The database is seeded!");
    }
}