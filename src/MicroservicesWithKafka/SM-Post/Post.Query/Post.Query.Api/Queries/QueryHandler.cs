using System;
using Post.Query.Domain.Entities;
using Post.Query.Domain.Repositories;

namespace Post.Query.Api.Queries;

public class QueryHandler : IQueryHandler
{
    private readonly IPostRepository _postRepository;

    public QueryHandler(IPostRepository postRepository)
    {
        _postRepository = postRepository;
    }

    public async Task<List<PostEntity>> HandleAsync(FindAllPostsQuery query) =>
    await _postRepository.ListAllAsync();

    public async Task<List<PostEntity>> HandleAsync(FindPostByIdQuery query)
    {
        var post = await _postRepository.GetByIdAsync(query.Id);
        return [post];
    }

    public Task<List<PostEntity>> HandleAsync(FindPostsByAuthor query)
    {
        throw new NotImplementedException();
    }

    public Task<List<PostEntity>> HandleAsync(FindPostsWithCommentsQuery query)
    {
        throw new NotImplementedException();
    }

    public Task<List<PostEntity>> HandleAsync(FindPostsWithLikesQuery query)
    {
        throw new NotImplementedException();
    }
}
