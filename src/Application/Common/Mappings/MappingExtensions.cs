using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventSourcingExample.Application.Common.Models;

namespace EventSourcingExample.Application.Common.Mappings
{
    public static class MappingExtensions
    {
        public static Task<PaginatedList<TDestination>> ToPaginatedListAsync<TDestination>(
            this IQueryable<TDestination> queryable,
            int pageNumber,
            int pageSize)
        {
            return PaginatedList<TDestination>.CreateAsync(queryable, pageNumber, pageSize);
        }

        public async static Task<PaginatedList<TDestination>> ToPaginatedListAsync<TDestination>(
            this IQueryable<TDestination> queryable,
            Pager pager)
            where TDestination : class
        {
            var result = await queryable.Paginate(pager).ToListAsync();
            return new PaginatedList<TDestination>(result, pager.TotalRows, pager.Index, pager.Size);
        }

        public static Task<List<TDestination>> ProjectToListAsync<TDestination>(
            this IQueryable queryable,
            IConfigurationProvider configuration)
        {
            return queryable
                .ProjectTo<TDestination>(configuration)
                .ToListAsync();
        }
    }
}
