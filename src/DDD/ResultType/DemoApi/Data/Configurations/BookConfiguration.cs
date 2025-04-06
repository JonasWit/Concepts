using DemoApi.Data.Common;
using DemoApi.Data.Converters;
using DemoApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DemoApi.Data.Configurations;

class BookConfiguration : IEntityTypeConfiguration<Book>
{
    public void Configure(EntityTypeBuilder<Book> entityBuilder)
    {
        entityBuilder.ToTable("Books");

        entityBuilder.Ignore(book => book.Authors);

        entityBuilder.Property(book => book.Isbn)
            .HasConversion(new NullableIsbnConverter());
        
        entityBuilder.ComplexProperty(
            book => book.Title,
            builder =>
            {
                builder.Property(title => title.Value).HasColumnName("Title");
                builder.Property(title => title.Culture).HasColumnName("TitleCulture")
                    .HasConversion(new CultureInfoConverter());
            });
        
        entityBuilder.Property(book => book.Culture)
            .HasConversion(new CultureInfoConverter());

        entityBuilder.ComplexProperty(book => book.Release).Configure(new ReleaseConfiguration());

        entityBuilder.HasOne(book => book.Publisher).WithMany().HasForeignKey("PublisherId");

        entityBuilder.HasIndex(book => book.Key).IsUnique();
    }
}
