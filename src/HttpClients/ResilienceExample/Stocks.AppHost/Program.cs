IDistributedApplicationBuilder builder = DistributedApplication.CreateBuilder(args);

IResourceBuilder<PostgresDatabaseResource> stocksDb = builder.AddPostgres("stocks-database")
    .WithDataVolume()
    .AddDatabase("stocks");

IResourceBuilder<ProjectResource> stocksApi = builder.AddProject<Projects.Stocks_Api>("stocks-api")
    .WithReference(stocksDb);

builder.AddProject<Projects.Stocks_Realtime>("stocks-realtime")
    .WithReference(stocksApi);

builder.Build().Run();
