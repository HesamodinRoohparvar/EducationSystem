using EducationSystem.Application.Common.Exceptions;
using EducationSystem.Application.Common.Interfaces;
using EducationSystem.Domain.Resources;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EducationSystem.Application.Admins.AcademicBranches.Command
{
    #region command

    public record DeleteAcademicBranchCommand(int Id) : IRequest;

    #endregion

    #region validator

    public class DeleteAcademicBranchValidator : AbstractValidator<DeleteAcademicBranchCommand>
    {
        public DeleteAcademicBranchValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithName(Resource.AcademicBranchId);
        }
    }

    #endregion

    #region handler

    public class DeleteAcademicBranchCommandHandler : IRequestHandler<DeleteAcademicBranchCommand>
    {
        private readonly IAppDbContext _dbContext;

        public DeleteAcademicBranchCommandHandler(IAppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Unit> Handle(DeleteAcademicBranchCommand request, CancellationToken cancellationToken)
        {
            var entity = await _dbContext.AcademicBranches
                .FindAsync(request.Id);

            if(entity == null)
            {
                throw new NotFoundException(Resource.AcademicBranchNotFound);
            }

            _dbContext.AcademicBranches.Remove(entity);

            await _dbContext.SaveChangesAsync();

            return Unit.Value;
        }
    }

    #endregion
}
