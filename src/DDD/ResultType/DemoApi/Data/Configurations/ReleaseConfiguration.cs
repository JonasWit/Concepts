using DemoApi.Data.Common;
using DemoApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DemoApi.Data.Configurations;

class ReleaseConfiguration : IComplexPropertyConfiguration<Release>
{
    public ComplexPropertyBuilder<Release> Configure(ComplexPropertyBuilder<Release> entityBuilder)
    {
        entityBuilder.Ignore(release => release.Publisher);

        entityBuilder.Ignore(release => release.Edition);
        entityBuilder.ComplexProperty<EditionRepresentation>("EditionRepresentation")
            .Configure(new EditionRepresentationConfiguration())
            .UsePropertyAccessMode(PropertyAccessMode.Property);

        entityBuilder.Ignore(release => release.Publication);
        entityBuilder.ComplexProperty<PublicationInfoRepresentation>("PublicationRepresentation")
            .Configure(new PublicationInfoRepresentationConfiguration())
            .UsePropertyAccessMode(PropertyAccessMode.Property);

        return entityBuilder;
    }
}
