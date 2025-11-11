using Post.Query.Domain.Entities;
using Post.Query.Domain.Repositories;
using Post.Query.Infrastructure.DataAccess;

namespace Post.Query.Infrastructure.Repositories;

public class CommentRepository(DatabaseContextFactory databaseContextFactory): ICommentRepository
{
    public async Task CreateAsync(CommentEntity comment)
    {
        await using var databaseContext = databaseContextFactory.CreateDbContext();
        databaseContext.Comments.Add(comment);
        await databaseContext.SaveChangesAsync();      
    }

    public async Task UpdateAsync(CommentEntity comment)
    {
        await using var databaseContext = databaseContextFactory.CreateDbContext();
        databaseContext.Comments.Update(comment);
        await databaseContext.SaveChangesAsync();      
    }

    public async Task<CommentEntity?> GetByIdAsync(Guid commentId)
    {
        await using var databaseContext = databaseContextFactory.CreateDbContext();
        return databaseContext.Comments.FirstOrDefault(c => c.CommentId == commentId);  
    }

    public async Task DeleteAsync(Guid commentId)
    {
        await using var databaseContext = databaseContextFactory.CreateDbContext();
        var comment = await GetByIdAsync(commentId);
        if (comment == null) return;
        databaseContext.Comments.Remove(comment);
        await databaseContext.SaveChangesAsync();      
    }
}