var builder = DistributedApplication.CreateBuilder(args);

var gitHubApi = builder.AddExternalService("github-api", new Uri("https://api.github.com/"));

builder.AddProject<Projects.GitHub_Integration_Api>("github-integration-api")
    .WithReference(gitHubApi);

builder.Build().Run();
