using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DemoApi.Data.Converters;

class NullableStringConverter : ValueConverter<string, string?>
{
    public NullableStringConverter() : base(
        name => string.IsNullOrWhiteSpace(name) ? null : name,
        name => name ?? string.Empty,
        new ConverterMappingHints())
    {
    }

    public override bool ConvertsNulls => true;
}