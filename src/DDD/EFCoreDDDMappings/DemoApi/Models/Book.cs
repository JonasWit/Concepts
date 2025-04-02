using System.Globalization;
using DemoApi.Common;

namespace DemoApi.Models;

public class Book
{
    public int Id { get; private set; }
    public string Key { get; private set; }
    public BookTitle Title { get; private set; }
    public Isbn? Isbn { get; private set; }

    public CultureInfo Culture { get; private set; }

    public Release Release { get; private set; }

    public Publisher Publisher
    {
        get => Release.Publisher;
        set => Release.Publisher = value;
    }

    public IEnumerable<Author> Authors => AuthorsCollection.OrderBy(ba => ba.Ordinal).Select(ba => ba.Author);
    private ICollection<BookAuthor> AuthorsCollection { get; } = new List<BookAuthor>();

    public static Book CreateNew(
        BookTitle title, CultureInfo culture, Isbn? isbn,
        IEnumerable<Author> authors, Release release, string key) =>
        new(0, key, title, culture, isbn, authors, release);

    public static Book CreateExisting(
        int id, BookTitle title, CultureInfo culture,
        Isbn? isbn, IEnumerable<Author> authors, Release release, string key) =>
        new(id <= 0 ? throw new ArgumentException("Identity must be positive") : id,
            key, title, culture, isbn, authors, release);

    private Book(int id, string key, BookTitle title, CultureInfo culture,
                 Isbn? isbn, IEnumerable<Author> authors, Release release)
        : this(id, key, culture, isbn)
    {
        AuthorsCollection = authors.Select((author, index) => new BookAuthor(this, author, index + 1)).ToList();
        Release = release;
        Title = title;
    }

    // Used by EF Core
    private Book(int id, string key, CultureInfo culture, Isbn? isbn) =>
        (Id, Key, Title, Release, Culture, Isbn) =
        (id, key, default!, default!, culture, isbn);

    public (Author author, int ordinal) Append(Author author)
    {
        if (TryFind(author) is BookAuthor existing) return (existing.Author, existing.Ordinal);
        var @new = new BookAuthor(this, author, AuthorsCount + 1);
        AuthorsCollection.Add(@new);
        return (@new.Author, @new.Ordinal);
    }

    public bool Remove(Author author)
    {
        var existing = TryFind(author);
        if (existing is null) return false;

        foreach (var next in AuthorsCollection.Where(ba => ba.Ordinal > existing.Ordinal)) next.Ordinal -= 1;

        AuthorsCollection.Remove(existing);
        return true;
    }

    private BookAuthor? TryFind(Author author) =>
        author.Id == 0 ? null
        : AuthorsCollection.FirstOrDefault(ba => ba.Author.Id == author.Id);

    public int AuthorsCount => AuthorsCollection.Count;

    public override string ToString() => Title.Value;
}
