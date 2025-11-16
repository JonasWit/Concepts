using System;
using CQRS.Core.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Post.Cmd.Api.Commands;
using Post.Cmd.Api.DTOs;
using Post.Common.DTOs;

namespace Post.Cmd.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class RestoreReadDbController : ControllerBase
{

    private readonly ILogger<NewPostController> _logger;
    private readonly ICommandDispatcher _commandDispatcher;

    public RestoreReadDbController(ILogger<NewPostController> logger, ICommandDispatcher commandDispatcher)
    {
        _logger = logger;
        _commandDispatcher = commandDispatcher;
    }

    [HttpPost]
    public async Task<ActionResult> RestoreReadDbAsync()
    {
        try
        {
            await _commandDispatcher.SendAsync(new RestoreReadDbCommand());
            return StatusCode(StatusCodes.Status201Created, new BaseResponse()
            {
                Message = "Read database restored",
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
        catch (Exception ex)
        {
            const string safeErrorMessage = "Error while restoring read db";
            _logger.Log(LogLevel.Error, ex, safeErrorMessage);
            return StatusCode(StatusCodes.Status500InternalServerError, new BaseResponse()
            {
                Message = safeErrorMessage,
            });
        }
    }
}
