using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeospaceCore
{
    public class LoggerCalc:ILogger
    {
        Logger logger;
        Logger error;
        public LoggerCalc(string logName, string errorName)
        {
            logger = LogManager.GetLogger(logName);
            error = LogManager.GetLogger(errorName);
        }
        void ILogger.LogCalc(string msg)
        {
            logger.Debug(msg);
        }
        void ILogger.LogIonka(string msg)
        {
            error.Debug(msg);
        }
        void ILogger.LogError(string msg)
        {
            error.Debug(msg);
        }
        void ILogger.LogUmagf(string msg)
        {
            logger.Debug(msg);
        }
    }
}

