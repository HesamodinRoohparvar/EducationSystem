using EducationSystem.Application.Common.Constans;
using EducationSystem.Application.Common.Excensions;
using EducationSystem.Application.Common.Exceptions;
using EducationSystem.Application.Common.Interfaces;
using EducationSystem.Domain.Entities;
using EducationSystem.Domain.Resources;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EducationSystem.Application.Admins.Roles.Command
{
    #region command

    public record UpdateRoleCommand(
        int Id,
        string Title,
        string Description,
        List<string> PermissionIds) :IRequest;

    #endregion

    #region validator

    public class UpdateRoleCommandValidator : AbstractValidator<UpdateRoleCommand>
    {
        public UpdateRoleCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithName(Resource.RoleId);

            RuleFor(x => x.Title)
                .NotEmpty()
                .MaximumLength(50)
                .WithName(Resource.Title);

            RuleFor(x => x.Description)
                .MaximumLength(250)
                .WithName(Resource.Description);
        }
    }

    #endregion

    #region handler

    public class UpdateRoleCommandhandler : IRequestHandler<UpdateRoleCommand, Unit>
    {
        private readonly IAppDbContext _dbContext;

        public UpdateRoleCommandhandler(IAppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Unit> Handle(UpdateRoleCommand request, CancellationToken cancellationToken)
        {
            var entity = await _dbContext.Roles
                .Include(x => x.RolePermissions)
                .Where(x => x.Id == request.Id)
                .SingleOrDefaultAsync();

            if(entity == null)
            {
                throw new NotFoundException(Resource.RoleNotFound);
            }

            if (DefaultRoleNames.List.Contains(entity.Title))
            {
                throw new OperationNotAllowedException(Resource.AnyOperationsOnDefaultRolesAreNotAllowed);
            }

            if(entity.Title != request.Title)
            {
                var isTitleDuplicated = await _dbContext.Roles
                    .AnyAsync(x => x.Title == request.Title);

                if (isTitleDuplicated)
                {
                    throw new DuplicateException(Resource.DuplicateTitle);
                }
            }

            entity.Title = request.Title;
            entity.Description = request.Description;
            entity.RolePermissions = CreateRolePermissionList(request.PermissionIds);

            await _dbContext.SaveChangesAsync();

            return Unit.Value;
        }

        public List<RolePermission> CreateRolePermissionList(List<string> permissionIds)
        {
            var rolePermissions = new List<RolePermission>();

            foreach(var permissionId in permissionIds)
            {
                var splitedPermissionId = permissionId.Split(":");

                if (splitedPermissionId.Length == 3)
                {
                    var area = splitedPermissionId[0];
                    var controller = splitedPermissionId[1];
                    var action = splitedPermissionId[2];

                    rolePermissions.Add(new RolePermission
                    {
                        Area = area,
                        Action = action,
                        Controller = controller
                    });
                }
            }

            return rolePermissions;
        }
    }

    #endregion
}
