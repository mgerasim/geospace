using GeospaceEntity.Models;
using GeospaceEntity.Models.Codes;
using GeospaceMediana.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GeospaceMediana.Models
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
                CodeIonka codeIonka;

                try
                {
                    codeIonka = codesIonka.Where(x => x.YYYY == date.Year && x.MM == date.Month && x.DD == date.Day && x.HH == hour)
                        .OrderBy(x => x.MI)
                        .ToList()[0];
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