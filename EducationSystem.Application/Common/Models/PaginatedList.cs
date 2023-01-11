using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EducationSystem.Application.Common.Models
{
    public class PaginatedList<T>
    {
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public int TotalItems { get; set; }
        public int CurrentPage { get; set; }
        public bool IsLastPage { get; set; }
        public bool IsFirstPage { get; set; }
        public bool HasNextPage { get; set; }
        public List<T> Items { get; set; }
        public bool HasPreviousPage { get; set; }
        public int TotalFilteredItems { get; set; }

        public static async Task<PaginatedList<T>> CreateAsync(IQueryable<T> source, int page, int pageSize)
        {
            page = page <= 0 ? 1 : page;
            pageSize = pageSize <= 0 ? 10 : pageSize;
            var totalItems = await source.CountAsync();
            source = source ?? new List<T>().AsQueryable();

            var result = new PaginatedList<T>
            {
                CurrentPage = page,
                PageSize = pageSize,
                TotalItems = totalItems,
                TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize),
                Items = await source.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync(),
            };

            result.TotalFilteredItems = result.Items.Count();

            result.IsFirstPage = result.CurrentPage == 1;
            result.IsLastPage = result.CurrentPage == result.TotalPages;

            result.HasNextPage = result.CurrentPage < result.TotalPages;
            result.HasPreviousPage = result.CurrentPage > 1;

            return result;
        }
    }
}
