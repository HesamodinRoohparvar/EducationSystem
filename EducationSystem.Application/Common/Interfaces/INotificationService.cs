namespace EducationSystem.Application.Common.Interfaces
{
    public interface INotificationService
    {
        Task SendAsync(string recipient, string subject, string body);
        Task SendAsync(IEnumerable<string> recipient, string subject, string body);
    }
}
