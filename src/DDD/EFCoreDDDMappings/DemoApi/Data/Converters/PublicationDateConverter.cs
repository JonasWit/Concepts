using DemoApi.Models;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DemoApi.Data.Converters;

public class PublicationDateConverter : ValueConverter<PublicationDate?, int?>
{
    public PublicationDateConverter() : base(
        date => PublicationDateToInt(date),
        value => IntToPublicationDate(value))
    {
    }

    private static int? PublicationDateToInt(PublicationDate? date) =>
        date is null ? null : NonNullPublicationDateToInt(date);

    private static int NonNullPublicationDateToInt(PublicationDate date)
    {
        var (year, month, day) = date.Map(
            full => (full.Date.Year, full.Date.Month, full.Date.Day),
            month => (month.Year, month.Month, 0),
            year => (year.Number, 0, 0));
        return year * 10_000 + month * 100 + day;
    }

    private static PublicationDate? IntToPublicationDate(int? date) =>
        date is null ? null : NonNullIntToPublicationDate(date.Value);

    private static PublicationDate NonNullIntToPublicationDate(int value) => (value / 10_000, value / 100 % 100, value % 100) switch
    {
        (int year, 0, 0) => new Year(year),
        (int year, int month, 0) => new YearMonth(year, month),
        (int year, int month, int day) => new FullDate(new DateOnly(year, month, day)),
    };

    public override bool ConvertsNulls => false;
}