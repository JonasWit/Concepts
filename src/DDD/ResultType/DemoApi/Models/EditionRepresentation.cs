using DemoApi.Models.Editions;

namespace DemoApi.Models;

public record EditionRepresentation(Type Discriminator, YearSeason? Season, int Number);

public static class EditionRepresentationConversions
{
    public static EditionRepresentation ToRepresentation(this IEdition edition) =>
        edition switch
        {
            SeasonalEdition seasonal => new EditionRepresentation(edition.GetType(), seasonal.Season, seasonal.Year),
            OrdinalEdition ordinal => new EditionRepresentation(edition.GetType(), null, ordinal.Number),
            _ => throw new InvalidOperationException($"Unsupported edition type {edition.GetType().Name}")
        };
    
    public static IEdition ToEdition(this EditionRepresentation representation) =>
        representation.Discriminator == typeof(SeasonalEdition) && representation.Season.HasValue ? (IEdition)new SeasonalEdition(representation.Season.Value, representation.Number)
        : representation.Discriminator == typeof(OrdinalEdition) ? new OrdinalEdition(representation.Number)
        : throw new InvalidOperationException($"Invalid representation");
}