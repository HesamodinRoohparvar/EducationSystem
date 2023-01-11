using EducationSystem.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EducationSystem.Application.Common.Extensions
{
    public static class PagingExtensions
    {
        public static Task<PaginatedList<TDestination>> ApplyPagingAsync<TDestination>(this IQueryable<TDestination> queryable, int page, int pageSize) where TDestination : class
            => PaginatedList<TDestination>.CreateAsync(queryable, page, pageSize);
    }
}
