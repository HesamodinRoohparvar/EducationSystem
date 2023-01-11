using EducationSystem.Application.Common.Exceptions;
using EducationSystem.Application.Common.Interfaces;
using EducationSystem.Domain.Resources;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EducationSystem.Application.Admins.StudentCourses.Command
{
    #region command

    public record UpdateStudentCourseCommand(
        int Id,
        float Score,
        int TermCourseId,
        int StudentId) : IRequest;

    #endregion

    #region validator

    public class UpdateStudentCourseCommandValidator : AbstractValidator<UpdateStudentCourseCommand>
    {
        public UpdateStudentCourseCommandValidator()
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

    public class UpdateStudentCourseCommandHandler : IRequestHandler<UpdateStudentCourseCommand, Unit>
    {
        private readonly IAppDbContext _dbContext;

        public UpdateStudentCourseCommandHandler(IAppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Unit> Handle(UpdateStudentCourseCommand request, CancellationToken cancellationToken)
        {
            var entity = await _dbContext.StudentCourses
                .Where(x => x.Id == request.Id)
                .SingleOrDefaultAsync();

            if (entity == null)
            {
                throw new NotFoundException(Resource.StudentCourseNotFound);
            }

            entity.Score = request.Score;
            entity.TermCourseId = request.TermCourseId;
            entity.StudentId = request.StudentId;

            await _dbContext.SaveChangesAsync();

            return Unit.Value;
        }
    }

    #endregion
}
