using DemoApi.Models;
using DemoApi.Data.Configurations;
using Microsoft.EntityFrameworkCore;

namespace DemoApi.Data;

public class BookstoreDbContext : DbContext
{
    public DbSet<Author> Authors => base.Set<Author>();

    public DbSet<Book> Books => base.Set<Book>();

    public DbSet<BookAuthor> BookAuthors => base.Set<BookAuthor>();

    public DbSet<Publisher> Publishers => base.Set<Publisher>();

    public BookstoreDbContext(DbContextOptions<BookstoreDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        new AuthorConfiguration().Configure(modelBuilder.Entity<Author>());
        new BookConfiguration().Configure(modelBuilder.Entity<Book>());
        new BookAuthorConfiguration().Configure(modelBuilder.Entity<BookAuthor>());
        new PublisherConfiguration().Configure(modelBuilder.Entity<Publisher>());
    }
}