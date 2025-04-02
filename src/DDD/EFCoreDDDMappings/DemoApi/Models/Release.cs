using DemoApi.Models.Editions;

namespace DemoApi.Models;

public class Release
{
    public Publisher Publisher { get; set; }

    public IEdition Edition { get; private set; }

    private EditionRepresentation EditionRepresentation
    {
        get => Edition.ToRepresentation();
        set => Edition = value.ToEdition();
    }

    public PublicationInfo Publication { get; private set; }

    private PublicationInfoRepresentation PublicationRepresentation
    {
        get => Publication.ToRepresentation();
        set => Publication = value.ToPublicationInfo();
    }

    public Release(Publisher publisher, IEdition edition, PublicationInfo publication) =>
        (Publisher, Edition, Publication) = (publisher, edition, publication);

    private YearSeason? EditionSeason => Edition is SeasonalEdition seasonal ? seasonal.Season : null;
    private int EditionNumber => Edition switch
    {
        SeasonalEdition seasonal => seasonal.Year,
        OrdinalEdition ordinal => ordinal.Number,
        _ => throw new InvalidOperationException($"Unsupported edition type {Edition.GetType().Name}")
    };

    private Release()
    {
        Publisher = default!;

        Edition = new OrdinalEdition(1);
        
        Publication = new NotPlannedYet();
    }

    public void AdvanceToNext(PublicationInfo publication)
    {
        Publication = publication;
        Edition = Edition.AdvanceToNext();
    }

    public void SetStatus(PublicationInfo publication)
    {
        if (Publication is Published) return;
        Publication = publication;
    }
}
