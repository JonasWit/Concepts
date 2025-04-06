using System.Globalization;

namespace DemoApi.Common;

public static class CultureInfoParsing
{
    public static Result<CultureInfo, string> TryParseCultureName(this string cultureName)
    {
        try
        {
            return Result<CultureInfo, string>.Success(CultureInfo.GetCultureInfo(cultureName, true));
        }
        catch (CultureNotFoundException)
        {
            return Result<CultureInfo, string>.Failure("Invalid culture name");
        }
    }
}