using System.Collections;
using System.Globalization;

namespace DemoApi.Models;

public class BookAuthorsCollectionWrapper(Book book, ICollection<BookAuthor> bookAuthors) : IAuthorsCollection
{
    private Book Book { get; } = book;
    private ICollection<BookAuthor> BookAuthors { get; } = bookAuthors;

    public IEnumerable<CultureInfo> Cultures =>
        BookAuthors.Select(author => author.Author.Culture).Distinct();

    public int Count => BookAuthors.Count;

    public void Append(Author author) =>
        BookAuthors.Add(new BookAuthor(Book, author, BookAuthors.Count + 1));

    public void AppendMany(IEnumerable<Author> authors)
    {
        foreach (Author author in authors) Append(author);
    }

    public IEnumerator<Author> GetEnumerator() =>
        BookAuthors.Select(author => author.Author).GetEnumerator();

    public bool MoveAuthorDown(int authorId)
    {
        if (FindById(authorId) is BookAuthor author &&
            FindByOrdinal(author.Ordinal + 1) is BookAuthor nextAuthor)
        {
            author.Ordinal += 1;
            nextAuthor.Ordinal -= 1;
            return true;
        }

        return false;
    }

    private BookAuthor? FindById(int authorId) =>
        BookAuthors.FirstOrDefault(author => author.AuthorId == authorId);

    private BookAuthor? FindByOrdinal(int ordinal) =>
        BookAuthors.FirstOrDefault(author => author.Ordinal == ordinal);

    public bool MoveAuthorToBeginning(int authorId)
    {
        if (FindById(authorId) is BookAuthor author && author.Ordinal > 1)
        {
            var move = BookAuthors.Where(candidate => candidate.Ordinal < author.Ordinal);
            foreach (BookAuthor previous in move) previous.Ordinal += 1;
            author.Ordinal = 1;
            return true;
        }

        return false;
    }

    public bool MoveAuthorToEnd(int authorId)
    {
        if (FindById(authorId) is BookAuthor author && author.Ordinal < BookAuthors.Count)
        {
            var move = BookAuthors.Where(candidate => candidate.Ordinal > author.Ordinal);
            foreach (BookAuthor next in move) next.Ordinal -= 1;
            author.Ordinal = BookAuthors.Count;
            return true;
        }

        return false;
    }

    public bool MoveAuthorUp(int authorId)
    {
        if (FindById(authorId) is BookAuthor author &&
            FindByOrdinal(author.Ordinal - 1) is BookAuthor previousAuthor)
        {
            author.Ordinal -= 1;
            previousAuthor.Ordinal += 1;
            return true;
        }

        return false;
    }

    public bool RemoveAuthor(int authorId)
    {
        if (FindById(authorId) is BookAuthor author)
        {
            var snap = BookAuthors.Where(candidate => candidate.Ordinal > author.Ordinal);
            BookAuthors.Remove(author);
            foreach (BookAuthor next in snap) next.Ordinal -= 1;
            return true;
        }

        return false;
    }

    IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
}