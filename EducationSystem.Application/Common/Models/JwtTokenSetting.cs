namespace EducationSystem.Application.Common.Models
{
    public class JwtTokenSetting
    {
        public string ValidIssuer { get; set; }
        public string ValidAudience { get; set; }
        public bool SaveToken { get; set; }
        public bool ValidateIssuer { get; set; }
        public bool ValidateLifetime { get; set; }
        public bool ValidateAudience { get; set; }
        public bool RequireSignedToken { get; set; }
        public bool RequireGttpsMetadata { get; set; }
        public bool RequireExpirationTime { get; set; }
        public bool ValidateIssuerSigningKey { get; set; }
        public string IssuerSigningKey { get; set; }
        public string TokenDecryptionKey { get; set; }
        public short AccessTokenExpiresMinutes { get; set; }
        public int RefreshTokenExpiresMinutes { get; set; }
        public short NotBeforeMinutes { get; set; }
    }
}
