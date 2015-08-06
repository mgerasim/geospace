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
        int minusDay;
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

            AMS.Profile.Ini Ini = new AMS.Profile.Ini(AppDomain.CurrentDomain.BaseDirectory + "\\GeospaceCalcService.ini");
            if (!Ini.HasSection("COMMON"))
                Ini.SetValue("COMMON", "MinusDay", 7);
            minusDay = Ini.GetValue("COMMON", "MinusDay", 7);

            DateTime globalNow = DateTime.Now, dt = globalNow;

            try
            {
                List<Station> listStation = Station.GetAll();                

                for (int day = minusDay; day > 0; day--)
                {
                    dt = globalNow;
                    dt = dt.AddDays(-day);
                    foreach (Station item in listStation)
                    {
                        for (int i = 0; i < 24; i++)
                        {
                            GeospaceEntity.Helper.HelperCalculation.Start_Calc_Average(dt, item, i);
                            theLog.LogCalc(item.Name + " - " + item.Code.ToString() + " "
                                + dt.ToShortDateString() + " " + i.ToString() + " час успешно");
                        }                        
                    }
                }
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

        void ICalculation.AverageCalc_Run(DateTime dt)
        {
            theLog.LogCalc("Run logger AverageCacl");

            try
            {
                List<Station> listStation = Station.GetAll();
                foreach (Station item in listStation)                    
                {
                    for (int h = 0; h < dt.Hour; h++)
                    {
                        GeospaceEntity.Helper.HelperCalculation.Start_Calc_Average(dt, item, h);
                    }
                }
                theLog.LogCalc(dt.ToShortDateString() + " " + dt.ToShortTimeString() + " успешно");
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
