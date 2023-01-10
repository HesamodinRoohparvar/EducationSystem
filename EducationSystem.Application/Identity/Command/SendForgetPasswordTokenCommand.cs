using EducationSystem.Application.Common.Interfaces;
using EducationSystem.Application.Validators;
using EducationSystem.Domain.Entities;
using EducationSystem.Domain.Enumerations;
using EducationSystem.Domain.Resources;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EducationSystem.Application.Identity.Command
{
    #region command

    public class SendForgetPasswordTokenCommand : IRequest
    {
        public string Email { get; set; }
    }

    #endregion

    #region validator

    public class SendForgetPasswordTokenCommandValidator : AbstractValidator<SendForgetPasswordTokenCommand>
    {
        private readonly IAppDbContext _dbContext;

        public SendForgetPasswordTokenCommandValidator(IAppDbContext dbContext)
        {
            _dbContext = dbContext;

            RuleFor(x => x.Email)
                .NotEmpty()
                .MaximumLength(320)
                .ValidEmailAddress()
                .Must(ExistEmail).WithMessage("'{PropertyName}' یافت نشد.")
                .WithName(Resource.Email);
        }

        private bool ExistEmail(string email)
        {
            return _dbContext.Users.Any(x => x.Email == email);
        }
    }

    #endregion

    #region handler

    public class SendForgetPasswordTokenCommandHandler : IRequestHandler<SendForgetPasswordTokenCommand>
    {
        private readonly IAppDbContext _dbContext;
        private readonly ITokenManagerService _tokenManager;
        private readonly INotificationService _notificationService;

        public SendForgetPasswordTokenCommandHandler(IAppDbContext dbContext, ITokenManagerService tokenManager
            , INotificationService notificationService)
        {
            _dbContext = dbContext;
            _tokenManager = tokenManager;
            _notificationService = notificationService;
        }

        public async Task<Unit> Handle(SendForgetPasswordTokenCommand request, CancellationToken cancellationToken)
        {
            var userId = await _dbContext.Users
                .Where(x => x.Email == request.Email)
                .Select(x => x.Id)
                .SingleOrDefaultAsync();

            var token = await _tokenManager
                .CreateTokenAsync(userId, TokenType.ResetPassword, TokenDataType.AlphaNumerical, 10);

            var userToken = new UserToken
            {
                Type = TokenType.ResetPassword,
                UserId = userId,
                Value = token,
                ExpireAt = DateTime.UtcNow.AddMinutes(15)
            };

            _dbContext.UserTokens.Add(userToken);

            await _dbContext.SaveChangesAsync();

            await _notificationService.SendAsync(request.Email, "بازیابی رمز عبور", token);

            return Unit.Value;
        }
    }

    #endregion
}
