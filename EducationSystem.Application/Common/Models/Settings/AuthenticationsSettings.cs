namespace EducationSystem.Application.Common.Models.Settings
{
    public class AuthenticationsSettings
    {
        public LoginSettings Login { get; set; }
        public JwtTokenSetting Jwt { get; set; }
    }
}
