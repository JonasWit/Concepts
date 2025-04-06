using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DemoApi.Data.Converters;

class TypeDiscriminatorConverter : ValueConverter<Type, string>
{
    public TypeDiscriminatorConverter(params Type[] oneOf) : base(
        type => type.Name,
        name => oneOf.First(type => type.Name == name),
        new ConverterMappingHints(size: oneOf.Max(type => type.Name.Length), unicode: true)) { }
}