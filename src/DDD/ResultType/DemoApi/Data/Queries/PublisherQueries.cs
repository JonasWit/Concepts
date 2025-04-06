using DemoApi.Models;
using Microsoft.EntityFrameworkCore;

namespace DemoApi.Data.Queries;

public static class PublisherQueries
{
    public static IQueryable<Publisher> WithOptionalId(this IQueryable<Publisher> publishers, int? id) =>
        !id.HasValue ? publishers
        : publishers.Where(publisher => publisher.Id == id.Value);

    public static IQueryable<Publisher> WithKey(this IQueryable<Publisher> publishers, string key) =>
        publishers.Where(publisher => publisher.Key == key);
    
    public static Task<Publisher?> TryFindByKey(this IQueryable<Publisher> publishers, string key) =>
        publishers.FirstOrDefaultAsync(publisher => publisher.Key == key);

    public static async Task<(string? collidingKey, IEnumerable<string> similarKeys)> GetKeyCollisions(this DbSet<Publisher> publishers, string candidateKey) =>
        await publishers.Select(publisher => publisher.Key).FindCollisions(candidateKey);}