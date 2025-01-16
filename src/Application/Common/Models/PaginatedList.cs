using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventSourcingExample.Application.Common.Models
{
    public class PaginatedList<T>
    {
        public List<T> Items { get; }
        public int PageIndex { get; }
        public int TotalPages { get; }
        public int TotalCount { get; }

        public PaginatedList(List<T> items, int totalCount, int pageIndex, int pageSize)
        {
            PageIndex = pageIndex;
            TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
            TotalCount = totalCount;
            Items = items;
        }

        public bool HasPreviousPage => PageIndex > 1;

        public bool HasNextPage => PageIndex < TotalPages;

        /// <summary>
        /// Creates PaginatedList
        /// </summary>
        public static async Task<PaginatedList<T>> CreateAsync(IQueryable<T> source, int pageIndex, int pageSize)
        {
            var count = await source.CountAsync();

            var items = await source
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PaginatedList<T>(items, count, pageIndex, pageSize);
        }
    }
}
