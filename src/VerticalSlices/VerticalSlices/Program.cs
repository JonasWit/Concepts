using FluentValidation;
using Microsoft.EntityFrameworkCore;
using VerticalSlices.Endpoints;
using VerticalSlices.Products;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options => options.CustomSchemaIds(t => t.FullName?.Replace('+', '.')));

builder.Services.AddDbContext<AppDbContext>(o => o.UseInMemoryDatabase("eshop"));

builder.Services.AddEndpoints();

builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapEndpoints();

app.Run();
