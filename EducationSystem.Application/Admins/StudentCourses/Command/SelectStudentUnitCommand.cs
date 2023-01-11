using EducationSystem.Application.Common.Exceptions;
using EducationSystem.Application.Common.Interfaces;
using EducationSystem.Domain.Entities;
using EducationSystem.Domain.Resources;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EducationSystem.Application.Admins.StudentCourses.Command
{
    #region command

    public record SelectStudentUnitCommand(
        int TermCourseId,
        int StudentId) : IRequest<SelectStudentUnitCommandResponce>;

    #endregion

    #region response

    public record SelectStudentUnitCommandResponce(int Id);

    #endregion

    #region handler

    public class SelectStudentUnitCommandHandler : IRequestHandler<SelectStudentUnitCommand, SelectStudentUnitCommandResponce>
    {
        private readonly IAppDbContext _dbContext;

        public SelectStudentUnitCommandHandler(IAppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<SelectStudentUnitCommandResponce> Handle(SelectStudentUnitCommand request, CancellationToken cancellationToken)
        {
            var prerequisiteId = await _dbContext.TermCourses
                .Include(x => x.Course)
                .Where(x => x.Id == request.TermCourseId)
                .Select(x => x.Course.PrerequisiteId)
                .SingleOrDefaultAsync();

            var entity = new StudentCourse();

            if (prerequisiteId == null)
            {
                entity = new StudentCourse
                {
                    TermCourseId = request.TermCourseId,
                    StudentId = request.StudentId
                };

                _dbContext.StudentCourses.Add(entity);

                await _dbContext.SaveChangesAsync();
            }
            else
            {
                var passThePrerequisite = await _dbContext.StudentCourses
                    .Where(x =>
                        x.Score >= 10 &&
                        x.StudentId == request.StudentId &&
                        x.TermCourseId == request.TermCourseId)
                    .AnyAsync();

                if (passThePrerequisite)
                {
                    entity = new StudentCourse
                    {
                        TermCourseId = request.TermCourseId,
                        StudentId = request.StudentId
                    };

                    _dbContext.StudentCourses.Add(entity);

                    await _dbContext.SaveChangesAsync();
                }
                else
                {
                    throw new OperationNotAllowedException(Resource.FluentValidationInvalidCourseError);
                }
            }

            return new SelectStudentUnitCommandResponce(entity.Id);
        }
    }

    #endregion
}
