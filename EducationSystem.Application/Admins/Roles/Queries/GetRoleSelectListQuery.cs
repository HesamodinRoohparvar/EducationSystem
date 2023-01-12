using EducationSystem.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EducationSystem.Application.Admins.Roles.Queries
{
    #region response

    public class RoleSelectListItem
    {
        public int Value { get; set; }
        public string Lable { get; set; }

    }

    #endregion

    #region query

    public record GetRoleSelectListQuery : IRequest<List<RoleSelectListItem>>;

    #endregion

    #region handler

    public class GetRoleSelectListQueryHandler : IRequestHandler<GetRoleSelectListQuery, List<RoleSelectListItem>>
    {
        private readonly IAppDbContext _dbContext;

        public GetRoleSelectListQueryHandler(IAppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<RoleSelectListItem>> Handle(GetRoleSelectListQuery request, CancellationToken cancellationToken)
        {
            var result = await _dbContext.Roles
                .AsNoTracking()
                .OrderByDescending(x => x.CreatedAt)
                .Select(x => new RoleSelectListItem
                {
                    Value = x.Id,
                    Lable = x.Title
                }).ToListAsync();

            return result;
        }
    }

    #endregion
}
