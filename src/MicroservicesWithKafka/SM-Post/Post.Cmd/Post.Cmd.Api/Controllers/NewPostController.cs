using CQRS.Core.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Post.Cmd.Api.Commands;

namespace Post.Cmd.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class NewPostController : ControllerBase
{
    private readonly ILogger<NewPostController> _logger;
    private readonly ICommandDispatcher _cmdDispatcher;

    public NewPostController(ILogger<NewPostController> logger, ICommandDispatcher cmdDispatcher)
    {
        _logger = logger;
        _cmdDispatcher = cmdDispatcher;
    }
    
    
}