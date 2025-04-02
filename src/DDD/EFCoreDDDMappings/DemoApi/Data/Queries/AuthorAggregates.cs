using DemoApi.Models;
using Microsoft.EntityFrameworkCore;

namespace DemoApi.Data.Queries;

public static class AuthorAggregates
{
    public static IQueryable<Author> QueryAggregates(this DbSet<Author> authors) =>
        authors.AsNoTracking();
}