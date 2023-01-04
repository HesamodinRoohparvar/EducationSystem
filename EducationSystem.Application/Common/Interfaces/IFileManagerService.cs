using Microsoft.AspNetCore.Http;

namespace EducationSystem.Application.Common.Interfaces
{
    public interface IFileManagerService
    {
        Task<string> SaveFileAsync(IFormFile file);
        Task<string> SaveFileByPathAsync(byte[] fileBytes, string filePath);
        Task<string> SaveFileAsync(byte[] fileBytes, string extension = ".png");
        Task<string> UpdateFileAsync(IFormFile file, string path);
        Task DeleteFileAsync(string relativePath);
    }
}
