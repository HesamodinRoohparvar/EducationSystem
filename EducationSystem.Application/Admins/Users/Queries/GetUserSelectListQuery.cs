using EducationSystem.Application.Common.Constans;
using EducationSystem.Application.Common.Enumerations;
using EducationSystem.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EducationSystem.Application.Admins.Users.Queries
{
    #region response

    public class UserSelectListItem
    {
        public string Lable { get; set; }
        public int Value { get; set; }
    }

    #endregion

    #region query

    public record GetUserSelectListQuery(RoleFilter Role) : IRequest<List<UserSelectListItem>>;

    #endregion

    #region handler

    public class GetUserSelectListQueryHandler : IRequestHandler<GetUserSelectListQuery, List<UserSelectListItem>>
    {
        private readonly IAppDbContext _dbContext;

        public GetUserSelectListQueryHandler(IAppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<UserSelectListItem>> Handle(GetUserSelectListQuery request, CancellationToken cancellationToken)
        {
            var query = _dbContext.Users
                .Include(x => x.Role)
                .OrderBy(x => x.Id)
                .AsNoTracking();

            if(request.Role == RoleFilter.Admin)
            {
                query = query.Where(x => x.Role.Title == DefaultRoleNames.Admin);
            }

            if (request.Role == RoleFilter.Teacher)
            {
                query = query.Where(x => x.Role.Title == DefaultRoleNames.Teacher);
            }

            if (request.Role == RoleFilter.Student)
            {
                query = query.Where(x => x.Role.Title == DefaultRoleNames.Student);
            }

            var result = await query
                .Select(x => new UserSelectListItem
                {
                    Value = x.Id,
                    Lable = string.Join("", x.FirsName, x.LastName)
                }).ToListAsync();

            return result;
        }
    }

    #endregion
}
