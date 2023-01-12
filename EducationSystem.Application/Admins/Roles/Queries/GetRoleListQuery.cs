using EducationSystem.Application.Common.Extensions;
using EducationSystem.Application.Common.Interfaces;
using EducationSystem.Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EducationSystem.Application.Admins.Roles.Queries
{
    #region response

    public class RoleListItem
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int UserCount { get; set; }
        public int PermissionCount { get; set; }
        public string CreatedAtDate { get; set; }
        public string CreatedAtTime { get; set; }
    }

    #endregion

    #region query

    public record GetRoleListQuery(string Title) : PagingRequest, IRequest<PaginatedList<RoleListItem>>;

    #endregion

    #region handler

    public class GetRoleListQueryHandler : IRequestHandler<GetRoleListQuery, PaginatedList<RoleListItem>>
    {
        private readonly IAppDbContext _dbContext;

        public GetRoleListQueryHandler(IAppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<PaginatedList<RoleListItem>> Handle(GetRoleListQuery request, CancellationToken cancellationToken)
        {
            var query = _dbContext.Roles
                .Include(x => x.Users)
                .Include(x => x.RolePermissions)
                .AsNoTracking();

            if (!string.IsNullOrEmpty(request.Title))
            {
                query = query.Where(x => x.Title.Contains(request.Title));
            }

            var result = await query
                .OrderByDescending(x => x.CreatedAt)
                .Select(x => new RoleListItem
                {
                    Id = x.Id,
                    Title = x.Title,
                    Description = x.Description,
                    UserCount = x.Users.Count(),
                    PermissionCount = x.RolePermissions.Count(),
                    CreatedAtDate = x.CreatedAt.Format("yyyy/MM/dd"),
                    CreatedAtTime = x.CreatedAt.Format("hh:mm:ss")
                }).ApplyPagingAsync(request.Page, request.PageSize);

            return result;
        }
    }

    #endregion
}
