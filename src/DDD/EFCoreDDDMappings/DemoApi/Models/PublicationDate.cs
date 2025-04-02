namespace DemoApi.Models;

public abstract record PublicationDate;

public sealed record FullDate(DateOnly Date) : PublicationDate;
public sealed record YearMonth(int Year, int Month) : PublicationDate;
public sealed record Year(int Number) : PublicationDate;

public static class PublicationDateExtensions
{
    public static TResult Map<TResult>(
        this PublicationDate date,
        Func<FullDate, TResult> fullDate,
        Func<YearMonth, TResult> yearMonth,
        Func<Year, TResult> year) =>
        date switch
        {
            FullDate ymd => fullDate(ymd),
            YearMonth ym => yearMonth(ym),
            Year y => year(y),
            _ => throw new ArgumentException($"Unknown publication date kind: {date}")
        };
}
