using EducationSystem.Application.Common.Interfaces;
using EducationSystem.Application.Common.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace EducationSystem.Infrastructure.Services
{
    public class SettingManagerService : ISettingManagerService
    {
        private readonly IConfigurationRoot _configurationRoot;
        private readonly string _fileName = "appsettings.json";
        private readonly ILogger<SettingManagerService> _logger;
        private readonly IOptionsMonitor<ApplicationSettings> _options;
        private static JsonSerializerOptions _serializerOptions = new()
        {
            WriteIndented = true
        };

        public SettingManagerService(IConfigurationRoot configurationRoot, ILogger<SettingManagerService> logger,
            IOptionsMonitor<ApplicationSettings> options)
        {
            _configurationRoot = configurationRoot;
            _logger = logger;
            _options = options;
        }

        public ApplicationSettings Settings => _options.CurrentValue;

        public async Task UpdateAsync(Action<ApplicationSettings> changes)
        {
            var fileInfo = new FileInfo(_fileName);
            var fileContent = await File.ReadAllTextAsync(fileInfo.FullName);

            var jdoc = JsonDocument.Parse(fileContent);
            var jnode = jdoc.RootElement.Deserialize<ApplicationSettings>();

            changes(jnode);

            var result = JsonSerializer.Serialize(jnode, _serializerOptions);

            await File.WriteAllTextAsync(fileInfo.FullName, result);

            _configurationRoot.Reload();
        }
    }
}
