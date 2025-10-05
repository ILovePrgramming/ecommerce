namespace ECommerce.Services.Logging
{
    using ECommerce.Core.Interfaces;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Provides logging functionality for the application using Microsoft.Extensions.Logging.
    /// </summary>
    public class LoggingService : ILoggingService
    {
        private readonly ILogger<LoggingService> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="LoggingService"/> class.
        /// </summary>
        /// <param name="logger">The logger instance to use for logging.</param>
        public LoggingService(ILogger<LoggingService> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Logs an informational message.
        /// </summary>
        /// <param name="message">The message to log.</param>
        /// <param name="args">Optional arguments for message formatting.</param>
        public void LogInfo(string message, params object[] args)
        {
            _logger.LogInformation(message, args);
        }

        /// <summary>
        /// Logs a warning message.
        /// </summary>
        /// <param name="message">The message to log.</param>
        /// <param name="args">Optional arguments for message formatting.</param>
        public void LogWarning(string message, params object[] args)
        {
            _logger.LogWarning(message, args);
        }

        /// <summary>
        /// Logs an error message with an exception.
        /// </summary>
        /// <param name="message">The message to log.</param>
        /// <param name="ex">The exception to log.</param>
        /// <param name="args">Optional arguments for message formatting.</param>
        public void LogError(string message, Exception ex, params object[] args)
        {
            _logger.LogError(ex, message, args);
        }

        /// <summary>
        /// Logs a debug message.
        /// </summary>
        /// <param name="message">The message to log.</param>
        /// <param name="args">Optional arguments for message formatting.</param>
        public void LogDebug(string message, params object[] args)
        {
            _logger.LogDebug(message, args);
        }
    }
}
