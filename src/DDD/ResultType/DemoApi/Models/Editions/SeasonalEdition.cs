namespace DemoApi.Models.Editions;

public class SeasonalEdition(YearSeason season, int year) : IEdition
{
    public YearSeason Season { get; private set; } = season;
    public int Year { get; private set; } = year;

    public IEdition AdvanceToNext() =>
        new SeasonalEdition(Season.Next(), Season.IsLast() ? Year + 1 : Year);
}
