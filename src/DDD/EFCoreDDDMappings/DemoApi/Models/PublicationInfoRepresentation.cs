namespace DemoApi.Models;

public record PublicationInfoRepresentation(Type Discriminator, PublicationDate? Date);

public static class PublicationInfoRepresentationConversions
{
    public static PublicationInfoRepresentation ToRepresentation(this PublicationInfo publication) => publication.Map(
        published => new PublicationInfoRepresentation(publication.GetType(), published.PublishedOn),
        planned => new PublicationInfoRepresentation(publication.GetType(), planned.PlannedFor),
        notPlanned => new PublicationInfoRepresentation(publication.GetType(), null));

    public static PublicationInfo ToPublicationInfo(this PublicationInfoRepresentation representation)
    {
        PublicationDate[] args = representation.Date is not null ? [representation.Date] : Array.Empty<PublicationDate>();
        return (PublicationInfo)Activator.CreateInstance(representation.Discriminator, args)!;
    }
}