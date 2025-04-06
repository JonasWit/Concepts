using System.Globalization;

namespace DemoApi.Models;

public interface IAuthorsCollection : IEnumerable<Author>
{
    IEnumerable<CultureInfo> Cultures { get; }

    int Count { get; }

    void Append(Author author);
    void AppendMany(IEnumerable<Author> authors);
    bool RemoveAuthor(int authorId);
    bool MoveAuthorUp(int authorId);
    bool MoveAuthorDown(int authorId);
    bool MoveAuthorToBeginning(int authorId);
    bool MoveAuthorToEnd(int authorId);
}