using EducationSystem.Application.Common.Excensions;
using EducationSystem.Application.Common.Exceptions;
using EducationSystem.Application.Common.Extensions;
using EducationSystem.Application.Common.Interfaces;
using EducationSystem.Application.Validators;
using EducationSystem.Domain.Enumerations;
using EducationSystem.Domain.Resources;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace EducationSystem.Application.Teachers.Users.Command
{
    #region command

    public record UpdateUserinformationCommand(
        int Id,
        string FirstName,
        string LastName,
        Nationality Nationality,
        string IdentificationCode,
        Religion Religion,
        string BirthDate,
        string MobileNumber,
        string HomeNumber,
        string Address,
        string PostalCode,
        IFormFile Photo,
        string FatherName,
        string WorkAddress,
        string WorkPhoneNumber,
        string Email) : IRequest;

    #endregion

    #region validator

    public class UpdateUserinformationCommandValidator : AbstractValidator<UpdateUserinformationCommand>
    {
        public UpdateUserinformationCommandValidator()
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

            RuleFor(x => x.MobileNumber)
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

            RuleFor(x => x.WorkAddress)
                .MaximumLength(800)
                .WithName(Resource.WorkAddress);

            RuleFor(x => x.WorkPhoneNumber)
                .MaximumLength(11)
                .Matches(@"\d{11}")
                .WithMessage("{PropertyName} باید یازده رقم باشد.")
                .WithName(Resource.WorkPhoneNumber);
        }
    }

    #endregion

    #region handler

    public class UpdateUserinformationCommandHandler : IRequestHandler<UpdateUserinformationCommand, Unit>
    {
        private readonly IAppDbContext _dbContext;
        private readonly IFileManagerService _fileManager;
        private readonly ICurrentUserService _currentUser;
        public UpdateUserinformationCommandHandler(IAppDbContext dbContext, IFileManagerService fileManager
            , ICurrentUserService currentUser)
        {
            _dbContext = dbContext;
            _fileManager = fileManager;
            _currentUser = currentUser;
        }

        public async Task<Unit> Handle(UpdateUserinformationCommand request, CancellationToken cancellationToken)
        {
            var entity = await _dbContext.Users
                .FindAsync(_currentUser.UserId);

            if (entity == null)
            {
                throw new NotFoundException(Resource.UserNotFound);
            }

            if (request.IdentificationCode != entity.IdentificationCode)
            {
                var isIdentificationCode = await _dbContext.Users
                    .AnyAsync(x => x.IdentificationCode == request.IdentificationCode);

                if (isIdentificationCode)
                {
                    throw new DuplicateException(Resource.DuplicateIdentification);
                }
            }

            if ( entity.MobileNumber != request.MobileNumber)
            {
                var isIdentificationCode = await _dbContext.Users
                    .AnyAsync(x => x.MobileNumber == request.MobileNumber);

                if (isIdentificationCode)
                {
                    throw new DuplicateException(Resource.DuplicateMobileNumber);
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
            entity.MobileNumber = request.MobileNumber;
            entity.HomeNumber = request.HomeNumber;
            entity.Email = request.Email.ToLower();
            entity.Address = request.Address;
            entity.PostalCode = request.PostalCode;
            entity.Photo = await _fileManager.SaveFileAsync(request.Photo);
            entity.FatherName = request.FatherName;
            entity.WorkAddress = request.WorkAddress;
            entity.WorkPhoneNumber = request.WorkPhoneNumber;

            await _dbContext.SaveChangesAsync();

            return Unit.Value;
        }
    }

    #endregion
}
