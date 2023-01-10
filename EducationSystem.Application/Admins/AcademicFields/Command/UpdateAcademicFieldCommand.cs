using EducationSystem.Application.Common.Excensions;
using EducationSystem.Application.Common.Exceptions;
using EducationSystem.Application.Common.Interfaces;
using EducationSystem.Domain.Resources;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EducationSystem.Application.Admins.AcademicFields.Command
{
    #region command

    public record UpdateAcademicFieldCommand(
        int Id,
        string Title,
        string Description,
        int AcademicBranchId) : IRequest;

    #endregion

    #region validator

    public class UpdateAcademicFieldCommandValidator : AbstractValidator<UpdateAcademicFieldCommand>
    {
        public UpdateAcademicFieldCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithName(Resource.AcademicFieldId);

            RuleFor(x => x.Title)
                .NotEmpty()
                .MaximumLength(25)
                .WithName(Resource.Title);

            RuleFor(x => x.Description)
                .MaximumLength(250)
                .WithName(Resource.Description);
        }
    }

    #endregion

    #region handler

    public class UpdateAcademicFieldCommandHandler : IRequestHandler<UpdateAcademicFieldCommand, Unit>
    {
        private readonly IAppDbContext _dbContext;

        public UpdateAcademicFieldCommandHandler(IAppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Unit> Handle(UpdateAcademicFieldCommand request, CancellationToken cancellationToken)
        {
            var entity = await _dbContext.AcademicFields
                .Where(x => x.Id == request.Id)
                .SingleOrDefaultAsync();

            if(entity == null)
            {
                throw new NotFoundException(Resource.AcademicFieldNotFound);
            }

            if (entity.Title != request.Title)
            {
                var isTitleDuplicated = await _dbContext.AcademicFields
                    .AnyAsync(x => x.Title == request.Title);

                if (isTitleDuplicated)
                {
                    throw new DuplicateException(Resource.DuplicateTitle);
                }
            }


            entity.Title = request.Title;
            entity.Description = request.Description;
            entity.AcademicBranchId = request.AcademicBranchId;

            await _dbContext.SaveChangesAsync();

            return Unit.Value;
        }
    }

    #endregion
}
