using CQRS.Core.Commnds;

namespace Post.Cmd.Api.Commands
{
    public class EditPostCommand : BaseCommand
    {
        public string? Message { get; set; }
    }
}
