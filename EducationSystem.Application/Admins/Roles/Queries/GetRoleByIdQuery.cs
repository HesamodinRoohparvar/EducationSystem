using MediatR;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using EducationSystem.Domain.Resources;
using EducationSystem.Application.Common.Exceptions;
using EducationSystem.Application.Common.Extensions;
using EducationSystem.Application.Common.Interfaces;

namespace EducationSystem.Application.Admins.Roles.Queries
{
    #region query

    public record GetRoleByIdQuery(int Id) : IRequest<GetRoleByIdQueryResponse>;

    #endregion

    #region response

    public class GetRoleByIdQueryResponse
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int UserCount { get; set; }
        public int PermissionCount { get; set; }
        public string CreateAt { get; set; }
        public List<string> PermissionIds { get; set; }
    }

    #endregion

    #region validator

    public class GetRoleByIdQueryValidator : AbstractValidator<GetRoleByIdQuery>
    {
        public GetRoleByIdQueryValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithName(Resource.RoleId);
        }
    }

    #endregion

    #region handler

    public class GetRoleByIdQueryHandler : IRequestHandler<GetRoleByIdQuery, GetRoleByIdQueryResponse>
    {
        private readonly IAppDbContext _dbContext;

        public GetRoleByIdQueryHandler(IAppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<GetRoleByIdQueryResponse> Handle(GetRoleByIdQuery request, CancellationToken cancellationToken)
        {
            var result = await _dbContext.Roles
                .Include(x => x.RolePermissions)
                .Include(x => x.Users)
                .Where(x => x.Id == request.Id)
                .Select(x => new GetRoleByIdQueryResponse
                {
                    Id = x.Id,
                    Title = x.Title,
                    Description = x.Description,
                    UserCount = x.Users.Count(),
                    PermissionCount = x.RolePermissions.Count(),
                    CreateAt = x.CreatedAt.Format("yyyy/MM/dd hh:mm:ss"),
                    PermissionIds = x.RolePermissions.Select(x => string.Join(":", x.Area, x.Controller, x.Action)).ToList()
                }).SingleOrDefaultAsync();

            if(result == null)
            {
                throw new NotFoundException(Resource.RoleNotFound);
            }

            return result;
        }
    }

    #endregion
}
