using EducationSystem.Application.Common.Excensions;
using EducationSystem.Application.Common.Exceptions;
using EducationSystem.Application.Common.Interfaces;
using EducationSystem.Domain.Resources;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EducationSystem.Application.Admins.AcademicBranches.Command
{
    #region command

    public record UpdateAcademicBranchCommand(
        int Id,
        string Title,
        string Description) : IRequest;

    #endregion

    #region validator

    public class UpdateAcadmicBranchValidator : AbstractValidator<UpdateAcademicBranchCommand>
    {
        public UpdateAcadmicBranchValidator()
        {
            RuleFor(x => x.Id).
                GreaterThan(0)
                .WithName(Resource.AcademicBranchId);

            RuleFor(x => x.Title)
                .NotEmpty()
                .MaximumLength(25)
                .WithName(Resource.Title);

            RuleFor(x => x.Description)
                .NotEmpty()
                .MaximumLength(250)
                .WithName(Resource.Description);
        }
    }

    #endregion

    #region handler

    public class UpdateAcademicBranchCommandHandler : IRequestHandler<UpdateAcademicBranchCommand, Unit>
    {
        private readonly IAppDbContext _dbContext;

        public UpdateAcademicBranchCommandHandler(IAppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Unit> Handle(UpdateAcademicBranchCommand request, CancellationToken cancellationToken)
        {
            var entity = await _dbContext.AcademicBranches
                .Where(x => x.Id == request.Id)
                .SingleOrDefaultAsync();

            if (entity == null)
            {
                throw new NotFoundException(Resource.AcademicBranchNotFound);
            }

            if(entity.Title != request.Title)
            {
                var isTitleDuplicated = await _dbContext.AcademicBranches
                    .AnyAsync(x => x.Title == request.Title);

                if (isTitleDuplicated)
                {
                    throw new DuplicateException(Resource.DuplicateTitle);
                }
            }

            entity.Title = request.Title;
            entity.Description = request.Description;

            await _dbContext.SaveChangesAsync();

            return Unit.Value;
        }
    }

    #endregion
}
