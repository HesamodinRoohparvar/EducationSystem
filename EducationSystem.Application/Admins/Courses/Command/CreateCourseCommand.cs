using EducationSystem.Application.Common.Excensions;
using EducationSystem.Application.Common.Interfaces;
using EducationSystem.Domain.Entities;
using EducationSystem.Domain.Resources;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EducationSystem.Application.Admins.Courses.Command
{
    #region command

    public record CreateCourseCommand(
        string Title,
        string Description,
        int AcademicFieldId,
        int PrerequisiteId) : IRequest<CreateCourseCommandResponse>;

    #endregion

    #region response

    public record CreateCourseCommandResponse(int Id);

    #endregion

    #region validator

    public class CreateCourseCommandValidator : AbstractValidator<CreateCourseCommand>
    {
        public CreateCourseCommandValidator()
        {
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

    public class CreateCourseCommandHandler : IRequestHandler<CreateCourseCommand, CreateCourseCommandResponse>
    {
        private readonly IAppDbContext _dbContext;

        public CreateCourseCommandHandler(IAppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<CreateCourseCommandResponse> Handle(CreateCourseCommand request, CancellationToken cancellationToken)
        {
            var isTitleDuplicated = await _dbContext.Courses
                .AnyAsync(x => x.Title == request.Title);

            if (isTitleDuplicated)
            {
                throw new DuplicateException(Resource.DuplicateTitle);
            }

            var entity = new Course
            {
                Title = request.Title,
                Description = request.Description,
                AcademicFieldId = request.AcademicFieldId,
                PrerequisiteId = request.PrerequisiteId
            };

            _dbContext.Courses.Add(entity);

            await _dbContext.SaveChangesAsync();

            return new CreateCourseCommandResponse(entity.Id);
        }
    }

    #endregion
}
