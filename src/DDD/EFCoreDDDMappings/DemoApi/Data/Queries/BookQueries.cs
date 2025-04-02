using System.Globalization;
using DemoApi.Migrations;
using DemoApi.Models;
using Microsoft.EntityFrameworkCore;

namespace DemoApi.Data.Queries;

public static class BookQueries
{
    public static IQueryable<Book> WithOptionalCultureName(this IQueryable<Book> book, string? cultureName)
    {
        if (cultureName is null) return book;
        CultureInfo culture = CultureInfo.GetCultureInfo(cultureName);
        return book.Where(book => book.Culture == culture);
    }

    public static IQueryable<Book> WithOptionalId(this IQueryable<Book> books, int? id) =>
        !id.HasValue ? books
        : books.Where(book => book.Id == id);

    public static IQueryable<Book> WithKey(this IQueryable<Book> books, string key) =>
        books.Where(book => book.Key == key);
    
    public static IQueryable<Book> WithOptionalAuthorId(this IQueryable<Book> books, int? authorId) =>
        !authorId.HasValue ? books
        : books.Where(book =>
            EF.Property<ICollection<BookAuthor>>(book, "AuthorsCollection").Any(bookAuthor => bookAuthor.AuthorId == authorId.Value));

    public static IQueryable<Book> WithOptionalAuthorKey(this IQueryable<Book> books, string? key) =>
        key == null ? books
        : books.Where(book =>
            EF.Property<ICollection<BookAuthor>>(book, "AuthorsCollection").Any(bookAuthor => bookAuthor.Author.Key == key));

    public static async Task<(string? collidingKey, IEnumerable<string> similarKeys)> GetKeyCollisions(this DbSet<Book> books, string candidateKey) =>
        await books.Select(book => book.Key).FindCollisions(candidateKey);

    public static async Task<IEnumerable<(Author author, IEnumerable<Book> books)>> GetPublishedAuthorsAndBooks(this IQueryable<Book> books, string? handle) =>
        (await books.WithOptionalAuthorKey(handle).ToListAsync())
            .SelectMany(book => book.Authors.Select(author => (author: author, book: book)))
            .Where(pair => handle == null || pair.author.Key == handle)
            .GroupBy(
                pair => pair.author,
                pair => pair.book,
                EqualityComparer<Author>.Create((a, b) => a?.Id == b?.Id, a => a.Id.GetHashCode()))
            .Select(group => (group.Key, (IEnumerable<Book>)group));
}