using EducationSystem.Application.Common.Excensions;
using EducationSystem.Application.Common.Exceptions;
using EducationSystem.Application.Common.Interfaces;
using EducationSystem.Domain.Resources;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EducationSystem.Application.Admins.Courses.Command
{
    #region command

    public record UpdateCourseCommand(
        int Id,
        string Title,
        string Description,
        int AcademicField,
        int PrerequisiteId) : IRequest;

    #endregion

    #region validator

    public class UpdateCourseCommandValidator : AbstractValidator<UpdateCourseCommand>
    {
        public UpdateCourseCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithName(Resource.CourseId);

            RuleFor(x => x.Title)
                .NotEmpty()
                .MaximumLength(25)
                .WithName(Resource.Title);

            RuleFor(x => x.Description)
                .MaximumLength(250)
                .WithName(Resource.Description);
        }
    }

    #endregion

    #region handler

    public class UpdateCourseCommandHandler : IRequestHandler<UpdateCourseCommand, Unit>
    {
        private readonly IAppDbContext _dbContext;

        public UpdateCourseCommandHandler(IAppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Unit> Handle(UpdateCourseCommand request, CancellationToken cancellationToken)
        {
            var entity = await _dbContext.Courses
                .Where(x => x.Id == request.Id)
                .SingleOrDefaultAsync();

            if (entity == null)
            {
                throw new NotFoundException(Resource.CourseNotFound);
            }

            if(entity.Title != request.Title)
            {
                var isTitleDuplicated = await _dbContext.Courses
                    .AnyAsync(x => x.Title == request.Title);

                if (isTitleDuplicated)
                {
                    throw new DuplicateException(Resource.DuplicateTitle);
                }
            }

            entity.Title = request.Title;
            entity.Description = request.Description;
            entity.AcademicFieldId = request.AcademicField;
            entity.PrerequisiteId = request.PrerequisiteId;

            await _dbContext.SaveChangesAsync();

            return Unit.Value;
        }
    }

    #endregion
}
