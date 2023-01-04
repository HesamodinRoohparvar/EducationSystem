using System.Security.Claims;
using EducationSystem.Domain.Enumerations;
using EducationSystem.Application.Common.Models;

namespace EducationSystem.Application.Common.Interfaces
{
    public interface ITokenManagerService
    {
        Task<JwtToken> GenerateJwtTokenAsynce(int userId, List<Claim> claims);
        Task<string> CreateTokenAsync(int userId, TokenType type,TokenDataType dataType, double expiresMinutes);
    }
}
