using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeospaceEntity.Models.Codes;
using GeospaceEntity.Models;

namespace GeospaceEntity.Helper
{
    public class DateTimeKhabarovsk
    {
        private static TimeZoneInfo _vladZone = null;

        public static DateTime Now
        {
            get
            {
                if (_vladZone == null)
                {
                    _vladZone = TimeZoneInfo.FindSystemTimeZoneById("Vladivostok Standard Time");
                }

                return TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.Local, _vladZone);
            }

        }
    }

    public class Range
    {
        public Range()
        { }

        public Range(Range range)
        {
            Min = range.Min;
            Max = range.Max;
        }

        public string Header
        {
            get
            {
                return Min.ToString() + "-" + Max.ToString();
            }
        }

        public int Min;
        public int Max;
    }

    public static class HelperCalculation
    {
       
        private static int[] _calcDays = null;

        private static List<int> getValuesByRange(IList<CodeIonka> codesIonka, string type, int hour, DateTime startDate, DateTime endDate)
        {
            List<int> listValues = new List<int>();

            for (var date = startDate; date <= endDate; date = date.AddDays(1))
            {
                CodeIonka codeIonka;

                try
                {
                    codeIonka = codesIonka.Where(x => x.YYYY == date.Year && x.MM == date.Month && x.DD == date.Day && x.HH == hour)
                        .OrderBy(x => x.MI)
                        .ToList()[0];
                }
                catch (Exception)
                {
                    continue;
                }

                int value = 0;

                switch (type)
                {
                    case "f0F2":
                        value = codeIonka.f0F2;
                        break;
                    case "M3000F2":
                        value = codeIonka.M3000F2;
                        break;
                }

                if (value < 1000)
                {
                    listValues.Add(value);
                }

            }

            return listValues;
        }
        public static DateTime GetCalcDateBySeq(DateTime month, int seqNumber)
        {
            if (_calcDays == null)
            {
                _calcDays = new int[6];

                for (int i = 0; i < 6; i++)
                {
                    _calcDays[i] = 5 * i + 4;
                }
            }

            int countDays = DateTime.DaysInMonth(month.Year, month.Month);

            int day = _calcDays[seqNumber];

            if (countDays == 31)
            {
                if (seqNumber == 5)
                {
                    day = 30;
                }
            }

            if (month.Month == 2)
            {
                if (seqNumber == 5)
                {
                    if (countDays == 28)
                    {
                        day = 27;
                    }
                    else
                    {
                        day = 28;
                    }
                }
            }

            return new DateTime(month.Year, month.Month, day);
        }

        //расчет медианы
        public static void Calc_Mediana(Station station, int year, int month, string type)
        {
            DateTime curMonth = new DateTime(year, month, DateTime.DaysInMonth(year, month));
            DateTime tmpPrevMonth = curMonth.AddMonths(-1);
            DateTime prevMonth = new DateTime(tmpPrevMonth.Year, tmpPrevMonth.Month, 1);
            DateTime nextMonth = prevMonth.AddMonths(2);

            DateTime nowDateTime = DateTimeKhabarovsk.Now;

            var codesIonka = CodeIonka.GetByPeriod(station, prevMonth, curMonth);

            int countDays = DateTime.DaysInMonth(year, month);

            for (int i = 1; i < 7; i++)
            {
                DateTime calcDate = GetCalcDateBySeq(curMonth, i - 1);

                int[] medians = Enumerable.Repeat(-1, 24).ToArray();

                if (calcDate <= nowDateTime)
                {
                    for (int hour = 0; hour < 24; hour++)
                    {
                        DateTime endRange = calcDate.AddDays(-1);

                        List<int> listValues = getValuesByRange(codesIonka, type, hour, endRange.AddDays(-9), endRange);

                        listValues.Sort();

                        if (listValues.Count != 0)
                        {
                            if (listValues.Count == 1)
                            {
                                medians[hour] = listValues[0];
                            }
                            else if (listValues.Count % 2 == 0)
                            {
                                int index1 = listValues.Count / 2 - 1;
                                int index2 = listValues.Count / 2;

                                medians[hour] = (int)Math.Ceiling((listValues[index1] + listValues[index2]) / 2.0);
                            }
                            else
                            {
                                int index = listValues.Count / 2;

                                medians[hour] = listValues[index];
                            }
                        }
                    }
                }

                DateTime medianaDate = curMonth;

                int rangeNumber = i;

                if (i == 6)
                {
                    medianaDate = nextMonth;
                    rangeNumber = 0;
                }

                for (int hour = 0; hour < 24; hour++)
                {
                    if (medians[hour] == -1)
                        continue;

                    Mediana mediana = Mediana.GetByDate(station, medianaDate.Year, medianaDate.Month, hour, rangeNumber);
                    mediana.Station = station;
                    mediana.YYYY = medianaDate.Year;
                    mediana.MM = medianaDate.Month;
                    mediana.HH = hour;
                    mediana.RangeNumber = rangeNumber;

                    switch (type)
                    {
                        case "f0F2":
                            if (mediana.IsFixed)
                                continue;

                            mediana.f0F2 = medians[hour];
                            break;
                        case "M3000F2":
                            mediana.M3000F2 = medians[hour];
                            break;
                    }

                    if (mediana.ID < 0)
                        mediana.Save();
                    else
                        mediana.Update();
                }
            }
        }

        //расчет средних значений f0F2 и M3000
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

                

                end = end.AddDays(-1);
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

                if( average.F2_05 != 0 ) average.F2_05 /= (int)Math.Round( (5.0 - average.F2_05_skip), 0 );
                if (average.M3000_05 != 0) average.M3000_05 /= (int)Math.Round((5.0 - average.M3000_05_skip), 0);

                if (average.F2_07 != 0) average.F2_07 /= (int)Math.Round((7.0 - average.F2_07_skip), 0);
                if (average.M3000_07 != 0) average.M3000_07 /= (int)Math.Round((7.0 - average.M3000_07_skip), 0);

                if (average.F2_10 != 0) average.F2_10 /= (int)Math.Round((10.0 - average.F2_10_skip), 0);
                if (average.M3000_10 != 0) average.M3000_10 /= (int)Math.Round((10.0 - average.M3000_10_skip), 0);

                if (average.F2_20 != 0) average.F2_20 /= (int)Math.Round((20.0 - average.F2_20_skip), 0);
                if (average.M3000_20 != 0) average.M3000_20 /= (int)Math.Round((20.0 - average.M3000_20_skip), 0);

                if (average.F2_27 != 0) average.F2_27 /= (int)Math.Round((27.0 - average.F2_27_skip), 0);
                if (average.M3000_27 != 0) average.M3000_27 /= (int)Math.Round((27.0 - average.M3000_27_skip), 0);

                if (average.F2_30 != 0) average.F2_30 /= (int)Math.Round((30.0 - average.F2_30_skip), 0);
                if (average.M3000_30 != 0) average.M3000_30 /= (int)Math.Round((30.0 - average.M3000_30_skip), 0);

                average.Update();
            }
            catch(Exception ex)
            {
                throw new Exception(stat.Name + stat.Code.ToString() + "Неизвестная ошибка при подсчете среднего", ex);
            }
        }
    }
}
