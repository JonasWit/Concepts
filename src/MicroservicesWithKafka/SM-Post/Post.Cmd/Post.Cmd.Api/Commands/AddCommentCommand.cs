using CQRS.Core.Commnds;

namespace Post.Cmd.Api.Commands;

public class AddCommentCommand : BaseCommand
{
    public string Comment { get; set; } = "";
    public string Username { get; set; } = "";
}