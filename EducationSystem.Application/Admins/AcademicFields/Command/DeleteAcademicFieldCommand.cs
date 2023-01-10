using EducationSystem.Application.Common.Exceptions;
using EducationSystem.Application.Common.Interfaces;
using EducationSystem.Domain.Resources;
using FluentValidation;
using MediatR;

namespace EducationSystem.Application.Admins.AcademicFields.Command
{
    #region command

    public record DeleteAcademicFieldCommand(int Id): IRequest;

    #endregion

    #region validator

    public class DeleteAcademicFieldCommandValidator : AbstractValidator<DeleteAcademicFieldCommand>
    {
        public DeleteAcademicFieldCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithName(Resource.AcademicFieldId);
        }
    }

    #endregion

    #region handler

    public class DeleteAcademicFieldCommandHandler : IRequestHandler<DeleteAcademicFieldCommand, Unit>
    {
        private readonly IAppDbContext _dbContext;

        public DeleteAcademicFieldCommandHandler(IAppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Unit> Handle(DeleteAcademicFieldCommand request, CancellationToken cancellationToken)
        {
            var entity = await _dbContext.AcademicFields
                .FindAsync(request.Id);

            if(entity == null)
            {
                throw new NotFoundException(Resource.AcademicFieldNotFound);
            }

            _dbContext.AcademicFields.Remove(entity);

            await _dbContext.SaveChangesAsync();

            return Unit.Value;
        }
    }

    #endregion
}
