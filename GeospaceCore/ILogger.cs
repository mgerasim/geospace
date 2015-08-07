using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeospaceCore
{
    public interface ILogger
    {
        void LogIonka(string msg);
        void LogUmagf(string msg);
        void LogError(string msg);
        void LogCalc(string msg);
        void LogMagma(string msg);
    }
}
