using System;
using CQRS.Core.Exceptions;
using CQRS.Core.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Post.Cmd.Api.Commands;
using Post.Common.DTOs;

namespace Post.Cmd.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class AddCommentController : ControllerBase
{
    private readonly ILogger<AddCommentController> _logger;
    private readonly ICommandDispatcher _commandDispatcher;

    public AddCommentController(ILogger<AddCommentController> logger, ICommandDispatcher commandDispatcher)
    {
        _logger = logger;
        _commandDispatcher = commandDispatcher;
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> AddCommentAsync(Guid id, AddCommentCommand command)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(command.Username))
            {
                throw new InvalidOperationException("Username cannot be empty");
            }

            command.Id = id;
            await _commandDispatcher.SendAsync(command);
            return Ok(new BaseResponse()
            {
                Message = "Comemnt added successfully"
            });
        }
        catch (InvalidOperationException ex)
        {
            _logger.Log(LogLevel.Warning, ex, "Client made bad request");
            return BadRequest(new BaseResponse()
            {
                Message = ex.Message,
            });
        }
        catch (AggregateNotFoundException ex)
        {
            _logger.Log(LogLevel.Warning, ex, "Client made bad request, incorrect post id");
            return BadRequest(new BaseResponse()
            {
                Message = ex.Message,
            });
        }
        catch (Exception ex)
        {
            const string safeErrorMessage = "Error while adding comment";
            _logger.Log(LogLevel.Error, ex, safeErrorMessage);
            return StatusCode(StatusCodes.Status500InternalServerError, new BaseResponse()
            {
                Message = safeErrorMessage,
            });
        }
    }
}
