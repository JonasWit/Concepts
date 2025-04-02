using DemoApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DemoApi.Data.Configurations;

class PublisherConfiguration : IEntityTypeConfiguration<Publisher>
{
    public void Configure(EntityTypeBuilder<Publisher> entityBuilder)
    {
        entityBuilder.ToTable("Publishers");
        entityBuilder.Property(publisher => publisher.Name).IsRequired();

        entityBuilder.HasIndex(publisher => publisher.Key).IsUnique();
    }
}