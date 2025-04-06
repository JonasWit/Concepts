namespace DemoApi.Models.PublicationBehavior;

public static class DateComparisons
{
    public static bool IsPublishedBefore(this PublicationInfo publication, DateOnly date) => publication.Map(
        published: published => published.IsPublishedBefore(date),
        planned: planned => planned.IsPublishedBefore(date),
        notPlannedYet: notPlannedYet => false
    );

    public static bool IsPlannedBefore(this PublicationInfo publication, DateOnly date) => publication.Map(
        published: published => published.IsPlannedBefore(date),
        planned: planned => planned.IsPlannedBefore(date),
        notPlannedYet: notPlannedYet => false
    );
}