using Microsoft.EntityFrameworkCore;
using Post.Query.Infrastructure.DataAccess;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

Action<DbContextOptionsBuilder> configureDbContext = o =>
    o.UseLazyLoadingProxies()
        .UseSqlServer(builder.Configuration.GetConnectionString("SqlServer"));

builder.Services.AddDbContext<DatabaseContext>(configureDbContext);
builder.Services.AddSingleton(new DatabaseContextFactory(configureDbContext));

// Create DB
var dataContext = builder.Services.BuildServiceProvider().GetRequiredService<DatabaseContext>();
dataContext.Database.EnsureCreated();

builder.Services.AddOpenApi();
var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();
app.Run();