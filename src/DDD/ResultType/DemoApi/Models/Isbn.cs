namespace DemoApi.Models;

public class Isbn
{
    public string Value { get; }

    public Isbn(string value)
    {
        if (value.Length != 13 || !long.TryParse(value, out _)) throw new ArgumentException("Invalid ISBN");
        Value = value;
    }
}