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
                Average average = new Average();
                average.YYYY = end.Year;
                average.MM = end.Month;
                average.DD = end.Day;
                average.HH = hour;
                average.Station = stat;

                end = end.AddDays(-1);
                DateTime start = end.AddDays(-29);
                List<CodeIonka> listIonka = (List<CodeIonka>)CodeIonka.GetByPeriod_StaticHH(stat, start, end, hour);
                listIonka.Reverse();

                for( int i = 0; i < listIonka.Count; i++ )
                {
                    if( i < 5 )
                    {
                        if (listIonka[i].f0F2 < 1000) average.F2_05 += listIonka[i].f0F2 / 5.0;
                        else average.F2_05_skip++;

                        if (listIonka[i].M3000F2 < 1000) average.M3000_05 += listIonka[i].M3000F2 / 5.0;
                        else average.M3000_05_skip++;
                    }

                    if (i < 7)
                    {
                        if (listIonka[i].f0F2 < 1000) average.F2_07 += listIonka[i].f0F2 / 7.0;
                        else average.F2_07_skip++;

                        if (listIonka[i].M3000F2 < 1000) average.M3000_07 += listIonka[i].M3000F2 / 7.0;
                        else average.M3000_07_skip++;
                    }

                    if (i < 10)
                    {
                        if (listIonka[i].f0F2 < 1000) average.F2_10 += listIonka[i].f0F2 / 10.0;
                        else average.F2_10_skip++;

                        if (listIonka[i].M3000F2 < 1000) average.M3000_10 += listIonka[i].M3000F2 / 10.0;
                        else average.M3000_10_skip++;
                    }

                    if (i < 20)
                    {
                        if (listIonka[i].f0F2 < 1000) average.F2_20 += listIonka[i].f0F2 / 20.0;
                        else average.F2_20_skip++;

                        if (listIonka[i].M3000F2 < 1000) average.M3000_20 += listIonka[i].M3000F2 / 20.0;
                        else average.M3000_20_skip++;
                    }

                    if (i < 27)
                    {
                        if (listIonka[i].f0F2 < 1000) average.F2_27 += listIonka[i].f0F2 / 27.0;
                        else average.F2_27_skip++;

                        if (listIonka[i].M3000F2 < 1000) average.M3000_27 += listIonka[i].M3000F2 / 27.0;
                        else average.M3000_27_skip++;
                    }

                    if (i < 30)
                    {
                        if (listIonka[i].f0F2 < 1000) average.F2_30 += listIonka[i].f0F2 / 30.0;
                        else average.F2_30_skip++;

                        if (listIonka[i].M3000F2 < 1000) average.M3000_30 += listIonka[i].M3000F2 /30.0;
                        else average.M3000_30_skip++;
                    }
                }


                if (Average.GetByDateUTC(average.Station, average.YYYY, average.MM, average.DD, average.HH) == null)
                    average.Save();
                //else Console.WriteLine(hour);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
