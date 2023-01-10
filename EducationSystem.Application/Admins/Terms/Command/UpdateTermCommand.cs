using EducationSystem.Application.Common.Excensions;
using EducationSystem.Application.Common.Exceptions;
using EducationSystem.Application.Common.Interfaces;
using EducationSystem.Domain.Resources;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EducationSystem.Application.Admins.Terms.Command
{
    #region command

    public record UpdateTermCommand(
        int Id,
        string Title,
        string Description) : IRequest;

    #endregion

    #region validator

    public class UpdateTermCommandValidator : AbstractValidator<UpdateTermCommand>
    {
        public UpdateTermCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithName(Resource.TermId);

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

    public class UpdateTermCommandHandler : IRequestHandler<UpdateTermCommand, Unit>
    {
        private readonly IAppDbContext _dbContext;
        public UpdateTermCommandHandler(IAppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Unit> Handle(UpdateTermCommand request, CancellationToken cancellationToken)
        {
            var entity = await _dbContext.Terms
                .Where(x => x.Id == request.Id)
                .SingleOrDefaultAsync();

            if(entity == null)
            {
                throw new NotFoundException(Resource.TermNotFound);
            }

            if(entity.Title != request.Title)
            {
                var isTitleDulicated = await _dbContext.Terms
                    .AnyAsync(x => x.Title == request.Title);

                if (isTitleDulicated)
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
