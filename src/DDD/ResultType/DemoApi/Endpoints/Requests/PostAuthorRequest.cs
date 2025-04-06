namespace DemoApi.Endpoints.Requests;

record PostAuthorRequest
(
    string FullName,
    string Culture,
    string FirstName,
    string? MiddleNames,
    string LastName,
    string? Handle
);
