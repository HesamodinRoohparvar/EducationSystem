using EducationSystem.Application.Common.Extensions;
using EducationSystem.Application.Common.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace EducationSystem.Infrastructure.Services
{
    public class FileManagerService : IFileManagerService
    {
        private const string UploadePath = "Uploads";
        private const string WebRootPath = "wwwroot";
        private readonly IDateTimeService _dateTimeService;
        private readonly ILogger<FileManagerService> _logger;
        public FileManagerService(IDateTimeService dateTimeService, ILogger<FileManagerService> logger)
        {
            _dateTimeService = dateTimeService;
            _logger = logger;
        }


        public async Task<string> SaveFileAsync(IFormFile file)
        {
            var path = GenerateFilePath(Path.GetExtension(file.FileName));

            try
            {
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }

                return path.Replace($"{WebRootPath}\\", string.Empty);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "an error occurred while saving file.");

                return null;
            }
        }

        public async Task<string> SaveFileByPathAsync(byte[] fileBytes, string filePath)
        {
            var path = Path.Combine(WebRootPath, filePath);
            var directory = Path.GetDirectoryName(path);

            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            try
            {
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await fileStream.WriteAsync(fileBytes);
                }

                return path.Replace($"{WebRootPath}\\", string.Empty);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "an error occurred while saving file.");

                return null;
            }
        }

        public async Task<string> SaveFileAsync(byte[] fileBytes, string extension = ".png")
        {
            var path = GenerateFilePath(extension);

            try
            {
                using(var fileStream = new FileStream(path , FileMode.Create))
                {
                    await fileStream.WriteAsync(fileBytes);
                }

                return path.Replace($"{WebRootPath}\\", string.Empty);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "an error occurred while saving file.");

                return null;
            }
        }

        public async Task<string> UpdateFileAsync(IFormFile file, string path)
        {
            await DeleteFileAsync(path);

            return await SaveFileAsync(file);
        }

        public Task DeleteFileAsync(string relativePath)
        {
            try
            {
                var dirctory = Path.Combine(WebRootPath, relativePath ?? string.Empty);

                if (File.Exists(relativePath))
                {
                    File.Delete(relativePath);
                }

                return Task.CompletedTask;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "an error occurred while deleting file.");

                return Task.CompletedTask;
            }
        }

        protected string GenerateFilePath(string extension)
        {
            try
            {
                extension = extension.Replace(".", string.Empty);

                var randomString = Guid.NewGuid().ToString().Replace("-", string.Empty);
                var currentYear = _dateTimeService.Now.Format("yyyy");
                var currentMonth = _dateTimeService.Now.Format("MM");
                var currentDay = _dateTimeService.Now.Format("dd");
                var currentDateTime = _dateTimeService.Now.Format("yy-MM-dd-hh-mm-ss");

                var currentYearDirectory = Path.Combine(WebRootPath, UploadePath, $"Year - {currentYear}");

                if (!Directory.Exists(currentYearDirectory))
                {
                    Directory.CreateDirectory(currentYearDirectory);
                }

                var currentMonthDirectory = Path.Combine(currentYearDirectory, $"Day - {currentDay}");

                if (!Directory.Exists(currentMonthDirectory))
                {
                    Directory.CreateDirectory(currentMonthDirectory);
                }

                var currentDayDirectory = Path.Combine(currentMonthDirectory, $"Day - {currentDay}");

                if (!Directory.Exists(currentDayDirectory))
                {
                    Directory.CreateDirectory(currentDayDirectory);
                }

                var fileName = $"{currentDateTime}-{randomString}.{extension}";

                return Path.Combine(currentDayDirectory, fileName);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "an error occurred while generating report image directory and path.");

                return string.Empty;
            }
        }
    }
}
