namespace EducationSystem.Application.Common.Constans
{
    public class DefaultRoleName
    {
        public const string Admin = "مدیر";
        public const string Teacher = "استاد";
        public const string Student = "دانشجو";

        public static List<string> List => new List<string>
        {
            Admin,
            Teacher,
            Student
        };
    }
}
