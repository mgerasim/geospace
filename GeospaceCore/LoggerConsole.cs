using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeospaceCore
{
    public class LoggerConsole:ILogger
    {
        void ILogger.LogIonka(string msg)
        {
            Console.WriteLine(msg);
        }

        void ILogger.LogUmagf(string msg)
        {
            Console.WriteLine(msg);
        }
        void ILogger.LogError(string msg)
        {
            Console.WriteLine(msg);
        }
        void ILogger.LogCalc(string msg)
        {
            Console.WriteLine(msg);
        }

        void ILogger.LogMagma(string msg)
        {
            Console.WriteLine(msg);
        }
    }
}
