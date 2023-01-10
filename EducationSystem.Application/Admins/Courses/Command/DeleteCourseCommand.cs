using EducationSystem.Application.Common.Exceptions;
using EducationSystem.Application.Common.Interfaces;
using EducationSystem.Domain.Resources;
using FluentValidation;
using MediatR;

namespace EducationSystem.Application.Admins.Courses.Command
{
    #region command

    public record DeleteCourseCommand(int Id) : IRequest;

    #endregion

    #region validator

    public class DeleteCourseCommandValidator : AbstractValidator<DeleteCourseCommand>
    {
        public DeleteCourseCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithName(Resource.CourseId);
        }
    }

    #endregion

    #region handler

    public class DeleteCourseCommandHandler : IRequestHandler<DeleteCourseCommand, Unit>
    {
        private readonly IAppDbContext _dbContext;

        public DeleteCourseCommandHandler(IAppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Unit> Handle(DeleteCourseCommand request, CancellationToken cancellationToken)
        {
            var entity = await _dbContext.Courses
                .FindAsync(request.Id);

            if(entity == null)
            {
                throw new NotFoundException(Resource.CourseNotFound);
            }

            _dbContext.Courses.Remove(entity);

            await _dbContext.SaveChangesAsync();

            return Unit.Value;
        }
    }

    #endregion
}
