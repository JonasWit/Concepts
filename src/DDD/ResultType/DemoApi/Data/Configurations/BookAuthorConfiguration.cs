using DemoApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DemoApi.Data.Configurations;

class BookAuthorConfiguration : IEntityTypeConfiguration<BookAuthor>
{
    public void Configure(EntityTypeBuilder<BookAuthor> entityBuilder)
    {
        entityBuilder.ToTable("BookAuthors");

        entityBuilder.HasKey(bookAuthor => new { bookAuthor.BookId, bookAuthor.AuthorId });

        entityBuilder.HasOne(bookAuthor => bookAuthor.Book)
            .WithMany("AuthorsCollection")
            .HasForeignKey(bookAuthor => bookAuthor.BookId);

        entityBuilder.HasOne(bookAuthor => bookAuthor.Author)
            .WithMany("BooksCollection")
            .HasForeignKey(bookAuthor => bookAuthor.AuthorId);
    }
}
