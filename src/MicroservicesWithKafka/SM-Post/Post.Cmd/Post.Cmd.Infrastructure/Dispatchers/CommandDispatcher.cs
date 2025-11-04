using CQRS.Core.Commnds;
using CQRS.Core.Infrastructure;

namespace Post.Cmd.Infrastructure.Dispatchers;

public class CommandDispatcher : ICommandDispatcher
{
    private readonly Dictionary<Type, Func<BaseCommand, Task>> _handlers = [];

    public void RegisterHandler<T>(Func<T, Task> handler) where T : BaseCommand
    {
        throw new NotImplementedException();
    }

    public Task SendAsync(BaseCommand command)
    {
        throw new NotImplementedException();
    }
}
