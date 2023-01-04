using EducationSystem.Application.Common.Interfaces;
using EducationSystem.Application.Common.Models;
using EducationSystem.Application.Common.Models.Settings;
using EducationSystem.Application.Security;
using EducationSystem.Domain;
using EducationSystem.Domain.Enumerations;
using EducationSystem.Domain.Resources;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace EducationSystem.Infrastructure.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly IAppDbContext _dbContext;
        private readonly LoginSettings _loginSettings;
        private readonly ILogger<IdentityService> _logger;
        private readonly IDateTimeService _dateTimeService;
        private readonly ITokenManagerService _tokenManagerService;

        public IdentityService(IAppDbContext dbContext, IOptionsMonitor<LoginSettings> loginSettings, ILogger<IdentityService> logger,
            IDateTimeService dateTimeService, ITokenManagerService tokenManagerService)
        {
            _dbContext = dbContext;
            _loginSettings = loginSettings.CurrentValue;
            _logger = logger;
            _dateTimeService = dateTimeService;
            _tokenManagerService = tokenManagerService;
        }

        public async Task<bool> AuthorizeAsync(int userId, string area, string controller, string action)
        {
            area = area.ToLower();
            controller = controller.ToLower();
            action = action.ToLower();

            var result = await _dbContext.Users
                .AsNoTracking()
                .Include(x => x.Role.RolePermissions)
                .Where(x => x.Id == userId)
                .SelectMany(x => x.Role.RolePermissions)
                .AnyAsync(x => x.Area == area && x.Controller == controller && x.Action == action);

            return result;
        }

        public async Task<Result<(User User, JwtToken Token)>> LoginAsync(string username, string password)
        {
            var user = await _dbContext.Users
                .Include(x => x.Role.RolePermissions)
                .Where(x => x.UserName.ToLower() == username.ToLower())
                .FirstOrDefaultAsync();

            if (user == null)
            {
                return Result<(User User, JwtToken token)>.Failure(Resource.UsernameOrPasswordIsWrong);
            }

            if (!PasswordHasher.Verify(password, user.PasswordHash))
            {
                user.AccessFailedCount += 1;

                if (user.AccessFailedCount >= _loginSettings.MaxFailedAttempts)
                {
                    user.AccessFailedCount = 0;
                    user.LockoutEndAt = _dateTimeService.Now.AddMinutes(_loginSettings.LockoutMinetes);

                    var lockTime = user.LockoutEndAt.Value - _dateTimeService.Now;

                    await _dbContext.SaveChangesAsync();

                    return Result<(User User, JwtToken token)>.Failure(GenerateAccountLockMessage(lockTime));
                }

                await _dbContext.SaveChangesAsync();

                return Result<(User User, JwtToken token)>.Failure(Resource.UsernameOrPasswordIsWrong);
            }

            if (user.AccessFailedCount != 0)
            {
                user.LockoutEndAt = null;
                user.AccessFailedCount = 0;
            }

            user.LastLoginAt = _dateTimeService.Now;

            await _dbContext.SaveChangesAsync();

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Role, user.Role.Title),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            };

            var jwtToken = await _tokenManagerService.GenerateJwtTokenAsynce(user.Id, claims);

            return Result<(User User, JwtToken token)>.Success((user, jwtToken));
        }

        public async Task<Result> ResetPasswordAsync(string resetPasswordToken, string newPassword)
        {
            var entity = await _dbContext.UserTokens
                .Include(x => x.User)
                .Where(x =>
                    x.Type == TokenType.ResetPassword &&
                    x.Value == resetPasswordToken)
                .FirstOrDefaultAsync();

            if(entity == null)
            {
                return Result.Failure(Resource.ResetPasswordTokenNotFound);
            }

            if(entity.ExpireAt < _dateTimeService.Now)
            {
                return Result.Failure(Resource.ResetPasswordTokenExpired);
            }

            entity.User.PasswordHash = PasswordHasher.Hash(newPassword);

            await _dbContext.SaveChangesAsync();

            return Result.Success();
        }


        #region utility

        private string GenerateAccountLockMessage(TimeSpan lockTime)
        {
            var message = string.Format(
                Resource.AccountIsLockedTemplate,
                _loginSettings.MaxFailedAttempts,
                lockTime.Minutes,
                lockTime.Seconds);

            return message;
        }
        #endregion
    }
}
