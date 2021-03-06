﻿using GeospaceEntity.Models;
using GeospaceEntity.Models.Codes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace GeospaceEntity.Helper
{
    public class MedianaCalculator
    {
        public class Range
        {
            public Range()
            {}

            public Range(Range range)
            {
                Min = range.Min;
                Max = range.Max;
            }

            public string Header
            {
                get
                {
                    return Min.ToString()+"-"+Max.ToString();
                }
            }

            public int Min;
            public int Max;
        }

        public static int GetNumberFromStartRange(int startRange)
        {
            return (startRange - 1) / 5;
        }

        private static Range[] ranges = null;

        public static Range GetRangeFromNumber(DateTime date, int number)
        {
            if(ranges == null)
            {
                ranges = new Range[6];

                int curMin = 1;
                int curMax = 5;

                for (int i = 0; i < 6; i++)
                {
                    ranges[i] = new Range { Min = curMin, Max = curMax };
                    curMin += 5;
                    curMax += 5;
                }
            }
            ranges[5].Max = DateTime.DaysInMonth(date.Year, date.Month);
            return new Range( ranges[number] );
        }

        public static int GetRangeFromDateReal(DateTime currDate)
        {
            int curDay = currDate.Day;
            int rangeNumber = 1;
            for (int i = 0; i < 6; i++)
            {
                var range = MedianaCalculator.GetRangeFromNumber(DateTime.Now, i);

                if (curDay >= range.Min && curDay <= range.Max)
                {
                    rangeNumber = i;
                    break;
                }
            }
            return rangeNumber;
        }
        public static int GetRangeFromDate(DateTime date)
        {
            if (ranges == null)
            {
                ranges = new Range[6];

                int curMin = 1;
                int curMax = 5;

                for (int i = 0; i < 6; i++)
                {
                    ranges[i] = new Range { Min = curMin, Max = curMax };
                    curMin += 5;
                    curMax += 5;
                }
            }

            int countDays = DateTime.DaysInMonth(date.Year, date.Month);
            ranges[5].Max = countDays;
            int number = 0;
            bool key = false;
            foreach(var item in ranges)
            {
                if(date.Day <= (item.Max)-2)
                {
                    key = true;
                    break;
                }
                number++;
            }
            if (key)
                return number;
            else
            {
                date = date.AddMonths(1);
                return 0;
            }
        }

        public static DateTime GetFromDate(DateTime date)
        {
            if (ranges == null)
            {
                ranges = new Range[6];

                int curMin = 1;
                int curMax = 5;

                for (int i = 0; i < 6; i++)
                {
                    ranges[i] = new Range { Min = curMin, Max = curMax };
                    curMin += 5;
                    curMax += 5;
                }
            }

            int countDays = DateTime.DaysInMonth(date.Year, date.Month);
            ranges[5].Max = countDays;
            int number = 0;
            bool key = false;
            foreach (var item in ranges)
            {
                if (date.Day <= (item.Max) - 2)
                {
                    key = true;
                    break;
                }
                number++;
            }
            if (key)
                return date;
            else
            {
                date = date.AddMonths(1);
                return date;
            }
        }

        private static int[] _calcDays = null; 

        public static DateTime GetCalcDateBySeq(DateTime month, int seqNumber)
        {
            if(_calcDays == null)
            {
                _calcDays = new int[6];

                for(int i=0;i<6;i++)
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
        // Вывод медианнцы для представления в пятидневной телеграмме FFF
        public static List<string> WriteHTML_FFF(int NumberStation,int Year, int Month, int range)
        {
            string pushStr = ("FFF");
            foreach(var item in Mediana.GetByRangeNumber(Station.GetByCode(NumberStation), Year, Month, range))
            {
                pushStr += item.f0F2.ToString("D3");
            }
            return new List<string>(Regex.Split(pushStr, @"(?<=\G.{5})", RegexOptions.Singleline));
        }
        public static void Calc(Station station, int year, int month, string type)
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
                        else
                        {
                            int rangeNumberPrev = i-1;
                            int monthPrev = month;
                            int yearPrev = year;

                            if (rangeNumberPrev == 0)
                            {
                                DateTime datePrev = new DateTime(year, month, 1);
                                datePrev = datePrev.AddDays(-1);
                                yearPrev = datePrev.Year;
                                monthPrev = datePrev.Month;
                                rangeNumberPrev = 5;
                            }

                            Mediana theMediana = null;
                           
                            try
                            {
                                //theMediana = Mediana.GetByMonth(station, year, month).Where(x => x.HH == hour).Single();
                                theMediana = Mediana.GetByRangeNumber(station, year, month,rangeNumberPrev).Where(x => x.HH == hour).Single();
                            }
                            catch
                            {
                                theMediana = new Mediana();
                                theMediana.M3000F2 = 0;
                                theMediana.f0F2 = 0;
                            }
                            
                            try
                            {

                                switch (type)
                                {
                                    case "f0F2":
                                        medians[hour] = theMediana.f0F2;
                                       // medians[hour] = Convert.ToInt32(Mediana.GetByMonth(station, year, month).Where(x => x.HH == hour).Average(x => x.f0F2));
                                        break;
                                    case "M3000F2":
                                        medians[hour] = theMediana.M3000F2;
                                       // medians[hour] = Convert.ToInt32(Mediana.GetByMonth(station, year, month).Where(x => x.HH == hour).Average(x => x.M3000F2));
                                        break;
                                }
                            }
                            catch
                            {
                                medians[hour] = 0;
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

                for(int hour=0;hour<24;hour++)
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

        private static List<int> getValuesByRange(IList<CodeIonka> codesIonka, string type, int hour, DateTime startDate, DateTime endDate)
        {
            List<int> listValues = new List<int>();

            for (var date = startDate; date <= endDate; date = date.AddDays(1))
            {
                CodeIonka codeIonka = null;

                try
                {
                    switch (type)
                    {
                        case "f0F2":
                            codeIonka = codesIonka.Where(x => x.YYYY == date.Year && x.MM == date.Month && x.DD == date.Day && x.HH == hour).Where(x => Math.Abs(x.delta_f0F2) < 33)
                                .OrderBy(x => x.MI)
                                .ToList()[0];
                            break;
                        case "M3000F2":
                            codeIonka = codesIonka.Where(x => x.YYYY == date.Year && x.MM == date.Month && x.DD == date.Day && x.HH == hour).Where(x => Math.Abs(x.delta_M3000) < 33)
                                    .OrderBy(x => x.MI)
                                    .ToList()[0];
                            break;
                        default:
                            continue;
                    }

                }
                catch(Exception)
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

    }
}