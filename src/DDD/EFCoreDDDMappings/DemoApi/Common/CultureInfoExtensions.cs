using System.Globalization;

namespace DemoApi.Common;

public static class CultureInfoExtensions
{
    public static IEnumerable<CultureInfo> GetSpecificParents(this CultureInfo culture)
    {
        while (culture != CultureInfo.InvariantCulture)
        {
            yield return culture;
            culture = culture.Parent;
        }
    }

    public static CultureInfo? GetLanguageOnly(this CultureInfo culture) =>
        culture.GetSpecificParents().LastOrDefault();
    
    public static CultureInfo? GetLanguageAndCountry(this CultureInfo culture) =>
        culture.GetSpecificParents().Reverse().Skip(1).FirstOrDefault();
}