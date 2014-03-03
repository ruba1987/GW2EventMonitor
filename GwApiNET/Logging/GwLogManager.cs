using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Slf;
using Slf.BitFactoryFacade;
using Slf.Factories;
using Slf.Resolvers;

namespace GwApiNET.Logging
{

    public static class GwLogManager
    {
        static GwLogManager()
        {
            // Create a named resolver that seperate logger retrival by name
            _resolver = new NamedFactoryResolver();
            LoggerService.FactoryResolver = _resolver;
        }

        private static NamedFactoryResolver _resolver;

        internal static void RegisterLogger(string name, ILogger logger)
        {
            _resolver.RegisterFactory(name, new SimpleLoggerFactory(logger));
            logger.Info("Logger Initialized");
        }

        /// <summary>
        /// Enable/Disable the specified log level for the specified logger.
        /// </summary>
        /// <param name="logger">logger to enable/disable</param>
        /// <param name="enabled">true: enable all; false: disable all</param>
        /// <param name="level">log level to enable/disable</param>
        internal static void SetLogLevel(ILogger logger, bool enabled, LogLevel level)
        {
            BitFactoryLogger blogger = logger as BitFactoryLogger;
            if (blogger != null)
            {
                blogger.SetLogLevel(enabled, (Slf.LogLevel) Enum.Parse(typeof (Slf.LogLevel), level.ToString()));
            }
        }
        /// <summary>
        /// Enable/Disable the specified log level for the specified logger.
        /// </summary>
        /// <param name="loggerName">logger to enable/disable</param>
        /// <param name="enabled">true: enable all; false: disable all</param>
        /// <param name="level">log level to enable/disable</param>
        public static void SetLogLevel(string loggerName, bool enabled, LogLevel level)
        {
            SetLogLevel(LoggerService.GetLogger(loggerName), enabled, level);
        }

        /// <summary>
        /// Enable/Disable the specified logger.
        /// </summary>
        /// <param name="loggerName">logger to enable/disable</param>
        /// <param name="enabled">true: enable all; false: disable all</param>
        public static void SetLogLevel(string loggerName, bool enabled)
        {
            foreach (LogLevel level in Enum.GetValues(typeof (LogLevel)))
            {
                SetLogLevel(LoggerService.GetLogger(loggerName), enabled, level);
            }
        }

        /// <summary>
        /// Enable/Disable the specified log level for all loggers.
        /// </summary>
        /// <param name="enable">true: enable all; false: disable all</param>
        /// <param name="level">log level to enable/disable</param>
        public static void SetLogLevel(bool enable, LogLevel level)
        {
            foreach (var logName in Constants.LoggerNames)
            {
                SetLogLevel(logName, enable, level);
            }
        }
        /// <summary>
        /// Enable/Disable all loggers at all log levels
        /// </summary>
        /// <param name="enable">true: enable all; false: disable all</param>
        public static void SetLogLevel(bool enable)
        {
            foreach (var logName in Constants.LoggerNames)
            {
                SetLogLevel(logName, enable);
            }
        }

        public static void TestLogger(string loggerName, string message)
        {
            var logger = LoggerService.GetLogger(loggerName);
            logger.Debug(message);
            logger.Info(message);
            logger.Warn(message);
            logger.Error(message);
            logger.Fatal(message);
        }

        ///<summary>
        /// Defines available log levels (or logging "categories").
        /// Log levels can be used to organize and filter your
        /// logging output.
        ///</summary>
        public enum LogLevel
        {
            /// <summary>
            /// The logging level is undefined. This is regarded
            /// an invalid value.
            /// </summary>
            Undefined = 0,

            /// <summary>
            /// Logs debugging output.
            /// </summary>
            Debug = 1,

            /// <summary>
            /// Logs basic information.
            /// </summary>
            Info = 2,

            /// <summary>
            /// Logs a warning.
            /// </summary>
            Warn = 3,

            /// <summary>
            /// Logs an error.
            /// </summary>
            Error = 4,

            /// <summary>
            /// Logs a fatal incident.
            /// </summary>
            Fatal = 5
        }
    }

}
