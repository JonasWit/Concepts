using GitHub.Integration.Api;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddOpenApi();

builder.Services.AddHttpClient();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
        policy
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod()
            .SetIsOriginAllowed(_ => true));
});

builder.Services.AddHttpClient("gh-client", httpClient =>
{
    httpClient.BaseAddress = new Uri("https://github-api");
    httpClient.DefaultRequestHeaders.Add("User-Agent", "GitHub-Integration-App");
});

builder.Services.AddHttpClient<GitHubClient>(httpClient =>
{
    httpClient.BaseAddress = new Uri("https://github-api");
    httpClient.DefaultRequestHeaders.Add("User-Agent", "GitHub-Integration-App");
});

var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseCors();

// Simple endpoint to get GitHub user data
app.MapGet("/user", async (string apiKey, GitHubClient gitHubClient) =>
{
    if (string.IsNullOrEmpty(apiKey))
    {
        return Results.BadRequest("API key is required");
    }

    try
    {
        var response = await gitHubClient.GetUser(apiKey);

        if (response is null)
        {
            return Results.NotFound();
        }

        return Results.Ok(response);
    }
    catch (Exception ex)
    {
        return Results.Problem(ex.Message);
    }
});

// Endpoint to get GitHub repositories for the authenticated user
app.MapGet("/repositories", async (string apiKey, GitHubClient gitHubClient) =>
{
    if (string.IsNullOrEmpty(apiKey))
    {
        return Results.BadRequest("API key is required");
    }

    try
    {
        var response = await gitHubClient.GetRepositories(apiKey);

        if (response is null)
        {
            return Results.NotFound();
        }

        return Results.Ok(response);
    }
    catch (Exception ex)
    {
        return Results.Problem(ex.Message);
    }
});

app.Run();
