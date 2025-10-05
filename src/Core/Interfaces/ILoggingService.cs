namespace ECommerce.Core.Interfaces
{
    /// <summary>
    /// Provides logging functionality for different log levels.
    /// </summary>
    public interface ILoggingService
    {
        /// <summary>
        /// Logs an informational message.
        /// </summary>
        /// <param name="message">The message to log.</param>
        /// <param name="args">Optional arguments for formatting the message.</param>
        void LogInfo(string message, params object[] args);

        /// <summary>
        /// Logs a warning message.
        /// </summary>
        /// <param name="message">The message to log.</param>
        /// <param name="args">Optional arguments for formatting the message.</param>
        void LogWarning(string message, params object[] args);

        /// <summary>
        /// Logs an error message with an exception.
        /// </summary>
        /// <param name="message">The message to log.</param>
        /// <param name="ex">The exception associated with the error.</param>
        /// <param name="args">Optional arguments for formatting the message.</param>
        void LogError(string message, Exception ex, params object[] args);

        /// <summary>
        /// Logs a debug message.
        /// </summary>
        /// <param name="message">The message to log.</param>
        /// <param name="args">Optional arguments for formatting the message.</param>
        void LogDebug(string message, params object[] args);
    }
}
