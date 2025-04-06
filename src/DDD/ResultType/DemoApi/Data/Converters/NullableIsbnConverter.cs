using DemoApi.Models;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DemoApi.Data.Converters;

public class NullableIsbnConverter : ValueConverter<Isbn?, string?>
{
    public NullableIsbnConverter() : base(
        isbn => isbn == null ? null : isbn.Value,
        value => value == null ? null : new Isbn(value)) { }
}