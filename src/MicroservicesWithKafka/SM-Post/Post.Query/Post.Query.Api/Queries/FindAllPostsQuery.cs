using System;
using CQRS.Core.Queries;

namespace Post.Query.Api.Queries;

public class FindAllPostsQuery : BaseQuery
{
    public FindAllPostsQuery(int pageNumber, int pageSize)
    {
        PageNumber = pageNumber;
        PageSize = pageSize;
    }

    public int PageNumber { get; set; }
    public int PageSize { get; set; }
}
