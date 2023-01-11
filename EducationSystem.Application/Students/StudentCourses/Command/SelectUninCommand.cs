using EducationSystem.Application.Common.Exceptions;
using EducationSystem.Application.Common.Interfaces;
using EducationSystem.Domain.Entities;
using EducationSystem.Domain.Resources;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EducationSystem.Application.Students.StudentCourses.Command
{
    #region command

    public record SelectUninCommand(
        int TermCourseId,
        int StudentId) : IRequest<SelectUninCommandResponse>;

    #endregion

    #region response

    public record SelectUninCommandResponse(int Id);

    #endregion

    #region handler

    public class SelectUninCommandHandler : IRequestHandler<SelectUninCommand, SelectUninCommandResponse>
    {
        private readonly IAppDbContext _dbContext;

        public SelectUninCommandHandler(IAppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<SelectUninCommandResponse> Handle(SelectUninCommand request, CancellationToken cancellationToken)
        {
            var prerequisiteId = await _dbContext.TermCourses
                .Include(x => x.Course)
                .Where(x => x.Id == request.TermCourseId)
                .Select(x => x.Course.PrerequisiteId)
                .SingleOrDefaultAsync();

            var entity = new StudentCourse();

            if (prerequisiteId == null)
            {
                // create
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

            return new SelectUninCommandResponse(entity.Id);
        }
    }

    #endregion
}
