using MediatR;
using Microsoft.EntityFrameworkCore;
using EducationSystem.Application.Common.Extensions;
using EducationSystem.Application.Common.Interfaces;
using EducationSystem.Application.Common.Models;

namespace EducationSystem.Application.Admins.Terms.Queries
{
    #region response

    public class TermListItem
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string CreateAt { get; set; }
    }

    #endregion

    #region query

    public record GetTermListQuery(string Title)  : PagingRequest, IRequest<PaginatedList<TermListItem>>;

    #endregion

    #region handle

    public class GetTermListQueryHandler : IRequestHandler<GetTermListQuery, PaginatedList<TermListItem>>
    {
        private readonly IAppDbContext _dbContext;

        public GetTermListQueryHandler(IAppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<PaginatedList<TermListItem>> Handle(GetTermListQuery request, CancellationToken cancellationToken)
        {
            var query = _dbContext.Terms
                .AsNoTracking();

            if(!string.IsNullOrEmpty(request.Title))
            {
                query = query.Where(x => x.Title.Contains(request.Title));
            }

            var result = await _dbContext.Terms
                .OrderBy(x => x.Id)
                .Select(x => new TermListItem
                {
                    Id = x.Id,
                    Title = x.Title,
                    StartDate = x.StartDate.Format("yyyy/MM/dd"),
                    EndDate = x.EndDate.Format("yyyy/MM/dd"),
                    CreateAt = x.CreatedAt.Format("yyyy/MM/dd")
                }).ApplyPagingAsync(request.Page, request.PageSize);

            return result;
        }
    }

    #endregion
}
