using System.Globalization;
using System.Collections;
using DemoApi.Common;

namespace DemoApi.Models;

public class AuthorsList : IAuthorsCollection
{
    private Book Book { get; }
    private List<BookAuthor> AuthorsCollection { get; set; }
    
    public IEnumerable<CultureInfo> Cultures =>
        AuthorsCollection.Select(author => author.Author.Culture).Distinct();

    public AuthorsList(Book book) : this(book, Enumerable.Empty<Author>()) { }

    public AuthorsList(Book book, IEnumerable<Author> authors)
    {
        Book = book;
        AuthorsCollection = authors.Select((author, offset) => new BookAuthor(book, author, offset + 1)).ToList();
    }

    public int Count => AuthorsCollection.Count;

    public void Append(Author author)
    {
        AuthorsCollection.Add(new BookAuthor(Book, author, AuthorsCollection.Count + 1));
    }

    public void AppendMany(IEnumerable<Author> authors)
    {
        foreach (Author author in authors) Append(author);
    }

    public bool RemoveAuthor(int authorId) =>
        AuthorsCollection.Remove(FilterById(authorId));

    private Func<BookAuthor, bool> FilterById(int authorId) =>
        author => author.AuthorId == authorId;

    public bool MoveAuthorUp(int authorId) =>
        AuthorsCollection.SwapWithPrevious(FilterById(authorId));
    
    public bool MoveAuthorDown(int authorId) =>
        AuthorsCollection.SwapWithNext(FilterById(authorId));
    
    public bool MoveAuthorToBeginning(int authorId) =>
        AuthorsCollection.MoveToBeginning(FilterById(authorId));
    
    public bool MoveAuthorToEnd(int authorId) =>
        AuthorsCollection.MoveToEnd(FilterById(authorId));

    public IEnumerator<Author> GetEnumerator() => AuthorsCollection
        .OrderBy(author => author.Ordinal)
        .Select(author => author.Author)
        .GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

    public override string ToString() => string.Join(", ", AuthorsCollection);
}

