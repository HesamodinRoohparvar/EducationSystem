using EducationSystem.Application.Common.Excensions;
using EducationSystem.Application.Common.Extensions;
using EducationSystem.Application.Common.Interfaces;
using EducationSystem.Application.Security;
using EducationSystem.Application.Validators;
using EducationSystem.Domain;
using EducationSystem.Domain.Enumerations;
using EducationSystem.Domain.Resources;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace EducationSystem.Application.Admins.Users.Commands;

#region command

public record CreateUserCommand(
    string FirstName,
    string LastName,
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
    bool IsActive) : IRequest<CreateUserCommandResponse>;

#endregion

#region response

public record CreateUserCommandResponse(int id);

#endregion

#region validator

public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty()
            .MaximumLength(25)
            .WithName(Resource.FirstName);

        RuleFor(x => x.LastName)
            .NotEmpty()
            .MaximumLength(25)
            .WithName(Resource.LastName);

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
            .Matches(@"\d{11}")
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

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, CreateUserCommandResponse>
{
    private readonly IAppDbContext _dbContext;
    private readonly IFileManagerService _fileManagerService;
    private readonly IUserNameGeneratorService _userNameGenerator;

    public CreateUserCommandHandler(IAppDbContext dbContext, IFileManagerService fileManagerService)
    {
        _dbContext = dbContext;
        _fileManagerService = fileManagerService;
    }

    public async Task<CreateUserCommandResponse> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        if (!string.IsNullOrEmpty(request.IdentificationCode))
        {
            var isIdentificationCodeDuplicated = await _dbContext.Users
                .AnyAsync(x => x.IdentificationCode == request.IdentificationCode);

            if (isIdentificationCodeDuplicated)
            {
                throw new DuplicateException(Resource.DuplicateIdentification);
            }
        }

        if (!string.IsNullOrEmpty(request.MobileNubmer))
        {
            var isMobileNumberDuplicated = await _dbContext.Users
                .AnyAsync(x => x.MobileNumber == request.MobileNubmer);

            if (isMobileNumberDuplicated)
            {
                throw new DuplicateException(Resource.DuplicateMobileNumber);
            }
        }

        if (!string.IsNullOrEmpty(request.Email))
        {
            var isMobileNumberDuplicated = await _dbContext.Users
                .AnyAsync(x => x.Email == request.Email);

            if (isMobileNumberDuplicated)
            {
                throw new DuplicateException(Resource.DuplicateEmail);
            }
        }

        var userCount = 0;

        var userName = _userNameGenerator.GenerateUserName(request.AcademicFieldId, userCount);

        var entity = new User
        {
            FirsName = request.FirstName,
            LastName = request.LastName,
            UserName = userName,
            PasswordHash = PasswordHasher.Hash(request.IdentificationCode),
            Nationality = request.Nationality,
            IdentificationCode = request.IdentificationCode,
            Religion = request.Religion,
            BirthDate = request.BirthDate.ToDateTime(),
            MobileNumber = request.MobileNubmer,
            HomeNumber = request.HomeNumber,
            Email = request.Email.ToLower(),
            Address = request.Address,
            PostalCode = request.PostalCode,
            Photo = await _fileManagerService.SaveFileAsync(request.Photo),
            FatherName = request.FatherName,
            WorkAddress = request.WorkAddress,
            WorkPhoneNumber = request.WorkPhoneNumber,
            RoleId = request.RoleId,
            AcademicFieldId = request.AcademicFieldId,
            IsActive = request.IsActive
        };

        _dbContext.Users.Add(entity);

        await _dbContext.SaveChangesAsync();

        return new CreateUserCommandResponse(entity.Id);
    }
}

#endregion