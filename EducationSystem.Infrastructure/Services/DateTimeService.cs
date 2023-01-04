using EducationSystem.Application.Common.Interfaces;

namespace EducationSystem.Infrastructure.Services
{
    public class DateTimeService : IDateTimeService
    {
        public DateTime Now => DateTime.Now;
    }
}
