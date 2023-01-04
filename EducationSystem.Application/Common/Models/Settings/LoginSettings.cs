namespace EducationSystem.Application.Common.Models.Settings
{
    public class LoginSettings
    {
        public int LockoutMinetes { get; set; }
        public int MaxFailedAttempts { get; set; }
        public bool LockAccountAffterTooManyAttempts { get; set; }
        public bool RequireActiveAccount { get; set; }
    }
}
