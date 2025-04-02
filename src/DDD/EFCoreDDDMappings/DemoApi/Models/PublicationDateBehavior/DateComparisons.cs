namespace DemoApi.Models.PublicationDateBehavior;

public static class DateComparisons
{
    public static bool EndsBefore(this PublicationDate publicationDate, DateOnly date) => publicationDate.Map(
        fullDate: fullDate => fullDate.Date < date,
        yearMonth: yearMonth => yearMonth.Year < date.Year || (yearMonth.Year == date.Year && yearMonth.Month < date.Month),
        year: year => year.Number < date.Year
    );
}