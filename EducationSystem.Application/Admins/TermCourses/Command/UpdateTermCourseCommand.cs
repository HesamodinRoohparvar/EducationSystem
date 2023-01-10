using EducationSystem.Application.Common.Exceptions;
using EducationSystem.Application.Common.Extensions;
using EducationSystem.Application.Common.Interfaces;
using EducationSystem.Application.Validators;
using EducationSystem.Domain.Resources;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EducationSystem.Application.Admins.TermCourses.Command
{
    #region command

    public record UpdateTermCoursesCommand(
        int Id,
        string Time,
        DayOfWeek Day,
        string Description,
        int TermId,
        int TeacherId,
        int CourseId) : IRequest;

    #endregion

    #region validator

    public class UpdateTermCoursesCommandValidator : AbstractValidator<UpdateTermCoursesCommand>
    {
        public UpdateTermCoursesCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithName(Resource.TermCourseId);

            RuleFor(x => x.Time)
                .ValidTime()
                .WithName(Resource.Time);

            RuleFor(x => x.Description)
                .MaximumLength(250)
                .WithName(Resource.Description);
        }
    }

    #endregion

    #region handler

    public class UpdateTermCoursesCommandHandler : IRequestHandler<UpdateTermCoursesCommand, Unit>
    {
        private readonly IAppDbContext _dbContext;

        public UpdateTermCoursesCommandHandler(IAppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Unit> Handle(UpdateTermCoursesCommand request, CancellationToken cancellationToken)
        {
            var entity = await _dbContext.TermCourses
                .Where(x => x.Id == request.Id)
                .SingleOrDefaultAsync();

            if (entity == null)
            {
                throw new NotFoundException(Resource.TermCourseNotFound);
            }

            entity.Time = request.Time.ToTimeSpan();
            entity.Description = request.Description;
            entity.TermId = request.TermId;
            entity.TeacherId = request.TeacherId;
            entity.CourseId = request.CourseId;

            await _dbContext.SaveChangesAsync();

            return Unit.Value;
        }
    }

    #endregion
}
