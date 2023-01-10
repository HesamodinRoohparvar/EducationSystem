using EducationSystem.Application.Common.Exceptions;
using EducationSystem.Application.Common.Interfaces;
using EducationSystem.Domain.Resources;
using FluentValidation;
using MediatR;

namespace EducationSystem.Application.Admins.Terms.Command
{
    #region command

    public record DeleteTermCommand(int Id) : IRequest;

    #endregion

    #region validator

    public class DeleteTermCommandValidator : AbstractValidator<DeleteTermCommand>
    {
        public DeleteTermCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithName(Resource.TermId);
        }
    }

    #endregion

    #region handler

    public class DeleteTermCommandHandler : IRequestHandler<DeleteTermCommand, Unit>
    {
        private readonly IAppDbContext _dbContext;

        public DeleteTermCommandHandler(IAppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Unit> Handle(DeleteTermCommand request, CancellationToken cancellationToken)
        {
            var entity = await _dbContext.Terms
                .FindAsync(request.Id);

            if(entity == null)
            {
                throw new NotFoundException(Resource.TermNotFound);
            }

            _dbContext.Terms.Remove(entity);

            await _dbContext.SaveChangesAsync();

            return Unit.Value;
        }
    }

    #endregion
}
