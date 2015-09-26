using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;

namespace PhotoOrganizer.BusinessModule.Log
{
    public class FileLogger : ILogger 
    {
        ILog log;

        private FileLogger(ILog fileLog)
        {
            log = fileLog;
        }

        public static ILogger CreateLogger(string LoggerName)
        {
            return new FileLogger(LogManager.GetLogger(LoggerName));
        }
        
        public void LogDebug(string debugMessage)
        {
            log.Debug(debugMessage);
        }

        public void LogInfo(string infoMessage)
        {
            log.Info(infoMessage);
        }

        public void LogError(string errorMessage)
        {
            log.Error(errorMessage);
        }

        public void LogException(string errorMessage, Exception e)
        {
            log.Error(errorMessage, e);
        }
    }
}
