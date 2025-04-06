using System.Text.Json.Serialization;
using DemoApi.Data;
using Microsoft.EntityFrameworkCore;

namespace DemoApi.Configuration;

static class AppConfiguration
{
    public static void AddDataServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<BookstoreDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("Bookstore")));

        services.AddScoped<DataSeed>();
    }

    public static void AddWebServices(this IServiceCollection services)
    {
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.AddScoped<UriHelper>();

        services.ConfigureHttpJsonOptions(options =>
        {
            options.SerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        });
    }
}