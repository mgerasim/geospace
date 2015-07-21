using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeospaceCore
{

    public class LoggerNLog : ILogger
    {

        Logger logger;
        Logger error;
        Logger logumagf;
        public LoggerNLog()
        {
            logger = LogManager.GetLogger("log");
            error = LogManager.GetLogger("error");
            logumagf = LogManager.GetLogger("logumagf");
        }
        void ILogger.LogIonka(string msg)
        {
            logger.Debug(msg);
        }

        void ILogger.LogUmagf(string msg)
        {
            logumagf.Debug(msg);
        }
        void ILogger.LogError(string msg)
        {
            error.Debug(msg);
        }
    }
}
