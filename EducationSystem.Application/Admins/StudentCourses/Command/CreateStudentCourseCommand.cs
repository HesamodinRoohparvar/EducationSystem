using EducationSystem.Application.Common.Interfaces;
using EducationSystem.Domain.Entities;
using EducationSystem.Domain.Resources;
using FluentValidation;
using MediatR;

namespace EducationSystem.Application.Admins.StudentCourses.Command
{
    #region command

    public record CreateStudentCourseCommand(
        float Score,
        int TermCourseId,
        int StudentId) : IRequest<CreateStudentCoursesCommandResponce>;

    #endregion

    #region response

    public record CreateStudentCoursesCommandResponce(int Id);

    #endregion

    #region validator

    public class CreateStudentCourseCommandValidator : AbstractValidator<CreateStudentCourseCommand>
    {
        public CreateStudentCourseCommandValidator()
        {
            RuleFor(x => x.Score)
                .GreaterThanOrEqualTo(0)
                .WithName(Resource.Score);
        }
    }

    #endregion

    #region handler

    public class CreateStudentCourseCommandHandler : IRequestHandler<CreateStudentCourseCommand, CreateStudentCoursesCommandResponce>
    {
        private readonly IAppDbContext _dbContext;

        public CreateStudentCourseCommandHandler(IAppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<CreateStudentCoursesCommandResponce> Handle(CreateStudentCourseCommand request, CancellationToken cancellationToken)
        {
            var entity = new StudentCourse
            {
                Score = request.Score,
                TermCourseId = request.TermCourseId,
                StudentId = request.StudentId
            };

            _dbContext.StudentCourses.Add(entity);

            await _dbContext.SaveChangesAsync();

            return new CreateStudentCoursesCommandResponce(entity.Id);
        }
    }

    #endregion
}
