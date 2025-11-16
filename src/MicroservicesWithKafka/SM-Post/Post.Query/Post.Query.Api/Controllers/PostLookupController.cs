using System;
using CQRS.Core.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Post.Common.DTOs;
using Post.Query.Api.DTOs;
using Post.Query.Api.Queries;
using Post.Query.Domain.Entities;

namespace Post.Query.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class PostLookupController : ControllerBase
{
    private readonly ILogger<PostLookupController> _logger;
    private readonly IQueryDispatcher<PostEntity> _queryDispatcher;

    public PostLookupController(ILogger<PostLookupController> logger, IQueryDispatcher<PostEntity> queryDispatcher)
    {
        _logger = logger;
        _queryDispatcher = queryDispatcher;
    }

    [HttpGet]
    public async Task<ActionResult> GetAllPostsAsync([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        try
        {
            var posts = await _queryDispatcher.SendAsync(new FindAllPostsQuery(pageNumber, pageSize));
            return NormalResponse(posts);
        }
        catch (Exception ex)
        {
            const string SAFE_ERROR_MESSAGE = "Error while processing request to retrieve posts";
            _logger.LogError(ex, SAFE_ERROR_MESSAGE);
            return StatusCode(StatusCodes.Status500InternalServerError, new BaseResponse()
            {
                Message = SAFE_ERROR_MESSAGE
            });
        }
    }


    [HttpGet("byId/{postId}")]
    public async Task<ActionResult> GetByPostIdAsync(Guid postId)
    {
        try
        {
            var posts = await _queryDispatcher.SendAsync(new FindPostByIdQuery() { Id = postId });
            return NormalResponse(posts);
        }
        catch (Exception ex)
        {
            const string SAFE_ERROR_MESSAGE = "Error while processing request to retrieve post by ID";
            _logger.LogError(ex, SAFE_ERROR_MESSAGE);
            return StatusCode(StatusCodes.Status500InternalServerError, new BaseResponse()
            {
                Message = SAFE_ERROR_MESSAGE
            });
        }
    }

    [HttpGet("byAuthor/{author}")]
    public async Task<ActionResult> GetByPostAuthorAsync(string author)
    {
        try
        {
            var posts = await _queryDispatcher.SendAsync(new FindPostsByAuthor() { Author = author });
            return NormalResponse(posts);
        }
        catch (Exception ex)
        {
            const string SAFE_ERROR_MESSAGE = "Error while processing request to retrieve post by Author";
            _logger.LogError(ex, SAFE_ERROR_MESSAGE);
            return StatusCode(StatusCodes.Status500InternalServerError, new BaseResponse()
            {
                Message = SAFE_ERROR_MESSAGE
            });
        }
    }

    [HttpGet("withComments")]
    public async Task<ActionResult> GetByPostsWithCommentsAsync()
    {
        try
        {
            var posts = await _queryDispatcher.SendAsync(new FindPostsWithCommentsQuery());
            return NormalResponse(posts);
        }
        catch (Exception ex)
        {
            const string SAFE_ERROR_MESSAGE = "Error while processing request to retrieve posts with comments";
            _logger.LogError(ex, SAFE_ERROR_MESSAGE);
            return StatusCode(StatusCodes.Status500InternalServerError, new BaseResponse()
            {
                Message = SAFE_ERROR_MESSAGE
            });
        }
    }

    [HttpGet("withlikes/{numberOfLikes}")]
    public async Task<ActionResult> GetByPostsWithCommentsAsync(int numberOfLikes)
    {
        try
        {
            var posts = await _queryDispatcher.SendAsync(new FindPostsWithLikesQuery() { NumberOfLikes = numberOfLikes });
            return NormalResponse(posts);
        }
        catch (Exception ex)
        {
            const string SAFE_ERROR_MESSAGE = "Error while processing request to retrieve posts with likes";
            _logger.LogError(ex, SAFE_ERROR_MESSAGE);
            return StatusCode(StatusCodes.Status500InternalServerError, new BaseResponse()
            {
                Message = SAFE_ERROR_MESSAGE
            });
        }
    }

    private ActionResult NormalResponse(List<PostEntity> posts)
    {
        if (posts == null || posts.Count == 0)
        {
            return NoContent();
        }

        var count = posts.Count;
        return Ok(new PostLookupResponse
        {
            Posts = posts,
            Message = $"Successfully returned posts with comments"
        });
    }
}
