using EducationSystem.Application.Common.Constans;
using EducationSystem.Application.Common.Exceptions;
using EducationSystem.Application.Common.Extensions;
using EducationSystem.Application.Common.Interfaces;
using EducationSystem.Domain.Resources;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EducationSystem.Application.Admins.Users.Queries
{
    #region response

    public record GetUserByIdQueryResponse
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Nationality { get; set; }
        public string IdentificationCode { get; set; }
        public string Religion { get; set; }
        public string BirthDate { get; set; }
        public string MobileNumber { get; set; }
        public string HomeNumber { get; set; }
        public string Address { get; set; }
        public string PostalCode { get; set; }
        public string Photo { get; set; }
        public string FatherName { get; set; }
        public string? FatherPhoneNumber { get; set; }
        public string? WorkAddress { get; set; }
        public string? WorkPhoneNumber { get; set; }
        public string UserName { get; set; }
        public string? Email { get; set; }
        public string AcademicField { get; set; }
        public string GraduationDate { get; set; }
        public bool IsActive { get; set; }
        public string CreatedAt { get; set; }
        public string LastLoginAt { get; set; }
        public string RoleName { get; set; }
    }

    #endregion

    #region query

    public record GetUserByIdQuery(int Id) : IRequest<GetUserByIdQueryResponse>;

    #endregion

    #region Validator

    public class GetUserByIdQueryValidator : AbstractValidator<GetUserByIdQuery>
    {
        public GetUserByIdQueryValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithName(Resource.UserId);
        }
    }

    #endregion

    #region handler

    public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, GetUserByIdQueryResponse>
    {
        private readonly IAppDbContext _dbContext;

        public GetUserByIdQueryHandler(IAppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<GetUserByIdQueryResponse> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            var result = await _dbContext.Users
                .AsNoTracking()
                .Include(x => x.Role)
                .Where(x => x.Id == request.Id)
                .Select(x => new GetUserByIdQueryResponse
                {
                    Id = x.Id,
                    FirstName = x.FirsName,
                    LastName = x.LastName,
                    Nationality = x.Nationality.GetLocalizedDescription(),
                    IdentificationCode = x.IdentificationCode,
                    Religion = x.Religion.GetLocalizedDescription(),
                    BirthDate = x.BirthDate.Format("yyyy/MM/dd"),
                    MobileNumber = x.MobileNumber,
                    HomeNumber = x.HomeNumber,
                    Address = x.Address,
                    PostalCode = x.PostalCode,
                    Photo = string.IsNullOrEmpty(x.Photo) ? Defaults.DefaultAvatarPath : string.Join("/", Defaults.HostUrl, x.Photo),
                    FatherName = x.FatherName,
                    FatherPhoneNumber = x.FatherPhoneNumber,
                    WorkAddress = x.WorkAddress,
                    WorkPhoneNumber = x.WorkPhoneNumber,
                    UserName = x.UserName,
                    RoleName = x.Role.Title,
                    Email = x.Email,
                    AcademicField = x.AcademicField.Title,
                    GraduationDate = x.GraduationDate.Format("yyyy/MM/dd"),
                    IsActive = x.IsActive,
                    CreatedAt = x.CreatedAt.Format("yyyy/MM/dd hh:mm:ss"),
                    LastLoginAt = x.LastLoginAt.Format("yyyy/MM/dd hh:mm:ss"),
                }).SingleOrDefaultAsync();

            if (result == null)
            {
                throw new NotFoundException(Resource.UserNotFound);
            }

            return result;
        }
    }

    #endregion
}
