using CQRS.Core.Commnds;
using CQRS.Core.Infrastructure;

namespace Post.Cmd.Infrastructure.Dispatchers;

public class CommandDispatcher : ICommandDispatcher
{
    private readonly Dictionary<Type, Func<BaseCommand, Task>> _handlers = [];

    public void RegisterHandler<T>(Func<T, Task> handler) where T : BaseCommand
    {
        if (_handlers.ContainsKey(typeof(T)))
            throw new IndexOutOfRangeException("You cannot register the same command twice");
        _handlers.Add(typeof(T), c => handler((T)c));
    }

    public async Task SendAsync(BaseCommand command)
    {
        if (_handlers.TryGetValue(command.GetType(), out var handler))
            await handler(command);
        else
            throw new ArgumentNullException(nameof(handler), "Handler not registered");
    }
}