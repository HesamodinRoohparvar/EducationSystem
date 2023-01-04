using EducationSystem.Application.Common.Excensions;
using EducationSystem.Application.Common.Exceptions;
using EducationSystem.Application.Common.Extensions;
using EducationSystem.Application.Common.Interfaces;
using EducationSystem.Application.Security;
using EducationSystem.Application.Validators;
using EducationSystem.Domain.Enumerations;
using EducationSystem.Domain.Resources;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace EducationSystem.Application.Admins.Users.Commands
{
    #region command

    public record UpdateUserCommand(
        int Id,
        string FirstName,
        string LastName,
        string NewPassword,
        Nationality Nationality,
        string IdentificationCode,
        Religion Religion,
        string BirthDate,
        string MobileNubmer,
        string HomeNumber,
        string Email,
        string Address,
        string PostalCode,
        IFormFile Photo,
        string FatherName,
        string FatherPhoneNumber,
        string WorkAddress,
        string WorkPhoneNumber,
        int RoleId,
        int AcademicFieldId,
        bool IsActive) : IRequest;

    #endregion

    #region validator

    public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
    {
        public UpdateUserCommandValidator()
        {
            RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithName(Resource.UserId);

            RuleFor(x => x.FirstName)
            .NotEmpty()
            .MaximumLength(25)
            .WithName(Resource.FirstName);

            RuleFor(x => x.LastName)
                .NotEmpty()
                .MaximumLength(25)
                .WithName(Resource.LastName);

            RuleFor(x => x.NewPassword)
                .NotEmpty()
                .Length(8, 50)
                .When(x => !string.IsNullOrEmpty(x.NewPassword), ApplyConditionTo.AllValidators)
                .WithName(Resource.Password);

            RuleFor(x => x.Nationality)
                .NotEmpty()
                .WithName(Resource.Nationality);

            RuleFor(x => x.IdentificationCode)
                .NotEmpty()
                .MaximumLength(11)
                .Matches(@"\d{11}")
                .WithMessage("{PropertyName} باید یازده رقم باشد.")
                .WithName(Resource.IdentificationCode);

            RuleFor(x => x.Religion)
                .NotEmpty()
                .WithName(Resource.Religion);

            RuleFor(x => x.BirthDate)
                .NotEmpty()
                .WithName(Resource.BirthDate);

            RuleFor(x => x.MobileNubmer)
                .NotEmpty()
                .ValidMobileNumber()
                .MaximumLength(11)
                .WithName(Resource.MobileNumber);

            RuleFor(x => x.HomeNumber)
                .NotEmpty()
                .MaximumLength(11)
                .Matches(@"\d{11}")
                .WithMessage("{PropertyName} باید یازده رقم باشد.")
                .WithName(Resource.HomeNumber);

            RuleFor(x => x.Email)
                .ValidEmailAddress()
                .MaximumLength(320)
                .When(x => !string.IsNullOrEmpty(x.Email), ApplyConditionTo.AllValidators)
                .WithName(Resource.Email);

            RuleFor(x => x.Address)
                .NotEmpty()
                .MaximumLength(800)
                .WithName(Resource.Address);

            RuleFor(x => x.PostalCode)
                .NotEmpty()
                .MaximumLength(10)
                .Matches(@"\d{10}")
                .WithMessage("{PropertyName} باید ده رقم باشد.")
                .WithName(Resource.PostalCode);

            RuleFor(x => x.Photo)
                .NotEmpty()
                .NotNull()
                .WithName(Resource.Photo);

            RuleFor(x => x.FatherName)
                .NotEmpty()
                .MaximumLength(25)
                .WithName(Resource.FatherName);

            RuleFor(x => x.FatherPhoneNumber)
                .ValidMobileNumber()
                .MaximumLength(11)
                .WithName(Resource.FatherPhoneNumber);

            RuleFor(x => x.WorkAddress)
                .MaximumLength(800)
                .WithName(Resource.WorkAddress);

            RuleFor(x => x.WorkPhoneNumber)
                .MaximumLength(11)
                .Matches(@"\d{11}")
                .WithMessage("{PropertyName} باید یازده رقم باشد.")
                .WithName(Resource.WorkPhoneNumber);

            RuleFor(x => x.RoleId)
                .GreaterThan(0)
                .WithName(Resource.RoleId);

            RuleFor(x => x.AcademicFieldId)
                .GreaterThan(0)
                .WithName(Resource.AcademicField);
        }
    }

    #endregion

    #region handler 

    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, Unit>
    {
        private readonly IAppDbContext _dbContext;
        private readonly IFileManagerService _fileManagerService;

        public UpdateUserCommandHandler(IAppDbContext dbContext, IFileManagerService fileManagerService)
        {
            _dbContext = dbContext;
            _fileManagerService = fileManagerService;
        }

        public async Task<Unit> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var entity = await _dbContext.Users.FindAsync(request.Id);

            if(entity == null)
            {
                throw new NotFoundException(Resource.UserNotFound);
            }

            if (!string.IsNullOrEmpty(request.IdentificationCode))
            {
                if(request.IdentificationCode != entity.IdentificationCode)
                {
                    var isIdentificationCode = await _dbContext.Users
                        .AnyAsync(x => x.IdentificationCode == request.IdentificationCode);

                    if (isIdentificationCode)
                    {
                        throw new DuplicateException(Resource.DuplicateIdentification);
                    }
                }
            }

            if (!string.IsNullOrEmpty(request.MobileNubmer))
            {
                if (request.MobileNubmer != entity.MobileNumber)
                {
                    var isIdentificationCode = await _dbContext.Users
                        .AnyAsync(x => x.MobileNumber == request.MobileNubmer);

                    if (isIdentificationCode)
                    {
                        throw new DuplicateException(Resource.DuplicateMobileNumber);
                    }
                }
            }

            if (!string.IsNullOrEmpty(request.Email))
            {
                if (request.Email != entity.Email)
                {
                    var isIdentificationCode = await _dbContext.Users
                        .AnyAsync(x => x.Email == request.Email);

                    if (isIdentificationCode)
                    {
                        throw new DuplicateException(Resource.DuplicateEmail);
                    }
                }
            }

            entity.FirsName = request.FirstName;
            entity.LastName = request.LastName;
            entity.Nationality = request.Nationality;
            entity.IdentificationCode = request.IdentificationCode;
            entity.Religion = request.Religion;
            entity.BirthDate = request.BirthDate.ToDateTime();
            entity.MobileNumber = request.MobileNubmer;
            entity.HomeNumber = request.HomeNumber;
            entity.Email = request.Email.ToLower();
            entity.Address = request.Address;
            entity.PostalCode = request.PostalCode;
            entity.Photo = await _fileManagerService.SaveFileAsync(request.Photo);
            entity.FatherName = request.FatherName;
            entity.FatherPhoneNumber = request.FatherPhoneNumber;
            entity.WorkAddress = request.WorkAddress;
            entity.WorkPhoneNumber = request.WorkPhoneNumber;
            entity.RoleId = request.RoleId;
            entity.AcademicFieldId = request.AcademicFieldId;
            entity.IsActive = request.IsActive;

            if (request.NewPassword != null)
            {
                entity.PasswordHash = PasswordHasher.Hash(request.NewPassword);
            }

            await _dbContext.SaveChangesAsync();

            return Unit.Value;
        }
    }

    #endregion
}
