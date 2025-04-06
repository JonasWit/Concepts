using System.Globalization;

namespace DemoApi.Models;

public class Author
{
    public int Id { get; private set; }
    public string Key { get; private set; }
    public string FullName { get; private set; } = string.Empty;
    public PersonalName Name { get; private set; }

    public CultureInfo Culture { get; private set; }
    private ICollection<BookAuthor> BooksCollection { get; } = new List<BookAuthor>();

    public static Author CreateNew(CultureInfo culture, PersonalName name, string fullName, string key) =>
        new(0, key, culture, name, fullName);

    public static Author CreateExisting(int id, CultureInfo culture, PersonalName name, string fullName, string key) =>
        new(id <= 0 ? throw new ArgumentException("Identity must be positive") : id, key, culture, name, fullName);

    private Author(int id, string key, CultureInfo culture, PersonalName name, string fullName)
        : this(id, key, culture, fullName)
    {
        Name = name;
    }

    private Author(int id, string key, CultureInfo culture, string fullName)                    // Used by EF Core
    {
        Id = id;
        Key = key;
        Culture = culture;
        FullName = string.IsNullOrWhiteSpace(fullName) ? throw new ArgumentException("Name must be non-empty", nameof(fullName)) : fullName;
        Name = new(FullName, string.Empty, string.Empty);
    }

    public override string ToString() => $"[{Id}], {FullName}";
}
