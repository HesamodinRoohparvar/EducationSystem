using EducationSystem.Application.Common.Models;

namespace EducationSystem.Application.Common.Interfaces
{
    public interface ISettingManagerService
    {
        ApplicationSettings Settings { get; }
        Task UpdateAsync(Action<ApplicationSettings> changes);
    }
}
