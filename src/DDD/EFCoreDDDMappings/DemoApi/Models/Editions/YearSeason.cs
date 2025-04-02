namespace DemoApi.Models.Editions;

public enum YearSeason
{
    Spring = 0,
    Summer,
    Autumn,
    Winter
};

public static class YearSeasonExtensions
{
    public static bool IsLast(this YearSeason season) =>
        season == Enum.GetValues<YearSeason>().Last();

    public static YearSeason Next(this YearSeason season) =>
        (YearSeason)(((int)season + 1) % Enum.GetValues<YearSeason>().Length);
}