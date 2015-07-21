using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeospaceCore
{
    public class Decode:IDecode
    {
        ILogger logger;
        public Decode(ILogger logger)
        {
            this.logger = logger;
        }
        void IDecode.Run(string fileName)
        {
            try
            {
                logger.LogIonka("Run logger ionka");
                logger.LogUmagf("Run logger umagf");
            }
            catch
            {
                logger.LogError("Error Global");
            }
        }
    }
}
