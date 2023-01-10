using MediatR;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using EducationSystem.Domain.Resources;
using EducationSystem.Application.Security;
using EducationSystem.Application.Common.Interfaces;

namespace EducationSystem.Application.Admins.Identity.Command
{
    #region command

    public class ResetPasswordCommand : IRequest
    {
        public string Token { get; set; }
        public string NewPassword { get; set; }

        public ResetPasswordCommand(string token)
        {
            Token = token;
        }
    }

    #endregion

    #region validator

    public class ResetPasswordCommandValidator : AbstractValidator<ResetPasswordCommand>
    {
        private readonly IAppDbContext _dbContext;

        public ResetPasswordCommandValidator(IAppDbContext dbContext)
        {
            _dbContext = dbContext;

            RuleFor(x => x.NewPassword)
                .NotEmpty()
                .Length(8, 50)
                .WithName(Resource.NewPassword);

            RuleFor(x => x.Token)
                .NotEmpty()
                .MaximumLength(250)
                .Must(ExistToken).WithMessage("{PropertyName} یافت نشد")
                .Must(ValidToken).WithMessage("{PropertyName} منقظی شد")
                .WithName(Resource.Token);
        }

        private bool ExistToken(string token)
        {
            return _dbContext.UserTokens.Any(x => x.Value == token);
        }

        private bool ValidToken(string token)
        {
            return _dbContext.UserTokens.Any(x => x.Value == token && x.ExpireAt > DateTime.Now);
        }
    }

    #endregion

    #region handler

    public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand>
    {
        private readonly IAppDbContext _dbContext;

        public ResetPasswordCommandHandler(IAppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Unit> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await _dbContext.UserTokens
                .Where(x => x.Value == request.Token)
                .Select(x => x.User)
                .SingleOrDefaultAsync();

            user.PasswordHash = PasswordHasher.Hash(request.NewPassword);

            await _dbContext.SaveChangesAsync();

            return Unit.Value;
        }
    }

    #endregion
}
