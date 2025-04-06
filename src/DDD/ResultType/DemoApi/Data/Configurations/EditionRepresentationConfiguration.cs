using DemoApi.Data.Common;
using DemoApi.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using DemoApi.Data.Converters;
using DemoApi.Models.Editions;

namespace DemoApi.Data.Configurations;

public class EditionRepresentationConfiguration : IComplexPropertyConfiguration<EditionRepresentation>
{
    public ComplexPropertyBuilder<EditionRepresentation> Configure(ComplexPropertyBuilder<EditionRepresentation> builder)
    {
        builder.Property(representation => representation.Discriminator)
            .HasColumnName("EditionDiscriminator")
            .HasConversion(new TypeDiscriminatorConverter(typeof(SeasonalEdition), typeof(OrdinalEdition)));

        builder.Property(representation => representation.Season)
            .HasColumnName("EditionSeason")
            .HasConversion(new YearSeasonConverter());

        builder.Property(representation => representation.Number)
            .HasColumnName("EditionNumber");

        return builder;
    }
}