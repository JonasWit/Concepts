using DemoApi.Models;
using Microsoft.EntityFrameworkCore;

namespace DemoApi.Data.Queries;

public static class BookAggregates
{
    public static IQueryable<Book> QueryAggregates(this DbSet<Book> books) => books
        .Include("AuthorsCollection")
        .Include("AuthorsCollection.Author")
        .Include(book => book.Publisher)
        .AsNoTracking();
}