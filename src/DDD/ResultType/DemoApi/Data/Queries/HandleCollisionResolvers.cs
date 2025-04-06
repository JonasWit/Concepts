using System.Globalization;
using System.Text.RegularExpressions;
using DemoApi.Common;
using DemoApi.Models;
using Microsoft.EntityFrameworkCore;

namespace DemoApi.Data.Queries;

public static class HandleCollisionResolvers
{
    public static async Task<string?> TryGetUniqueHandle(this DbSet<Author> authors, PersonalNameToSlug nameToSlug,
                                                         PersonalName name, CultureInfo culture, string? suggestedHandle)
    {
        if (!string.IsNullOrWhiteSpace(suggestedHandle))
        {
            return await authors.AnyAsync(author => author.Key == suggestedHandle) ? null : suggestedHandle;
        }
        return null;        // TODO: Implement unique slug generation
    }

    public static async Task<Result<string, string>> TryGetUniqueHandle(this DbSet<Book> books, BookTitleToSlug titleToSlug, BookTitle title, string? candidateHandle)
    {
        if (!string.IsNullOrWhiteSpace(candidateHandle))
        {
            return await books.AnyAsync(book => book.Key == candidateHandle) ? Result<string, string>.Failure("Handle is in use")
                : Result<string, string>.Success(candidateHandle);
        }

        string slugCandidate = titleToSlug(title.Culture, title.Value).Value;
        (string? collision, IEnumerable<string> similar) = await books.GetKeyCollisions(slugCandidate);
        string handle = AvoidHandleCollisionsWithNumber(slugCandidate, collision, similar);
        return Result<string, string>.Success(handle);
    }

    public static async Task<string?> TryGetUniqueHandle(this DbSet<Publisher> publishers, PublisherNameToSlug nameToSlug, string name, string? candidateHandle)
    {
        if (!string.IsNullOrWhiteSpace(candidateHandle))
        {
            return publishers.Any(publisher => publisher.Key == candidateHandle) ? null : candidateHandle;
        }

        string slugCandidate = nameToSlug(name).Value;
        (string? collision, IEnumerable<string> similar) = await publishers.GetKeyCollisions(slugCandidate);
        return AvoidHandleCollisionsWithNumber(slugCandidate, collision, similar);
    }

    private static string AvoidHandleCollisionsWithNumber(string key, string? collidingKey, IEnumerable<string> similarKeys) =>
        collidingKey is null ? key : $"{AppendNumberToKey(key, collidingKey, similarKeys)}";

    private static string AppendNumberToKey(string key, string collidingKey, IEnumerable<string> similarKeys) =>
        $"{key}-{GetNonCollidingKeySuffixNumber(collidingKey, similarKeys)}";

    private static int GetNonCollidingKeySuffixNumber(string collidingKey, IEnumerable<string> similarKeys) => similarKeys
        .Select(similarKey => TryExtractNumber(collidingKey, similarKey) ?? 0)
        .DefaultIfEmpty(0)
        .Max() + 1;

    private static int? TryExtractNumber(string collidingKey, string similarKey) =>
        similarKey.Length > collidingKey.Length &&
        Regex.Match(similarKey[collidingKey.Length..], @"^-(?<number>\d+)$") is Match match &&
        match.Success
            ? int.Parse(match.Groups["number"].Value)
            : null;
}