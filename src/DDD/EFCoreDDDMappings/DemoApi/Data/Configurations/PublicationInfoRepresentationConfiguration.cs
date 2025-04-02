using DemoApi.Data.Common;
using DemoApi.Data.Converters;
using DemoApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DemoApi.Data.Configurations;

class PublicationInfoRepresentationConfiguration : IComplexPropertyConfiguration<PublicationInfoRepresentation>
{
    public ComplexPropertyBuilder<PublicationInfoRepresentation> Configure(ComplexPropertyBuilder<PublicationInfoRepresentation> builder)
    {
        builder.Property(representation => representation.Discriminator)
            .HasColumnName("PublicationDiscriminator")
            .HasConversion(new TypeDiscriminatorConverter(typeof(Published), typeof(Planned), typeof(NotPlannedYet)));

        builder.Property(representation => representation.Date)
            .HasColumnName("PublicationDate")
            .HasConversion(new PublicationDateConverter());

        return builder;
    }
}
