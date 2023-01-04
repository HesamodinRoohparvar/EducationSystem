using EducationSystem.Application.Common.Exceptions;
using EducationSystem.Application.Common.Interfaces;
using EducationSystem.Domain.Resources;
using FluentValidation;
using MediatR;

namespace EducationSystem.Application.Admins.Users.Commands
{
    #region command

    public record DeleteUserCommand(int Id) : IRequest;

    #endregion

    #region validator

    public class DeleteUserCommandValidator : AbstractValidator<DeleteUserCommand>
    {
        public DeleteUserCommandValidator()
        {
            RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithName(Resource.UserId);
        }
    }

    #endregion

    #region handler

    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, Unit>
    {
        private readonly IAppDbContext _dbContext;

        public DeleteUserCommandHandler(IAppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Unit> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var entity = await _dbContext.Users
                .FindAsync(request.Id);

            if(entity == null)
            {
                throw new NotFoundException(Resource.UserNotFound);
            }

            _dbContext.Users.Remove(entity);

            await _dbContext.SaveChangesAsync();

            return Unit.Value;
        }
    }

    #endregion
}
