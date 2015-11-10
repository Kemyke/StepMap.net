using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StepMap.Logger.Logging
{
    /// <summary>
    /// Interface for logging.
    /// </summary>
    public interface ILogger
    {
        /// <summary>
        /// Writes a message to the debug log.
        /// </summary>
        /// <param name="format">Format string for the message.</param>
        /// <param name="args">Parameters of the format string.</param>
        void Debug(string format, params object[] args);

        /// <summary>
        /// Writes a message to the information log.
        /// </summary>
        /// <param name="format">Format string for the message.</param>
        /// <param name="args">Parameters of the format string.</param>
        void Info(string format, params object[] args);

        /// <summary>
        /// Writes a custom event to the information log.
        /// </summary>
        /// <param name="logEvent">Log event.</param>
        void Info(LogEventBase logEvent);

        /// <summary>
        /// Writes a message to the warning log.
        /// </summary>
        /// <param name="format">Format string for the message.</param>
        /// <param name="args">Parameters of the format string.</param>
        void Warning(string format, params object[] args);

        /// <summary>
        /// Writes a message to the warning log.
        /// </summary>
        /// <param name="error">Exception to log.</param>
        /// <param name="format">Format string for the message.</param>
        /// <param name="args">Parameters of the format string.</param>
        void Warning(Exception error, string format, params object[] args);

        /// <summary>
        /// Writes a message to the error log.
        /// </summary>
        /// <param name="format">Format string for the message.</param>
        /// <param name="args">Parameters of the format string.</param>
        void Error(string format, params object[] args);

        /// <summary>
        /// Writes a message to the error log.
        /// </summary>
        /// <param name="error">Exception to log.</param>
        /// <param name="messageToFormat">Format string for the message.</param>
        /// <param name="args">Parameters of the format string.</param>
        void Error(Exception error, string format, params object[] args);

        /// <summary>
        /// Writes a message to the fatal log.
        /// </summary>
        /// <param name="format">Format string for the message.</param>
        /// <param name="args">Parameters of the format string.</param>
        void Fatal(string format, params object[] args);

        /// <summary>
        /// Writes a message to the fatal log.
        /// </summary>
        /// <param name="error">Exception to log.</param>
        /// <param name="messageToFormat">Format string for the message.</param>
        /// <param name="args">Parameters of the format string.</param>
        void Fatal(Exception error, string format, params object[] args);
    }
}
