using Microsoft.EntityFrameworkCore;
using NSubstitute;

namespace TestConcepts.Tests;

public static class DbSetMockHelper
{
    public static DbSet<T> CreateMockDbSet<T>(List<T>? initialData = null) where T : class
    {
        var data = initialData ?? [];
        var mockSet = Substitute.For<DbSet<T>, IQueryable<T>>();

        mockSet.When(x => x.Add(Arg.Any<T>()))
            .Do(x => data.Add(x.Arg<T>()));

        mockSet.When(x => x.AddRange(Arg.Any<T[]>()))
            .Do(x => data.AddRange(x.Arg<T[]>()));

        mockSet.When(x => x.AddRange(Arg.Any<IEnumerable<T>>()))
            .Do(x => data.AddRange(x.Arg<IEnumerable<T>>()));

        mockSet.When(x => x.Remove(Arg.Any<T>()))
            .Do(x => data.Remove(x.Arg<T>()));

        mockSet.When(x => x.RemoveRange(Arg.Any<IEnumerable<T>>()))
            .Do(x =>
            {
                foreach (var item in x.Arg<IEnumerable<T>>())
                {
                    data.Remove(item);
                }
            });

        mockSet.When(x => x.Update(Arg.Any<T>()))
            .Do(x =>
            {
                var item = x.Arg<T>();
                var existingIndex = data.IndexOf(item);
                if (existingIndex >= 0)
                {
                    data[existingIndex] = item;
                }
            });

        UpdateQueryableProvider(mockSet, data);

        return mockSet;
    }

    private static void UpdateQueryableProvider<T>(DbSet<T> mockSet, List<T> data) where T : class
    {
        ((IQueryable<T>)mockSet).Provider.Returns(callInfo => data.AsQueryable().Provider);
        ((IQueryable<T>)mockSet).Expression.Returns(callInfo => data.AsQueryable().Expression);
        ((IQueryable<T>)mockSet).ElementType.Returns(data.AsQueryable().ElementType);
        ((IQueryable<T>)mockSet).GetEnumerator().Returns(callInfo => data.GetEnumerator());
    }
}
