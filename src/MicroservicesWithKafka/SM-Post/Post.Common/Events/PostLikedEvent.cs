using CQRS.Core.Events;

namespace Post.Common.Events;

public class PostLikedEvent() : BaseEvent(nameof(PostLikedEvent))
{
    public string? Author { get; set; }
    public string? Message { get; set; }
    public DateTime DatePosted { get; set; }
}