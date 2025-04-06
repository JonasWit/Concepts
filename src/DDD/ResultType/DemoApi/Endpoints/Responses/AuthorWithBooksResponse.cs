namespace DemoApi.Endpoints.Responses;

public record AuthorWithBooksResponse
(
    string FirstName,
    string LastName,
    string FullName,
    string Culture,
    string Url,
    SimpleBookResponse[] Books
);
