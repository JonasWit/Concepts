namespace DemoApi.Models;

public abstract record PublicationInfo;

public sealed record Published(PublicationDate PublishedOn) : PublicationInfo;
public sealed record Planned(PublicationDate PlannedFor) : PublicationInfo;
public sealed record NotPlannedYet : PublicationInfo;

public static class PublicationInfoExtensions
{
    public static TResult Map<TResult>(this PublicationInfo publication,
        Func<Published, TResult> published,
        Func<Planned, TResult> planned,
        Func<NotPlannedYet, TResult> notPlannedYet) =>
        publication switch
        {
            Published pub => published(pub),
            Planned plan => planned(plan),
            NotPlannedYet notPlanned => notPlannedYet(notPlanned),
            _ => throw new ArgumentException($"Unknown publication info kind: {publication}")
        };
}