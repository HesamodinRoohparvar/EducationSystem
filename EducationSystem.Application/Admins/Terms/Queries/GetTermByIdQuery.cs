using EducationSystem.Application.Common.Extensions;
using EducationSystem.Application.Common.Interfaces;
using EducationSystem.Domain.Resources;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EducationSystem.Application.Admins.Terms.Queries
{
    #region response

    public class GetTermByIdQueryResponse
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string StarDate { get; set; }
        public string EndDate { get; set; }
        public int TermCourseCount { get; set; }
        public int StudentCount { get; set; }
        public int TeacherCount { get; set; }
    }

    #endregion

    #region query

    public record GetRoleByIdQuery(int Id) : IRequest<GetTermByIdQueryResponse>;

    #endregion

    #region validator

    public class GetTermByIdQueryValidator : AbstractValidator<GetRoleByIdQuery>
    {
        public GetTermByIdQueryValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithName(Resource.TermId);
        }
    }

    #endregion

    #region handler

    public class GetTermByIdQueryHandler : IRequestHandler<GetRoleByIdQuery, GetTermByIdQueryResponse>
    {
        private readonly IAppDbContext _dbContext;

        public GetTermByIdQueryHandler(IAppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<GetTermByIdQueryResponse> Handle(GetRoleByIdQuery request, CancellationToken cancellationToken)
        {
            var result = await _dbContext.Terms
                .AsNoTracking()
                .Include(x => x.TermCourses)
                .ThenInclude(x => x.StudentCourses)
                .Where(x => x.Id == request.Id)
                .Select(x => new GetTermByIdQueryResponse
                {
                    Id = x.Id,
                    Title = x.Title,
                    Description = x.Description,
                    StarDate = x.StartDate.Format("yyyy/MM/dd"),
                    EndDate = x.EndDate.Format("yyyy/MM/dd"),
                    StudentCount =
                        x.TermCourses.Select(x => x.StudentCourses).Count(),
                    TermCourseCount = x.TermCourses.Count(),
                    TeacherCount = x.TermCourses.Select(x => x.Teacher).Count(),

                })
                .SingleOrDefaultAsync();

            return result;
        }
    }

    #endregion
}
