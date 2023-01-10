using EducationSystem.Application.Common.Extensions;
using EducationSystem.Application.Common.Interfaces;
using EducationSystem.Application.Validators;
using EducationSystem.Domain.Entities;
using EducationSystem.Domain.Resources;
using FluentValidation;
using MediatR;

namespace EducationSystem.Application.Admins.TermCourses.Command
{
    #region command

    public record CreateTermCourseCommand(
        string Time,
        DayOfWeek Day,
        string Description,
        int TermId,
        int TeacherId,
        int CourseId) : IRequest<CreateTermCourseCommandResponse>;

    #endregion

    #region response

    public record CreateTermCourseCommandResponse(int Id);

    #endregion

    #region validator

    public class CreateTermCourseCommandValidator : AbstractValidator<CreateTermCourseCommand>
    {
        public CreateTermCourseCommandValidator()
        {
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

    public class CreateTermCourseCommandHandler : IRequestHandler<CreateTermCourseCommand, CreateTermCourseCommandResponse>
    {
        private readonly IAppDbContext _dbContext;

        public CreateTermCourseCommandHandler(IAppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<CreateTermCourseCommandResponse> Handle(CreateTermCourseCommand request, CancellationToken cancellationToken)
        {
            var entity = new TermCourse
            {
                Time = request.Time.ToTimeSpan(),
                Description = request.Description,
                TermId = request.TermId,
                TeacherId = request.TeacherId,
                CourseId = request.CourseId
            };

            _dbContext.TermCourses.Add(entity);

            await _dbContext.SaveChangesAsync();

            return new CreateTermCourseCommandResponse(entity.Id);
        }
    }

    #endregion
}
