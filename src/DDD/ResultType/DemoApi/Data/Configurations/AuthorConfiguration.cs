using DemoApi.Data.Common;
using DemoApi.Data.Converters;
using DemoApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DemoApi.Data.Configurations;

class AuthorConfiguration : IEntityTypeConfiguration<Author>
{
    public void Configure(EntityTypeBuilder<Author> entity)
    {
        entity.ToTable("Authors");

        entity.ComplexProperty(author => author.Name).Configure(new PersonalNameConfiguration());

        entity.Property(author => author.Culture).HasConversion(new CultureInfoConverter());

        entity.HasIndex(author => author.Key).IsUnique();
    }
}