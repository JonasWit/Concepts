using System.Globalization;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DemoApi.Data.Converters;

public class CultureInfoConverter : ValueConverter<CultureInfo, string>
{
    public CultureInfoConverter() : base(
        culture => culture.Name,
        cultureName => CultureInfo.GetCultureInfo(cultureName)) { }
}
