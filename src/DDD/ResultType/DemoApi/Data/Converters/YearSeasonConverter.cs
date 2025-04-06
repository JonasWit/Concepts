using DemoApi.Models.Editions;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DemoApi.Data.Converters;

class YearSeasonConverter : ValueConverter<YearSeason?, string>
{
    public YearSeasonConverter() : base(
        season => YearSeasonToString(season),
        str => StringToYearSeason(str),
        new ConverterMappingHints(size: Enum.GetNames<YearSeason>().Max(name => name.Length), unicode: true)) { }

    private static string YearSeasonToString(YearSeason? season) =>
        season.HasValue && Enum.GetName(season.Value) is string name ? name : string.Empty;

    private static YearSeason? StringToYearSeason(string name) =>
        name is not null && Enum.TryParse<YearSeason>(name, out YearSeason season) ? season : null;
}