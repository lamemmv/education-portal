using EP.Data.Entities;
using System.Collections.Generic;

namespace EP.Data.Paginations
{
    public sealed class PagedList<TEntity> : IPagedList<TEntity> where TEntity : IEntity
    {
        private PagedList(int page, int size)
        {
        }

        private PagedList(long totalItems, IEnumerable<TEntity> items, int totalPages, int page, int size)
        {
            TotalItems = totalItems;
            Items = items;
            TotalPages = totalPages;
            Page = page;
            Size = size;
        }

        public IEnumerable<TEntity> Items { get; }

        public long TotalItems { get; }

        public int TotalPages { get; }

        public int Page { get; }

        public int Size { get; }

        public static IPagedList<TEntity> Empty(int page, int size)
        {
            return new PagedList<TEntity>(page, size);
        }

        public static IPagedList<TEntity> Create(long totalItems, IEnumerable<TEntity> items, int totalPages, int page, int size)
        {
            return new PagedList<TEntity>(totalItems, items, totalPages, page, size);
        }
    }
}
