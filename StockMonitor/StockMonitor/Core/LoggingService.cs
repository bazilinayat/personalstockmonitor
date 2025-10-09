using Serilog;
using System.IO;

namespace StockMonitor.Core
{
    /// <summary>
    /// Service to log everything throughout the application
    /// </summary>
    public class LoggingService
    {
        /// <summary>
        /// The logger object that will be used for logging
        /// </summary>
        private readonly ILogger _logger;

        /// <summary>
        /// The initialization of the service
        /// </summary>
        /// <param name="directory">the directory in which we will be loggging</param>
        public LoggingService(string directory)
        {
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            var logFilePath = Path.Combine(directory, "app-log.json");

            _logger = new LoggerConfiguration()
                .WriteTo.File(
                    path: logFilePath,
                    rollingInterval: RollingInterval.Day,
                    outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level}] {Message:lj}{NewLine}{Exception}",
                    formatProvider: null,
                    restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Information,
                    shared: true
                )
                .CreateLogger();
        }

        /// <summary>
        /// To write information logs
        /// </summary>
        /// <param name="message">Message to log</param>
        public void LogInformation(string message) => _logger.Information(message);

        /// <summary>
        /// To write error logs
        /// </summary>
        /// <param name="message">Message to log</param>
        /// <param name="exception">Exception to log</param>
        public void LogError(string message, Exception? exception = null) => _logger.Error(exception, message);

        /// <summary>
        /// To write debug logs
        /// </summary>
        /// <param name="message">Message to log</param>
        public void LogDebug(string message) => _logger.Debug(message);

        /// <summary>
        /// To write warning logs
        /// </summary>
        /// <param name="message">Message to log</param>
        public void LogWarning(string message) => _logger.Warning(message);
    }
}
