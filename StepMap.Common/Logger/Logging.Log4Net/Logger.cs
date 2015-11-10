using log4net;
using log4net.Core;
using log4net.Repository;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace StepMap.Logger.Logging.Log4Net
{
    /// <summary>
    /// Log4Net implementation of the ILogger interface.
    /// </summary>
    public class Logger : ILogger
    {
        private const string LoggerName = "StepMap.Logger.Logging.Log4Net.Logger";
        private const string LogEventLoggerName = "StepMap.Logger.Logging.Log4Net.LogEventLogger";

        private readonly ILog log4netLog = null;
        private readonly ILog log4netLogEventLog = null;
        private readonly ConcurrentDictionary<Type, PropertyInfo[]> logEventPropertyCache = null;

        public Logger()
        {
            // read the configuraton
            log4net.Config.XmlConfigurator.Configure();

            // retrive the base logger
            log4netLog = LogManager.GetLogger(LoggerName);
            // retrive the logEvent logger
            log4netLogEventLog = LogManager.GetLogger(LogEventLoggerName);

            logEventPropertyCache = new ConcurrentDictionary<Type, PropertyInfo[]>();
        }

        private IDictionary<string, object> GetLogEventProperties(LogEventBase logEvent)
        {
            Dictionary<string, object> propValues = new Dictionary<string, object>();

            PropertyInfo[] props;
            Type type = logEvent.GetType();

            if (!logEventPropertyCache.TryGetValue(type, out props))
            {
                props = type.GetProperties();
                logEventPropertyCache.TryAdd(type, props);
            }

            foreach (var prop in props)
            {
                object value = prop.GetValue(logEvent);
                propValues.Add(prop.Name, value);
            }

            return propValues;
        }

        #region ILogger Members

        public void Debug(string format, params object[] args)
        {
            if (format == null)
            {
                throw new ArgumentNullException("format");
            }

            if (log4netLog.IsDebugEnabled)
            {
                log4netLog.DebugFormat(format, args);
            }
        }

        public void Info(string format, params object[] args)
        {
            if (format == null)
            {
                throw new ArgumentNullException("format");
            }

            if (log4netLog.IsInfoEnabled)
            {
                log4netLog.InfoFormat(format, args);
            }
        }

        public void Info(LogEventBase logEvent)
        {
            if (logEvent == null)
            {
                throw new ArgumentNullException("logEvent");
            }

            if (log4netLogEventLog.IsInfoEnabled)
            {
                logEvent.Logged = DateTime.UtcNow;

                IDictionary<string, object> properties = GetLogEventProperties(logEvent);                

                LoggingEventData loggingEventData = new LoggingEventData();
                loggingEventData.Level = Level.Info;
                loggingEventData.LoggerName = log4netLogEventLog.Logger.Name;
                loggingEventData.TimeStamp = logEvent.Logged;

                LoggingEvent loggingEvent = new LoggingEvent(loggingEventData);

                foreach (var kvp in properties)
                {
                    loggingEvent.Properties[kvp.Key] = kvp.Value;
                }

                log4netLogEventLog.Logger.Log(loggingEvent);
            }
        }

        public void Warning(string format, params object[] args)
        {
            if (format == null)
            {
                throw new ArgumentNullException("format");
            }

            if (log4netLog.IsWarnEnabled)
            {
                log4netLog.WarnFormat(format, args);
            }
        }

        public void Warning(Exception error, string format, params object[] args)
        {
            if (error == null)
            {
                throw new ArgumentNullException("error");
            }

            if (format == null)
            {
                throw new ArgumentNullException("format");
            }

            if (log4netLog.IsWarnEnabled)
            {
                string msg = string.Format(format, args);
                log4netLog.Warn(msg, error);
            }
        }

        public void Error(string format, params object[] args)
        {            
            if (format == null)
            {
                throw new ArgumentNullException("format");
            }

            if (log4netLog.IsErrorEnabled)
            {
                log4netLog.ErrorFormat(format, args);
            }
        }

        public void Error(Exception error, string format, params object[] args)
        {
            if (error == null)
            {
                throw new ArgumentNullException("error");
            }

            if (format == null)
            {
                throw new ArgumentNullException("format");
            }

            if (log4netLog.IsErrorEnabled)
            {
                string msg = string.Format(format, args);
                log4netLog.Error(msg, error);
            }
        }

        public void Fatal(string format, params object[] args)
        {            
            if (format == null)
            {
                throw new ArgumentNullException("format");
            }

            if (log4netLog.IsFatalEnabled)
            {
                log4netLog.FatalFormat(format, args);
            }
        }

        public void Fatal(Exception error, string format, params object[] args)
        {
            if (error == null)
            {
                throw new ArgumentNullException("error");
            }

            if (format == null)
            {
                throw new ArgumentNullException("format");
            }

            if (log4netLog.IsFatalEnabled)
            {
                string msg = string.Format(format, args);
                log4netLog.Fatal(msg, error);
            }
        }

        #endregion
    }
}
