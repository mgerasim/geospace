using GeospaceEntity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeospaceCore
{
    public class Calculation: ICalculation
    {
       
        ILogger theLog;
        public Calculation(ILogger theLog)
        {
            // TODO: Complete member initialization
            this.theLog = theLog;
        }
        void ICalculation.MedianaCalc_Run()
        {
            theLog.LogCalc("Run logger MedianaCacl");

            
        }

        void ICalculation.AverageCalc_Run()
        {
            theLog.LogCalc("Run logger AverageCacl");

            DateTime dt = DateTime.Now;

            try
            {
                List<Station> listStation = Station.GetAll();
                foreach (Station item in listStation)
                    GeospaceEntity.Helper.HelperCalculation.Start_Calc_Average(dt, item, dt.Hour);

                theLog.LogCalc(dt.ToShortDateString() + " " + dt.ToShortTimeString() + "успешно");
            }
            catch (Exception ex)
            {
                theLog.LogError("Ошибка за " + dt.ToShortDateString()
                    + " " + dt.ToShortTimeString()
                    + "\n" + ex.Message
                    + "'n" + ex.Source
                    + "\n" + ex.StackTrace);
            }

        }
    }
}
