using System.Globalization;

namespace EducationSystem.Application.Common.Constans
{
    public static class Defaults
    {
        public static string HostUrl { get; set; }

        public static string DefaultAvatarPath =>
            $"{HostUrl}/avatar.jpeg";

        public static string CultureName { get; set; }
        public static CultureInfo Culture => new CultureInfo(CultureName);
    }
}
