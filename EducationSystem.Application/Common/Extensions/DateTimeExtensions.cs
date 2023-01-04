using EducationSystem.Application.Common.Constans;
using System.Text.RegularExpressions;

namespace EducationSystem.Application.Common.Extensions
{
    public static class DateTimeExtensions
    {
        public static string Format(this DateTime dateTime, string format = "yyyy/MM/dd hh:mm:ss")
        {
            return dateTime.ToString(format, Defaults.Culture);
        }

        public static string Format(this DateTime? dateTime, string format = "yyyy/MM/dd hh:mm:ss")
        {
            return dateTime?.ToString(format, Defaults.Culture) ?? string.Empty;
        }

        public static string Format(this TimeSpan time, string format = "hh:mm:ss")
        {
            return time.ToString(format?.Replace(":", "\\:"));
        }

        public static DateTime ToDateTime(this string dateTime)
        {
            dateTime = dateTime.ToNativeDigits(Defaults.CultureName, "en-us");

            return DateTime.Parse(dateTime, Defaults.Culture);
        }

        public static DateTime ToDateTime(this double unixTimeSeconds)
        {
            return new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(unixTimeSeconds);
        }

        public static double ToUnixTimeStamp(this DateTime dateTime)
        {
            return dateTime.Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;
        }

        public static TimeSpan ToTimeSpan(this string value)
        {
            value = NormalizeDateTime(value, ':');

            if (!Regex.Match(value, @"\d{2}:\d{2}:\d{2}", RegexOptions.Compiled).Success)
            {
                throw new FormatException("Value must be in 00:00:00 format.");
            }

            var splited = value.Split(':');

            return new TimeSpan(Convert.ToInt32(splited[0]), Convert.ToInt32(splited[1]), Convert.ToInt32(splited[2]));
        }

        private static string NormalizeDateTime(string value, char separator = '/')
        {
            return value
                .Trim()
                .Replace('\\', separator)
                .Replace('-', separator)
                .Replace('_', separator)
                .Replace(',', separator)
                .Replace('.', separator)
                .Replace(' ', separator)
                .Replace(':', separator);
        }
    }
}
