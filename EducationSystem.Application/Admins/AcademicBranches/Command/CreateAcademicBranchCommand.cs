using EducationSystem.Application.Common.Excensions;
using EducationSystem.Application.Common.Interfaces;
using EducationSystem.Domain.Entities;
using EducationSystem.Domain.Resources;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EducationSystem.Application.Admins.AcademicBranches.Command
{
    #region command

    public record CreateAcademicBranchCommand(
        string Title,
        string Description) : IRequest<CreateAcademicBranchCommandResponse>;

    #endregion

    #region response

    public record CreateAcademicBranchCommandResponse(int Id);

    #endregion

    #region validator

    public class CreateAcademicBranchCommandValidator : AbstractValidator<CreateAcademicBranchCommand>
    {
        public CreateAcademicBranchCommandValidator()
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

    public class CreateAcademicBranchCommandHandler : IRequestHandler<CreateAcademicBranchCommand, CreateAcademicBranchCommandResponse>
    {
        private readonly IAppDbContext _appDbContext;

        public CreateAcademicBranchCommandHandler(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<CreateAcademicBranchCommandResponse> Handle(CreateAcademicBranchCommand request, CancellationToken cancellationToken)
        {
            var isTitleDuplicated = await _appDbContext.AcademicBranches
                .AnyAsync(x => x.Title == request.Title);

            if (isTitleDuplicated)
            {
                throw new DuplicateException(Resource.DuplicateTitle);
            }

            var entity = new AcademicBranch
            {
                Title = request.Title,
                Description = request.Description,
            };

            _appDbContext.AcademicBranches.Add(entity);

            await _appDbContext.SaveChangesAsync();

            return new CreateAcademicBranchCommandResponse(entity.Id);
        }
    }

    #endregion
}
