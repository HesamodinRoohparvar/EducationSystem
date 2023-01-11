using EducationSystem.Application.Common.Exceptions;
using EducationSystem.Application.Common.Interfaces;
using EducationSystem.Domain.Resources;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EducationSystem.Application.Teachers.StudentCourses.Command
{
    #region command

    public record UpdateScoreCommand(
        int Id,
        float Score,
        int TermCourseId,
        int StudentId) : IRequest;

    #endregion

    #region validator

    public class UpdateScoreCommandValidator : AbstractValidator<UpdateScoreCommand>
    {
        public UpdateScoreCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithName(Resource.StudentCourseId);

            RuleFor(x => x.Score)
                .GreaterThanOrEqualTo(0)
                .WithName(Resource.Score);
        }
    }

    #endregion

    #region handler

    public class UpdateScoreCommandHandler : IRequestHandler<UpdateScoreCommand, Unit>
    {
        private readonly IAppDbContext _dbContext;

        public UpdateScoreCommandHandler(IAppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Unit> Handle(UpdateScoreCommand request, CancellationToken cancellationToken)
        {
            var entity = await _dbContext.StudentCourses
                .Where(x => x.Id == request.Id 
                && x.TermCourseId == request.TermCourseId 
                && x.StudentId == request.StudentId)
                .SingleOrDefaultAsync();

            if(entity == null)
            {
                throw new NotFoundException(Resource.StudentCourseNotFound);
            }

            entity.Score = request.Score;

            await _dbContext.SaveChangesAsync();

            return Unit.Value;
        }
    }

    #endregion
}
