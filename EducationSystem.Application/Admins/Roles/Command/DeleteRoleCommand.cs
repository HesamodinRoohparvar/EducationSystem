using EducationSystem.Application.Common.Constans;
using EducationSystem.Application.Common.Exceptions;
using EducationSystem.Application.Common.Interfaces;
using EducationSystem.Domain.Resources;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EducationSystem.Application.Admins.Roles.Command
{
    #region command

    public record DeleteRoleCommand(int Id): IRequest;

    #endregion

    #region validator

    public class DeleteRoleCommandValidator : AbstractValidator<DeleteRoleCommand>
    {
        public DeleteRoleCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithName(Resource.RoleId);
        }
    }

    #endregion

    #region handler

    public class DeleteRoleCommandHandler : IRequestHandler<DeleteRoleCommand, Unit>
    {
        private readonly IAppDbContext _dbContext;

        public DeleteRoleCommandHandler(IAppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Unit> Handle(DeleteRoleCommand request, CancellationToken cancellationToken)
        {
            var entity = await _dbContext.Roles
                .FindAsync(request.Id);

            if(entity == null)
            {
                throw new NotFoundException(Resource.RoleNotFound);
            }

            if (DefaultRoleNames.List.Contains(entity.Title))
            {
                throw new OperationNotAllowedException(Resource.AnyOperationsOnDefaultRolesAreNotAllowed);
            }

            var isRoleContainUsers = await _dbContext.Users
                .AnyAsync(x=>x.RoleId == request.Id);

            if (isRoleContainUsers)
            {
                throw new OperationNotAllowedException(Resource.RoleContainsSomeUsers);
            }

            _dbContext.Roles.Remove(entity);

            await _dbContext.SaveChangesAsync();

            return Unit.Value;
        }
    }

    #endregion
}
