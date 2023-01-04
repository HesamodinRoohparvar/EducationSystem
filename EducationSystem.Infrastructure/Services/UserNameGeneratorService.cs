using EducationSystem.Application.Common.Extensions;
using EducationSystem.Application.Common.Interfaces;

namespace EducationSystem.Infrastructure.Services
{
    public class UserNameGeneratorService :IUserNameGeneratorService
    {
        private readonly IDateTimeService _dateTimeService;

        public UserNameGeneratorService(IDateTimeService dateTimeService)
        {
            _dateTimeService = dateTimeService;
        }

        public string GenerateUserName(int academicFieldId, int userCount)
        {
            var currentDate = _dateTimeService.Now.Format("yyyy");

            var Id = academicFieldId.ToString("000");

            var userNumber = (userCount + 1).ToString("000");

            return $"{currentDate}{Id}{userNumber}";
        }
    }
}
