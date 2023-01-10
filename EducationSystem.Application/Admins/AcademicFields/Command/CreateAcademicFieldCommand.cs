using EducationSystem.Application.Common.Excensions;
using EducationSystem.Application.Common.Interfaces;
using EducationSystem.Domain.Entities;
using EducationSystem.Domain.Resources;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EducationSystem.Application.Admins.AcademicFields.Command
{
    #region command

    public record CreateAcademicFieldCommand(
        string Title,
        string Description,
        int AcademicBranchId) : IRequest<CreateAcademicFieldCommandResponse>;

    #endregion

    #region response

    public record CreateAcademicFieldCommandResponse(int Id);

    #endregion

    #region validator

    public class CreateAcademicFieldCommandValidator : AbstractValidator<CreateAcademicFieldCommand>
    {
        public CreateAcademicFieldCommandValidator()
        {
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

    public class CreateAcademicFieldCommandHandler : IRequestHandler<CreateAcademicFieldCommand, CreateAcademicFieldCommandResponse>
    {
        private readonly IAppDbContext _dbContext;

        public CreateAcademicFieldCommandHandler(IAppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<CreateAcademicFieldCommandResponse> Handle(CreateAcademicFieldCommand request, CancellationToken cancellationToken)
        {
            var isTitleDuplicated = await _dbContext.AcademicFields
                .AnyAsync(x => x.Title == request.Title);

            if (isTitleDuplicated)
            {
                throw new DuplicateException(Resource.DuplicateTitle);
            }

            var entity = new AcademicField
            {
                Title = request.Title,
                Description = request.Description,
                AcademicBranchId = request.AcademicBranchId
            };

            _dbContext.AcademicFields.Add(entity);

            await _dbContext.SaveChangesAsync();

            return new CreateAcademicFieldCommandResponse(entity.Id);
        }
    }

    #endregion
}
