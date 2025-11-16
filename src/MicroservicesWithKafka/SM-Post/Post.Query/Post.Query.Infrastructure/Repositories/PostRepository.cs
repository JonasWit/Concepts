using Microsoft.EntityFrameworkCore;
using Post.Query.Domain.Entities;
using Post.Query.Domain.Repositories;
using Post.Query.Infrastructure.DataAccess;

namespace Post.Query.Infrastructure.Repositories;

public class PostRepository(DatabaseContextFactory databaseContextFactory) : IPostRepository
{
    public async Task CreateAsync(PostEntity post)
    {
        await using var databaseContext = databaseContextFactory.CreateDbContext();
        databaseContext.Posts.Add(post);
        await databaseContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(PostEntity post)
    {
        await using var databaseContext = databaseContextFactory.CreateDbContext();
        databaseContext.Posts.Update(post);
        await databaseContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid postId)
    {
        await using var databaseContext = databaseContextFactory.CreateDbContext();
        var post = await GetByIdAsync(postId);
        if (post == null)
        {
            return;
        }

        databaseContext.Posts.Remove(post);
        await databaseContext.SaveChangesAsync();
    }

    public async Task<PostEntity?> GetByIdAsync(Guid postId)
    {
        await using var databaseContext = databaseContextFactory.CreateDbContext();
        return await databaseContext.Posts.Include(p => p.Comments)
            .FirstOrDefaultAsync(p => p.PostId == postId);
    }

    public async Task<List<PostEntity>> ListAllAsync(int pageNumber = 0, int pageSize = 0)
    {
        await using var databaseContext = databaseContextFactory.CreateDbContext();
        var query = databaseContext.Posts.AsNoTracking();

        if (pageNumber > 0 && pageSize > 0)
        {
            var skip = (pageNumber - 1) * pageSize;
            query = query.Skip(skip).Take(pageSize);
        }

        query = query.Include(p => p.Comments);
        return await query.ToListAsync();
    }

    public async Task<List<PostEntity>> ListByAuthorAsync(string author)
    {
        await using var databaseContext = databaseContextFactory.CreateDbContext();
        return await databaseContext.Posts.AsNoTracking()
            .Include(p => p.Comments)
            .Where(p => p.Author == author)
            .ToListAsync();
    }

    public async Task<List<PostEntity>> ListWithLikesAsync(int numberOfLikes)
    {
        await using var databaseContext = databaseContextFactory.CreateDbContext();
        return await databaseContext.Posts.AsNoTracking()
            .Include(p => p.Comments)
            .Where(p => p.Likes >= numberOfLikes)
            .ToListAsync();
    }

    public async Task<List<PostEntity>> ListWithCommentsAsync()
    {
        await using var databaseContext = databaseContextFactory.CreateDbContext();
        return await databaseContext.Posts.AsNoTracking()
            .Include(p => p.Comments)
            .Where(p => p.Comments.Count > 0)
            .ToListAsync();
    }
}