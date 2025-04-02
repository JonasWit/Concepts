using DemoApi.Models;
using Microsoft.EntityFrameworkCore;

namespace DemoApi.Data.Queries;

public static class AuthorQueries
{
    public static IQueryable<Author> WhereNotPublished(this IQueryable<Author> authors, DbSet<BookAuthor> bookAuthors) =>
        authors.Where(author => !bookAuthors.Any(bookAuthor => bookAuthor.AuthorId == author.Id));
    
    public static IQueryable<Author> WithOptionalId(this IQueryable<Author> authors, int? id) =>
        !id.HasValue ? authors
        : authors.Where(author => author.Id == id.Value);
    
    public static IQueryable<Author> WithOptionalKey(this IQueryable<Author> authors, string? key) =>
        string.IsNullOrWhiteSpace(key) ? authors : authors.Where(author => author.Key == key);
    
    public static async Task<(string? collidingKey, IEnumerable<string> similarKeys)> GetKeyCollisions(this DbSet<Author> authors, string candidateKey) =>
        await authors.Select(author => author.Key).FindCollisions(candidateKey);
    
    public static Task<Author?> TryFindByKey(this IQueryable<Author> authors, string key) =>
        authors.FirstOrDefaultAsync(author => author.Key == key);

    public static async Task<IEnumerable<(Author author, IEnumerable<Book> books)>> GetAuthorsAndBooks(this BookstoreDbContext dbContext, string? handle) =>
        (await dbContext.Authors
            .Where(author => handle == null || author.Key == handle)
            .Select(author => new
            {
                Author = author,
                Books = EF.Property<ICollection<BookAuthor>>(author, "BooksCollection").Select(ba => ba.Book)
            })
            .ToListAsync())
            .Select(record => (record.Author, record.Books));
    
    public static IQueryable<AuthorWithBookTitlesQueryResult> QueryWithBookTitles(this IQueryable<Author> authors) =>
        authors.AsNoTracking()
            .Select(author => new AuthorWithBookTitlesQueryResult(
                author,
                EF.Property<ICollection<BookAuthor>>(author, "BooksCollection")
                    .Select(ba => ba.Book)
                    .Select(book => new BookTitleQueryResult(book.Title.Value, book.Key)).ToArray()));
    
    public record AuthorWithBookTitlesQueryResult(Author Author, BookTitleQueryResult[] BookTitles);
    
    public record BookTitleQueryResult(string Title, string Handle);
}