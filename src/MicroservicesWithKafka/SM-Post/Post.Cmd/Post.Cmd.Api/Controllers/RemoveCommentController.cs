using CQRS.Core.Exceptions;
using CQRS.Core.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Post.Cmd.Api.Commands;
using Post.Common.DTOs;

namespace Post.Cmd.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class RemoveCommentController : ControllerBase
    {
        private readonly ILogger<RemoveCommentController> _logger;
        private readonly ICommandDispatcher _commandDispatcher;

        public RemoveCommentController(ILogger<RemoveCommentController> logger, ICommandDispatcher commandDispatcher)
        {
            _logger = logger;
            _commandDispatcher = commandDispatcher;
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> RemoveCommentAsync(Guid id, RemoveCommentCommand command)
        {
            try
            {
                command.Id = id;
                await _commandDispatcher.SendAsync(command);
                return Ok(new BaseResponse()
                {
                    Message = "Comment removed successfully"
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
                const string safeErrorMessage = "Error while removing comment";
                _logger.Log(LogLevel.Error, ex, safeErrorMessage);
                return StatusCode(StatusCodes.Status500InternalServerError, new BaseResponse()
                {
                    Message = safeErrorMessage,
                });
            }
        }
    }
}
