using EducationSystem.Application.Common.Interfaces;
using EducationSystem.Domain.Entities;
using EducationSystem.Domain.Resources;
using FluentValidation;
using MediatR;

namespace EducationSystem.Application.Admins.Roles.Command
{
    #region command

    public record CreateRoleCommand(
        string Title,
        string Description,
        List<string> PermissionIds) : IRequest<CreateRoleCommandResponse>;

    #endregion

    #region response

    public record CreateRoleCommandResponse(int Id);

    #endregion

    #region validator

    public class CreateRoleCommandValidator : AbstractValidator<CreateRoleCommand>
    {
        public CreateRoleCommandValidator()
        {
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

    public class CreateRoleCommandHandler : IRequestHandler<CreateRoleCommand, CreateRoleCommandResponse>
    {
        private readonly IAppDbContext _dbContext;

        public CreateRoleCommandHandler(IAppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<CreateRoleCommandResponse> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public List<RolePermission> CreateRolePermissionList(List<string> permissionIds)
        {
            var rolePermissions = new List<RolePermission>();

            foreach(var permissionId in permissionIds)
            {
                var splitedPermissionId = permissionId.Split(":");

                if(splitedPermissionId.Length == 3)
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
