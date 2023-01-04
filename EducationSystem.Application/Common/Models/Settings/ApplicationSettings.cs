using EducationSystem.Application.Common.Models.Settings;

namespace EducationSystem.Application.Common.Models
{
    public class ApplicationSettings
    {
        public DatabasesSettings Database { get; set; }
        public AuthenticationsSettings Authentication { get; set; }
        public TokenSettings Token { get; set; }
    }
}
