using MediatR;
using Microsoft.EntityFrameworkCore;
using EducationSystem.Application.Common.Interfaces;

namespace EducationSystem.Application.Admins.Terms.Queries
{
    #region response

    public class TermSelectListItem
    {
        public int Value { get; set; }
        public string Lable { get; set; }
    }

    #endregion

    #region query

    public record GetTermSelectListQuery : IRequest<List<TermSelectListItem>>;

    #endregion

    #region handler

    public class GetTermSelectListQueryHandler : IRequestHandler<GetTermSelectListQuery, List<TermSelectListItem>>
    {
        private readonly IAppDbContext _dbContext;

        public GetTermSelectListQueryHandler(IAppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<TermSelectListItem>> Handle(GetTermSelectListQuery request, CancellationToken cancellationToken)
        {
            var result = await _dbContext.Terms
                .OrderByDescending(x => x.CreatedAt)
                .Select(x => new TermSelectListItem
                {
                    Value = x.Id,
                    Lable = x.Title
                }).ToListAsync();

            return result;
        }
    }

    #endregion
}
