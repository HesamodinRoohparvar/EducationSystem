using EducationSystem.Application.Common.Interfaces;
using Loby.Tools;

namespace EducationSystem.Infrastructure.Services
{
    public class EmailNotificationService : INotificationService
    {
        private readonly Mailer _mailer;

        public EmailNotificationService(Mailer mailer)
        {
            _mailer = mailer;
        }

        public async Task SendAsync(string recipient, string subject, string body)
        {
            await _mailer.SendAsync(recipient, subject, body);
        }

        public async Task SendAsync(IEnumerable<string> recipient, string subject, string body)
        {
            await _mailer.SendAsync(recipient, subject, body);
        }
    }
}
