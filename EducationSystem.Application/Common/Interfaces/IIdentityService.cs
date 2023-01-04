using EducationSystem.Application.Common.Models;
using EducationSystem.Domain;

namespace EducationSystem.Application.Common.Interfaces
{
    public interface IIdentityService
    {
        Task<Result<(User User, JwtToken Token)>> LoginAsync(string username, string password);
        Task<Result> ResetPasswordAsync(string resetPasswordToken, string newPassword);
        Task<bool> AuthorizeAsync(int userId, string area, string controller, string action);
    }
}
