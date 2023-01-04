namespace EducationSystem.Application.Common.Models
{
    public class JwtToken
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public double AccessTokenExpireTime { get; set; }
        public double RefreshTokenExpireTime { get; set; }
    }
}
