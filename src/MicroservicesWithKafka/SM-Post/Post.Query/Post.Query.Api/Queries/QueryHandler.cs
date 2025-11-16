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

    public async Task<List<PostEntity>> HandleAsync(FindAllPostsQuery query)
    {
        var posts = await _postRepository.ListAllAsync(query.PageNumber, query.PageSize);
        return posts;
    }

    public async Task<List<PostEntity>> HandleAsync(FindPostByIdQuery query)
    {
        var post = await _postRepository.GetByIdAsync(query.Id);
        return [post];
    }

    public Task<List<PostEntity>> HandleAsync(FindPostsByAuthor query) =>
        _postRepository.ListByAuthorAsync(query.Author);

    public async Task<List<PostEntity>> HandleAsync(FindPostsWithCommentsQuery query)
    {
        var posts = await _postRepository.ListWithCommentsAsync();
        return posts;
    }

    public async Task<List<PostEntity>> HandleAsync(FindPostsWithLikesQuery query)
    {
        var posts = await _postRepository.ListWithLikesAsync(query.NumberOfLikes);
        return posts;
    }
}
