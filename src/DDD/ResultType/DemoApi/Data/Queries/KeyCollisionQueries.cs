using Microsoft.EntityFrameworkCore;

namespace DemoApi.Data.Queries;

internal static class KeyCollisionQueries
{
    internal static async Task<(string? collidingKey, IEnumerable<string> similarKeys)> FindCollisions(this IQueryable<string> keys, string candidateKey)
    {
        string? collidingKey = await keys.FirstOrDefaultAsync(key => key == candidateKey);
        if (collidingKey is null) return (null, Enumerable.Empty<string>());

        List<string> similarKeys = await keys.Where(key => key.StartsWith(candidateKey) && key.Length > candidateKey.Length).ToListAsync();
        return (collidingKey, similarKeys);
    }
}