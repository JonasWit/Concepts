using System;
using CQRS.Core.Infrastructure;
using CQRS.Core.Queries;
using Post.Query.Domain.Entities;

namespace Post.Query.Infrastructure.Dispatchers;

public class QueryDispatcher : IQueryDispatcher<PostEntity>
{
    private readonly Dictionary<Type, Func<BaseQuery, Task<List<PostEntity>>>> _handers = [];

    public void RegisterHandler<TQuery>(Func<TQuery, Task<List<PostEntity>>> handler) where TQuery : BaseQuery
    {
        if (_handers.ContainsKey(typeof(TQuery)))
        {
            throw new IndexOutOfRangeException("Quary is already registered!");
        }
        else
        {
            _handers.Add(typeof(TQuery), h => handler((TQuery)h));
        }
    }

    public async Task<List<PostEntity>> SendAsync(BaseQuery query)
    {
        if (_handers.TryGetValue(query.GetType(), out var handler))
        {
            return await handler(query);
        }
        else
        {
            throw new ArgumentNullException(nameof(handler), "Query handler was not registered");
        }
    }
}
