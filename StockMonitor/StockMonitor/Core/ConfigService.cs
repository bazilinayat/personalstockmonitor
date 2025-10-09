using Microsoft.Extensions.Options;

namespace StockMonitor.Core
{
    /// <summary>
    /// To store the information of AppConfig section from appsettings.json
    /// </summary>
    public class AppConfig
    {
        public string ApiBaseUrl { get; set; }
        public string Environment { get; set; }
    }

    /// <summary>
    /// To store the information of whole appsettings.json
    /// </summary>
    public class ConfigService
    {
        private readonly AppConfig _config;

        public ConfigService(IOptions<AppConfig> options)
        {
            _config = options.Value;
        }

        public string ApiBaseUrl => _config.ApiBaseUrl;
        public string Environment => _config.Environment;
    }
}
