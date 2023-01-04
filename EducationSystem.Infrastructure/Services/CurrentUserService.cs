using EducationSystem.Application.Common.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace EducationSystem.Infrastructure.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public int UserId => Convert.ToInt32(GetClaimValue(ClaimTypes.NameIdentifier));
        public string Role => GetClaimValue(ClaimTypes.NameIdentifier);

        private string GetClaimValue(string claimType)
        {
            return _httpContextAccessor?.HttpContext.User?.FindFirstValue(claimType);
        }
    }
}
