using EP.Data.Entities;
using System.Collections.Generic;

namespace EP.Data.Paginations
{
    public interface IPagedList<TEntity> where TEntity : IEntity
    {
        IEnumerable<TEntity> Items { get; }

        long TotalItems { get; }

        int TotalPages { get; }

        int Page { get; }

        int Size { get; }
    }
}
