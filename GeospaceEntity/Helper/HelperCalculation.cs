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
        public static void DisturbanceCalc(Station station, DateTime currDate)
        {
            List<CodeIonka> theCodeList = CodeIonka.GetByDate(station, currDate.Year, currDate.Month, currDate.Day);
            foreach (var theCode in theCodeList)
            {
                try
                {


                    if (
                        theCode.MI == 0 && (
                        theCode.f0F2 == 1002            // B - неотклоняющее поглащение
                            || theCode.f0F2 == 1006     // F - рассеяный след (диффузность)
                           ||theCode.delta_f0F2 < -30       // отрицательные суточное отклонение
                        )
                    )
                    {
                        Disturbance theDisturbance = null;
                        theDisturbance = Disturbance.GetByTime(station, currDate.Year, currDate.Month, currDate.Day, theCode.HH);
                        if (theDisturbance == null)
                        {
                            theDisturbance = new Disturbance();
                        }
                        theDisturbance.Station = station;
                        theDisturbance.YYYY = currDate.Year;
                        theDisturbance.MM = currDate.Month;
                        theDisturbance.DD = currDate.Day;
                        theDisturbance.HH = theCode.HH;
                        theDisturbance.MI = theCode.MI;
                        if (theDisturbance.ID < 0)
                        {
                            theDisturbance.Save();
                        }
                        else
                        {
                            theDisturbance.Update();
                        }

                    }
                   
                } 
                catch (Exception ex){
                        //Console.WriteLine(ex.Message);
                        //if (ex.InnerException != null) {
                        //    Console.WriteLine(ex.InnerException.Message);
                        //}
                    }
            }
        }
        public static void CharacterizationCalc(Station station, DateTime currDate)
        {
            int rangeNumber = MedianaCalculator.GetRangeFromDateReal(currDate);
            CharacterizationDay theCharacterizationDay_f0F2 = new CharacterizationDay(station, rangeNumber, currDate.Year, currDate.Month, "f0F2");
            CharacterizationDay theCharacterizationDay_M3000 = new CharacterizationDay(station, rangeNumber, currDate.Year, currDate.Month, "M3000");
            
            List<CodeIonka> theCodeList = CodeIonka.GetByDate(station, currDate.Year, currDate.Month, currDate.Day);
            foreach (var theCode in theCodeList)
            {
                try
                {
                    var value = theCharacterizationDay_f0F2.GetValues().Where(x => x.Day == theCode.DD && x.Hour == theCode.HH).Select(x => x.PrevRating).First();
                    if (value != null)
                    {
                        theCode.delta_f0F2 = (int)value;                        
                    }
                }
                catch{

                }

                try {
                    var value2 = theCharacterizationDay_f0F2.GetValues().Where(x => x.Day == theCode.DD && x.Hour == theCode.HH).Select(x => x._Rating).First();
                    if (value2 != null)
                    {
                        theCode.rating_f0F2 = Convert.ToDouble(value2);

                    }

                }
                catch{}

                
                try {
                    var value = theCharacterizationDay_M3000.GetValues().Where(x => x.Day == theCode.DD && x.Hour == theCode.HH).Select(x => x.PrevRating).First();
                    if (value != null)
                    {
                        theCode.delta_M3000 = value;

                    }

                }
                catch{}
                    

                
                try {
                    var value = theCharacterizationDay_M3000.GetValues().Where(x => x.Day == theCode.DD && x.Hour == theCode.HH).Select(x => x._Rating).First();
                    if (value != null)
                    {
                        theCode.rating_M3000 = Convert.ToDouble(value);

                    }

                }
                catch{}

               theCode.Update();
                
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

                if (listIonka.Count < 3)
                {
                    average.F2_05 = 1000;
                    average.F2_07 = 1000;
                    average.F2_10 = 1000;
                    average.F2_20 = 1000;
                    average.F2_27 = 1000;
                    average.F2_30 = 1000;

                    average.M3000_05 = 1000;
                    average.M3000_07 = 1000;
                    average.M3000_10 = 1000;
                    average.M3000_20 = 1000;
                    average.M3000_27 = 1000;
                    average.M3000_30 = 1000;

                    average.Update();
                    return;
                }

                listIonka.Reverse();

                for( int i = 0; i < listIonka.Count; i++ )
                {
                    if ((end.AddDays(-i)).Day == listIonka[i].DD)
                    {
                        if( i < 5 )
                        {                        
                            if (listIonka[i].f0F2 < 1000 && listIonka[i].f0F2 > 0) average.F2_05 += listIonka[i].f0F2;
                            else average.F2_05_skip++;

                            if (listIonka[i].M3000F2 < 1000 && listIonka[i].M3000F2 > 0) average.M3000_05 += listIonka[i].M3000F2;
                            else average.M3000_05_skip++;                        
                        }

                        if (i < 7)
                        {
                            if (listIonka[i].f0F2 < 1000 && listIonka[i].f0F2 > 0) average.F2_07 += listIonka[i].f0F2;
                            else average.F2_07_skip++;

                            if (listIonka[i].M3000F2 < 1000 && listIonka[i].M3000F2 > 0) average.M3000_07 += listIonka[i].M3000F2;
                            else average.M3000_07_skip++;
                        }

                        if (i < 10)
                        {
                            if (listIonka[i].f0F2 < 1000 && listIonka[i].f0F2 > 0) average.F2_10 += listIonka[i].f0F2;
                            else average.F2_10_skip++;

                            if (listIonka[i].M3000F2 < 1000 && listIonka[i].M3000F2 > 0) average.M3000_10 += listIonka[i].M3000F2;
                            else average.M3000_10_skip++;
                        }

                        if (i < 20)
                        {
                            if (listIonka[i].f0F2 < 1000 && listIonka[i].f0F2 > 0) average.F2_20 += listIonka[i].f0F2;
                            else average.F2_20_skip++;

                            if (listIonka[i].M3000F2 < 1000 && listIonka[i].M3000F2 > 0) average.M3000_20 += listIonka[i].M3000F2;
                            else average.M3000_20_skip++;
                        }

                        if (i < 27)
                        {
                            if (listIonka[i].f0F2 < 1000 && listIonka[i].f0F2 > 0) average.F2_27 += listIonka[i].f0F2;
                            else average.F2_27_skip++;

                            if (listIonka[i].M3000F2 < 1000 && listIonka[i].M3000F2 > 0) average.M3000_27 += listIonka[i].M3000F2;
                            else average.M3000_27_skip++;
                        }

                        if (i < 30)
                        {
                            if (listIonka[i].f0F2 < 1000 && listIonka[i].f0F2 > 0) average.F2_30 += listIonka[i].f0F2;
                            else average.F2_30_skip++;

                            if (listIonka[i].M3000F2 < 1000 && listIonka[i].M3000F2 > 0) average.M3000_30 += listIonka[i].M3000F2;
                            else average.M3000_30_skip++;
                        }
                    }
                    else
                    {
                        if (i < 5)
                        {
                            average.F2_05_skip++;
                            average.M3000_05_skip++;
                        }

                        if (i < 7)
                        {
                            average.F2_07_skip++;
                            average.M3000_07_skip++;
                        }
                        
                        if (i < 10)
                        {
                            average.F2_10_skip++;
                            average.M3000_10_skip++;
                        }

                        if (i < 20)
                        {
                            average.F2_20_skip++;
                            average.M3000_20_skip++;
                        }

                        if (i < 27)
                        {
                            average.F2_27_skip++;
                            average.M3000_27_skip++;
                        }

                        if (i < 30)
                        {
                            average.F2_30_skip++;
                            average.M3000_30_skip++;
                        }
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

                //Если за 5 дней меньше 60% данных => нет данных
                //Для всех остальных порог равен 70%
                if (average.F2_05_skip > (5 * 40) / 100.0) average.F2_05 = 1000;
                if (average.F2_07_skip > (7 * 30) / 100.0) average.F2_07 = 1000;
                if (average.F2_10_skip > (10 * 30) / 100.0) average.F2_10 = 1000;
                if (average.F2_20_skip > (20 * 30) / 100.0) average.F2_20 = 1000;
                if (average.F2_27_skip > (27 * 30) / 100.0) average.F2_27 = 1000;
                if (average.F2_30_skip > (30 * 30) / 100.0) average.F2_30 = 1000;

                if (average.M3000_05_skip > (5 * 40) / 100.0) average.M3000_05 = 1000;
                if (average.M3000_07_skip > (7 * 30) / 100.0) average.M3000_07 = 1000;
                if (average.M3000_10_skip > (10 * 30) / 100.0) average.M3000_10 = 1000;
                if (average.M3000_20_skip > (20 * 30) / 100.0) average.M3000_20 = 1000;
                if (average.M3000_27_skip > (27 * 30) / 100.0) average.M3000_27 = 1000;
                if (average.M3000_30_skip > (30 * 30) / 100.0) average.M3000_30 = 1000;
                
                average.Update();
            }
            catch(Exception ex)
            {
                throw new Exception(stat.Name + stat.Code.ToString() + "Неизвестная ошибка при подсчете среднего", ex);
            }
        }

        public static int interpolation2( List<int> values )
        {
            //индекс [1] интерполируется
            //структура массива:
            //             0                        1                     2                                       3
            //[ (prevValue or prevDayValue), >>>currValue<<<, (nextValue0 or nextDayValue0), (nextValue1 or nextDayValue0 or nextDayValue1) ]
            if (values[0] != 1000 && values[1] == 1000)
            {
                double del = 2.0;
                int next = values[2];
                if (next == 1000)
                {
                    del++;
                    next = values[3];             
                }
                if (next == 1000) return 1000;
                return (int)Math.Round(values[0] + (next - values[0]) / del, 0);
            }
            return values[1];
        }

        public static void interpolation(DateTime end, Station stat)
        {
            DateTime prevDT = end.AddDays(-1);
            DateTime nextDT = end.AddDays(1);
            List<Average> averageGlobalPrev = Average.GetByDate(stat, prevDT.Year, prevDT.Month, prevDT.Day);
            List<Average> averageGlobalCurr = Average.GetByDate(stat, end.Year, end.Month, end.Day);
            List<Average> averageGlobalNext = Average.GetByDate(stat, nextDT.Year, nextDT.Month, nextDT.Day);

            if (averageGlobalNext == null || averageGlobalNext.Count < 2
                || averageGlobalPrev == null || averageGlobalPrev.Count < 2 ) return;

            for (int h = 0; h < averageGlobalCurr.Count; h++ )
            {   
                Average prev = null;
                List<Average> next = new List<Average>();
                if (h == 0) prev = averageGlobalPrev[23];
                else prev = averageGlobalCurr[h-1];

                if (h == 23)
                {
                    next.Add(averageGlobalNext[0]);
                    next.Add(averageGlobalNext[1]);
                }
                else
                {
                    next.Add(averageGlobalCurr[h + 1]);
                    if (h < 22)
                        next.Add(averageGlobalCurr[h + 2]);
                    else next.Add(averageGlobalNext[0]);
                }

                averageGlobalCurr[h].F2_05 = interpolation2(new List<int>() { prev.F2_05, averageGlobalCurr[h].F2_05, next[0].F2_05, next[1].F2_05 });
                averageGlobalCurr[h].F2_07 = interpolation2(new List<int>() { prev.F2_07, averageGlobalCurr[h].F2_07, next[0].F2_07, next[1].F2_07 });
                averageGlobalCurr[h].F2_10 = interpolation2(new List<int>() { prev.F2_10, averageGlobalCurr[h].F2_10, next[0].F2_10, next[1].F2_10 });
                averageGlobalCurr[h].F2_20 = interpolation2(new List<int>() { prev.F2_20, averageGlobalCurr[h].F2_20, next[0].F2_20, next[1].F2_20 });
                averageGlobalCurr[h].F2_27 = interpolation2(new List<int>() { prev.F2_27, averageGlobalCurr[h].F2_27, next[0].F2_27, next[1].F2_27 });
                averageGlobalCurr[h].F2_30 = interpolation2(new List<int>() { prev.F2_30, averageGlobalCurr[h].F2_30, next[0].F2_30, next[1].F2_30 });

                averageGlobalCurr[h].M3000_05 = interpolation2(new List<int>() { prev.M3000_05, averageGlobalCurr[h].M3000_05, next[0].M3000_05, next[1].M3000_05 });
                averageGlobalCurr[h].M3000_07 = interpolation2(new List<int>() { prev.M3000_07, averageGlobalCurr[h].M3000_07, next[0].M3000_07, next[1].M3000_07 });
                averageGlobalCurr[h].M3000_10 = interpolation2(new List<int>() { prev.M3000_10, averageGlobalCurr[h].M3000_10, next[0].M3000_10, next[1].M3000_10 });
                averageGlobalCurr[h].M3000_20 = interpolation2(new List<int>() { prev.M3000_20, averageGlobalCurr[h].M3000_20, next[0].M3000_20, next[1].M3000_20 });
                averageGlobalCurr[h].M3000_27 = interpolation2(new List<int>() { prev.M3000_27, averageGlobalCurr[h].M3000_27, next[0].M3000_27, next[1].M3000_27 });
                averageGlobalCurr[h].M3000_30 = interpolation2(new List<int>() { prev.M3000_30, averageGlobalCurr[h].M3000_30, next[0].M3000_30, next[1].M3000_30 });               

                averageGlobalCurr[h].Update();
            }
        }

        public static void ConsolidatedTableCalc()
        {
          

        }
        public static string StationAk(int indexStation,DateTime time)
        {
            string ak = "";
            CodeUmagf code = CodeUmagf.GetByDate(Station.GetByCode(indexStation), time.Year, time.Month, time.Day);
            if (code != null) ak = code.ak.ToString("D2");
            return ak;
        }
        public static string StationIndexK(int indexStation, DateTime time)
        {
            string kIndex = "";
            CodeUmagf code = CodeUmagf.GetByDate(Station.GetByCode(indexStation), time.Year, time.Month, time.Day);
            if (code != null) kIndex = code._k1 + code._k2 + code._k3 + code._k4 +" "+ code._k5 + code._k6 + code._k7 + code._k8;
            return kIndex;
        }

        public static void ConsolidatedTableCalc(DateTime currDate)
        {
            ConsolidatedTable tableCalc = ConsolidatedTable.GetByDateUTC(currDate.Year, currDate.Month, currDate.Day);
            if(tableCalc == null)
            {
                tableCalc = new ConsolidatedTable();
                tableCalc.YYYY = currDate.Year;
                tableCalc.MM = currDate.Month;
                tableCalc.DD = currDate.Day;
                tableCalc.Save();
            }
            if (tableCalc.Th13_Amag == null || tableCalc.Th13_Amag == "") tableCalc.Th13_Amag = HelperCalculation.StationAk(45601, currDate);
            if (tableCalc.Th14_Apar == null || tableCalc.Th14_Apar == "") tableCalc.Th14_Apar = HelperCalculation.StationAk(46501, currDate);
            if (tableCalc.Th15_Akha == null || tableCalc.Th15_Akha == "") tableCalc.Th15_Akha = HelperCalculation.StationAk(43501, currDate);
            if (tableCalc.Th16_K == null || tableCalc.Th16_K == "") tableCalc.Th16_K = HelperCalculation.StationIndexK(43501, currDate);
            tableCalc.Update();

        }
    }
}
