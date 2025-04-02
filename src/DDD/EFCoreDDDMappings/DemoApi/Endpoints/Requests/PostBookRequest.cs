namespace DemoApi.Endpoints.Requests;

record PostBookRequest
(
    string Title,
    string TitleCulture,
    string Culture,
    string? Isbn,
    string PublisherHandle,
    string[] AuthorHandles,
    int Edition,
    DateOnly? PublishedOn,
    string? Handle
);
