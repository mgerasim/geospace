using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeospaceEntity.Models.Codes;
using GeospaceEntity.Models;

namespace GeospaceEntity.Helper
{
    public static class HelperCalculation
    {
        public static void Start_Calc_Average(DateTime end, Station stat, int hour)
        {
            try
            {
                Average averageProve = Average.GetByDateUTC(stat, end.Year, end.Month, end.Day, hour);
                Average average = new Average();

                average.YYYY = end.Year;
                average.MM = end.Month;
                average.DD = end.Day;
                average.HH = hour;
                average.Station = stat;

                if (averageProve == null)
                {                    
                    average.Save();                    
                }
                else
                {
                    average.created_at = averageProve.created_at;
                    average.ID = averageProve.ID;
                }

                

                //end = end.AddDays(-1);
                DateTime start = end.AddDays(-29);
                List<CodeIonka> listIonka = (List<CodeIonka>)CodeIonka.GetByPeriod_StaticHH(stat, start, end, hour);
                listIonka.Reverse();

                for( int i = 0; i < listIonka.Count; i++ )
                {
                    if( i < 5 )
                    {
                        if (listIonka[i].f0F2 < 1000) average.F2_05 += listIonka[i].f0F2;
                        else average.F2_05_skip++;

                        if (listIonka[i].M3000F2 < 1000) average.M3000_05 += listIonka[i].M3000F2;
                        else average.M3000_05_skip++;
                    }

                    if (i < 7)
                    {
                        if (listIonka[i].f0F2 < 1000) average.F2_07 += listIonka[i].f0F2;
                        else average.F2_07_skip++;

                        if (listIonka[i].M3000F2 < 1000) average.M3000_07 += listIonka[i].M3000F2;
                        else average.M3000_07_skip++;
                    }

                    if (i < 10)
                    {
                        if (listIonka[i].f0F2 < 1000) average.F2_10 += listIonka[i].f0F2;
                        else average.F2_10_skip++;

                        if (listIonka[i].M3000F2 < 1000) average.M3000_10 += listIonka[i].M3000F2;
                        else average.M3000_10_skip++;
                    }

                    if (i < 20)
                    {
                        if (listIonka[i].f0F2 < 1000) average.F2_20 += listIonka[i].f0F2;
                        else average.F2_20_skip++;

                        if (listIonka[i].M3000F2 < 1000) average.M3000_20 += listIonka[i].M3000F2;
                        else average.M3000_20_skip++;
                    }

                    if (i < 27)
                    {
                        if (listIonka[i].f0F2 < 1000) average.F2_27 += listIonka[i].f0F2;
                        else average.F2_27_skip++;

                        if (listIonka[i].M3000F2 < 1000) average.M3000_27 += listIonka[i].M3000F2;
                        else average.M3000_27_skip++;
                    }

                    if (i < 30)
                    {
                        if (listIonka[i].f0F2 < 1000) average.F2_30 += listIonka[i].f0F2;
                        else average.F2_30_skip++;

                        if (listIonka[i].M3000F2 < 1000) average.M3000_30 += listIonka[i].M3000F2;
                        else average.M3000_30_skip++;
                    }
                }

                if( average.F2_05 != 0 ) average.F2_05 /= (5.0 - average.F2_05_skip);
                if( average.M3000_05 != 0 ) average.M3000_05 /= (5.0 - average.M3000_05_skip);

                if( average.F2_07 != 0 ) average.F2_07 /= (7.0-average.F2_07_skip);
                if( average.M3000_07 != 0 ) average.M3000_07 /= (7.0-average.M3000_07_skip);

                if( average.F2_10 != 0 ) average.F2_10 /= (10.0-average.F2_10_skip);
                if( average.M3000_10 != 0 ) average.M3000_10 /= (10.0-average.M3000_10_skip);

                if( average.F2_20 != 0 ) average.F2_20 /= (20.0-average.F2_20_skip);
                if( average.M3000_20 != 0 ) average.M3000_20 /= (20.0-average.M3000_20_skip);

                if( average.F2_27 != 0 ) average.F2_27 /= (27.0-average.F2_27_skip);
                if( average.M3000_27 != 0 ) average.M3000_27 /= (27.0-average.M3000_27_skip);

                if( average.F2_30 != 0 ) average.F2_30 /= (30.0-average.F2_30_skip);
                if (average.M3000_30 != 0) average.M3000_30 /= (30.0 - average.M3000_30_skip);

                average.Update();
            }
            catch(Exception ex)
            {
                throw new Exception(stat.Name + stat.Code.ToString() + "Неизвестная ошибка при подсчете среднего", ex);
            }
        }
    }
}
