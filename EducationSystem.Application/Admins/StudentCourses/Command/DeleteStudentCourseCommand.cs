using EducationSystem.Application.Common.Exceptions;
using EducationSystem.Application.Common.Interfaces;
using EducationSystem.Domain.Resources;
using FluentValidation;
using MediatR;

namespace EducationSystem.Application.Admins.StudentCourses.Command
{
    #region command

    public record DeleteStudentCourseCommand(int Id) : IRequest;

    #endregion

    #region validator

    public class DeleteStudentCourseCommandValidator : AbstractValidator<DeleteStudentCourseCommand>
    {
        public DeleteStudentCourseCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithName(Resource.StudentCourseId);
        }
    }

    #endregion

    #region handler

    public class DeleteStudentCourseCommandHandler : IRequestHandler<DeleteStudentCourseCommand, Unit>
    {
        private readonly IAppDbContext _dbContext;

        public DeleteStudentCourseCommandHandler(IAppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Unit> Handle(DeleteStudentCourseCommand request, CancellationToken cancellationToken)
        {
            var entity = await _dbContext.StudentCourses
                .FindAsync(request.Id);

            if(entity == null)
            {
                throw new NotFoundException(Resource.StudentCourseNotFound);
            }

            _dbContext.StudentCourses.Remove(entity);

            await _dbContext.SaveChangesAsync();

            return Unit.Value;
        }
    }

    #endregion
}
