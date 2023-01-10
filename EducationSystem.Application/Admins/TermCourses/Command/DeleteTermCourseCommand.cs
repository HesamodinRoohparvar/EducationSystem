using EducationSystem.Application.Common.Exceptions;
using EducationSystem.Application.Common.Interfaces;
using EducationSystem.Domain.Resources;
using FluentValidation;
using MediatR;

namespace EducationSystem.Application.Admins.TermCourses.Command
{
    #region command

    public record DeleteTermCourseCommand(int Id) : IRequest;

    #endregion

    #region validator

    public class DeleteTermCourseCommandValidator : AbstractValidator<DeleteTermCourseCommand>
    {
        public DeleteTermCourseCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithName(Resource.TermCourseId);
        }
    }

    #endregion

    #region handler

    public class DeleteTermCourseCommandHandler : IRequestHandler<DeleteTermCourseCommand, Unit>
    {
        private readonly IAppDbContext _dbContext;
public DeleteTermCourseCommandHandler(IAppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Unit> Handle(DeleteTermCourseCommand request, CancellationToken cancellationToken)
        {
            var entity = await _dbContext.TermCourses
                .FindAsync(request.Id);

            if(entity == null)
            {
                throw new NotFoundException(Resource.CourseNotFound);
            }

            _dbContext.TermCourses.Remove(entity);

            await _dbContext.SaveChangesAsync();

            return Unit.Value;
        }
    }

    #endregion
}
