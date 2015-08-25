using GeospaceEntity.Helper;
using GeospaceEntity.Models;
using System;
using System.Collections.Generic;
using System.IO;
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
            DateTime dt = DateTime.Now;
            try
            {
                List<Station> listStation = Station.GetAll();
                foreach (Station item in listStation)
                {
                    GeospaceEntity.Helper.HelperCalculation.Calc_Mediana(item, dt.Year, dt.Month, "f0F2");
                    theLog.LogCalc(item.Name + " - " + item.Code.ToString() + " "
                        + dt.ToShortDateString() + " f0F2 успешно");
                    GeospaceEntity.Helper.HelperCalculation.Calc_Mediana(item, dt.Year, dt.Month, "M3000F2");
                    theLog.LogCalc(item.Name + " - " + item.Code.ToString() + " "
                        + dt.ToShortDateString() + " M3000F2 успешно");
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

        void ICalculation.DisturbanceCalc()
        {
            DateTime currDate = DateTime.Now;
            foreach (var station in Station.GetAll())
            {
                try
                {
                    HelperCalculation.DisturbanceCalc(station, currDate);
                }
                catch (Exception ex)
                {
                    theLog.LogError(ex.Message);
                }
            }
        }
        void ICalculation.DisturbanceCalc(DateTime bgnDate, DateTime endDate)
        {
            foreach (var station in Station.GetAll())
            {
                for (DateTime currDate = bgnDate; currDate < endDate.AddDays(1); currDate = currDate.AddDays(1))
                {

                    try
                    {
                        HelperCalculation.DisturbanceCalc(station, currDate);
                    }
                    catch (Exception ex)
                    {
                        theLog.LogError(ex.Message);
                    }
                }
            }
        }
        void ICalculation.CharacterizationCalc()
        {
            DateTime currDate = DateTime.Now;
            foreach (var station in Station.GetAll())
            {
                try
                {
                    HelperCalculation.CharacterizationCalc(station, currDate);
                }
                catch (Exception ex)
                {
                    theLog.LogError(ex.Message);
                }
            }
        }
        void ICalculation.CharacterizationCalc(DateTime bgnDate, DateTime endDate)
        {
            foreach (var station in Station.GetAll())
            {
                for (DateTime currDate = bgnDate; currDate < endDate.AddDays(1); currDate = currDate.AddDays(1))
                {

                    try
                    {
                        HelperCalculation.CharacterizationCalc(station, currDate);
                    }
                    catch (Exception ex)
                    {
                        theLog.LogError(ex.Message);
                    }
                }
            }
               
            
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

                for (int day = minusDay; day >= 0; day--)
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
                        GeospaceEntity.Helper.HelperCalculation.interpolation( dt, item);
                    }
                }
            }
            catch (Exception ex)
            {
                theLog.LogError(dt.ToShortDateString()
                    + " " + dt.ToShortTimeString()
                    + "\n" + ex.Message
                    + "'n" + ex.Source
                    + "\n" + ex.StackTrace);
            }
        }

        void ICalculation.AverageCalc_Run(DateTime start, DateTime end)
        {
            theLog.LogCalc("Run logger AverageCacl");
            DateTime dt = start;
            int count = 0;

            try
            {
                List<Station> listStation = Station.GetAll();
                while(true)
                {                    
                    dt = start.AddDays(count);
                    foreach (Station item in listStation)
                    {
                        for (int h = start.Hour; h <= end.Hour; h++)
                        {
                            GeospaceEntity.Helper.HelperCalculation.Start_Calc_Average(dt, item, h);
                            theLog.LogCalc("Average " + item.Name + " - " + item.Code.ToString() + " "
                                + dt.ToShortDateString() + " " + h.ToString() + " час успешно");
                        }
                        GeospaceEntity.Helper.HelperCalculation.interpolation(dt, item);
                    }
                    Console.WriteLine(dt.ToShortDateString());

                    if (dt.Year >= end.Year && dt.Month >= end.Month && dt.Day >= end.Day ) break;
                    count++;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Average Ошибка за " + dt.ToShortDateString()
                    + " " + dt.ToShortTimeString()
                    + "\n" + ex.Message
                    + "'n" + ex.Source
                    + "\n" + ex.StackTrace);
            }

        }
    }
}
