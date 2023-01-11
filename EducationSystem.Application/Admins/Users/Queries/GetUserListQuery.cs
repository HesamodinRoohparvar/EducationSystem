using EducationSystem.Application.Common.Constans;
using EducationSystem.Application.Common.Extensions;
using EducationSystem.Application.Common.Interfaces;
using EducationSystem.Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EducationSystem.Application.Admins.Users.Queries
{
    #region response

    public class UserListItem
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string FatherName { get; set; }
        public string UserName { get; set; }
        public string Photo { get; set; }
        public string IdentificationCode { get; set; }
        public string AcademicField { get; set; }
        public string GraduationDate { get; set; }
        public string RoleName { get; set; }
        public bool IsActive { get; set; }
        public string CreatedAtDate { get; set; }
        public string CreatedAtTime { get; set; }
        public string LastLoginAtDate { get; set; }
        public string LastLoginAtTime { get; set; }
    }

    #endregion

    #region query 

    public record GetUserListQuery(
        string Name,
        string RoleName,
        string UserName) : PagingRequest, IRequest<PaginatedList<UserListItem>>;

    #endregion

    #region handler

    public class GetUserListQueryHandler : IRequestHandler<GetUserListQuery, PaginatedList<UserListItem>>
    {
        private readonly IAppDbContext _dbContext;

        public GetUserListQueryHandler(IAppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<PaginatedList<UserListItem>> Handle(GetUserListQuery request, CancellationToken cancellationToken)
        {
            var query = _dbContext.Users
                .Include(x => x.Role)
                .AsNoTracking();

            if (!string.IsNullOrEmpty(request.Name))
            {
                query = query.Where(x =>
                    x.FirsName.Contains(request.Name) ||
                    x.LastName.Contains(request.Name));
            }

            if (!string.IsNullOrEmpty(request.RoleName))
            {
                query = query.Where(x => x.Role.Title.Contains(request.RoleName));
            }

            if (!string.IsNullOrEmpty(request.UserName))
            {
                query = query.Where(x => x.UserName.Contains(request.UserName));
            }

            var result = await query
                .OrderBy(x => x.Id)
                .Select(x => new UserListItem
                {
                    Id = x.Id,
                    FullName = string.Join("", x.FirsName, x.LastName),
                    IdentificationCode = x.IdentificationCode,
                    FatherName = x.FatherName,
                    UserName = x.UserName,
                    RoleName = x.Role.Title,
                    AcademicField = x.AcademicField.Title,
                    Photo = string.IsNullOrEmpty(x.Photo) ? Defaults.DefaultAvatarPath : string.Join("/", Defaults.HostUrl, x.Photo),
                    GraduationDate = x.GraduationDate.Format("yyyy/MM/dd"),
                    CreatedAtTime = x.CreatedAt.Format("hh:mm:ss"),
                    CreatedAtDate = x.CreatedAt.Format("yyyy/MM/dd"),
                    LastLoginAtTime = x.LastLoginAt.Format("hh:mm:ss"),
                    LastLoginAtDate = x.LastLoginAt.Format("yyyy/MM/dd"),
                    IsActive = x.IsActive
                }).ApplyPagingAsync(request.Page, request.PageSize);

            return result;
        }
    }

    #endregion
}
