using EducationSystem.Domain.Attributes;
using System.Reflection;

namespace EducationSystem.Application.Common.Extensions
{
    public static class EnumExtensions
    {
        public static string GetLocalizedDescription(this Enum value)
        {
            var attribute = value
                .GetType()
                .GetField(value.ToString())
                .GetCustomAttributes<LocalizedDescriptionAttribute>(false)
                .FirstOrDefault();

            return attribute != null ? attribute.Description : value.ToString();
        }

        public static Dictionary<int, string> ToDictionary(this Enum value)
        {
            return Enum
                .GetValues(value.GetType())
                .Cast<Enum>()
                .ToDictionary(e => Convert.ToInt32(e), e => e.GetLocalizedDescription());
        }

        public static int Count(this Enum value)
        {
            return Enum.GetNames(value.GetType()).Length;
        }
    }
}
