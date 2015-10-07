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
        Logger logmagma;
        Logger logUGEOI;
        public LoggerNLog()
        {
            logger = LogManager.GetLogger("log");
            error = LogManager.GetLogger("error");
            logumagf = LogManager.GetLogger("logumagf");
            logmagma = LogManager.GetLogger("logmagma");
            logUGEOI = LogManager.GetLogger("logUGEOI");
        }
        void ILogger.LogIonka(string msg)
        {
            logger.Debug(msg);
        }
        void ILogger.LogCalc(string msg)
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
        void ILogger.LogMagma(string msg)
        {
            logmagma.Debug(msg);
        }
        void ILogger.LogUGEOI(string msg)
        {
            logUGEOI.Debug(msg);
        }

    }
}
