using System.Globalization;
using System.Text;

namespace EducationSystem.Application.Common.Extensions
{
    public static class StringExtentions
    {
        public static byte[] GetUTF8Bytes(this string value)
        {
            return Encoding.UTF8.GetBytes(value);
        }

        public static string ToNativeDigits(this string value, string sourceCultureName, string destinationCultureName)
        {
            if (value is null)
            {
                return null;
            }

            var sourceCulture = CultureInfo.GetCultureInfo(sourceCultureName);
            var destinationCulture = CultureInfo.GetCultureInfo(destinationCultureName);

            for (int i = 0; i <= 9; i++)
            {
                value = value.Replace(sourceCulture.NumberFormat.NativeDigits[i], destinationCulture.NumberFormat.NativeDigits[i]);
            }

            return value;
        }
    }
}
