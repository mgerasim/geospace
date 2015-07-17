using GeospaceEntity.Models;
using GeospaceEntity.Models.Codes;
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

        public static Range GetRangeFromNumber(DateTime date, int number)
        {
            List<Range> ranges = new List<Range>();

            int curMin = 1;
            int curMax = 5;

            for (int i = 0; i < 5; i++)
            {
                curMin += 5;
                curMax += 5;

                ranges.Add(new Range { Min = curMin, Max = curMax });
            }

            ranges.Add(new Range { Min = 1, Max = 5 });

            int countDays = DateTime.DaysInMonth(date.Year, date.Month);

            if(countDays == 31)
            {
                ranges[4].Max = 31;
            }

            if (date.Month == 2)
            {
                ranges[4].Max = countDays;
            }

            return ranges[number];

        }

        public static void Calc(Station station, int year, int month, string type)
        {
            DateTime curMonth = new DateTime(year, month, DateTime.DaysInMonth(year, month));
            DateTime tmpPrevMonth = curMonth.AddMonths(-1);
            DateTime prevMonth = new DateTime(tmpPrevMonth.Year, tmpPrevMonth.Month, 1);

            var codesIonka = CodeIonka.GetByPeriod(station, prevMonth, curMonth);

            int countDays = DateTime.DaysInMonth(year, month);
            
            
            DateTime calcDate = new DateTime(year, month, 4);

            for (int i = 0; i < 6; i++)
            {
                int[] medians = new int[24];

                List<int> listValues;

                for (int hour = 0; hour < 24; hour++)
                {
                    medians[hour] = 0;

                    DateTime endRange = calcDate.AddDays(-1);

                    listValues = getValuesByRange(codesIonka, type, hour, endRange.AddDays(-9), endRange);

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

                            medians[hour] = (int)Math.Ceiling( (listValues[index1] + listValues[index2]) / 2.0 );
                        }
                        else
                        {
                            int index = listValues.Count / 2;

                            medians[hour] = listValues[index];
                        }
                    }
                }

                for(int hour=0;hour<24;hour++)
                {
                    Mediana mediana = Mediana.GetByDate(station, year, month, hour, i);
                    mediana.Station = station;
                    mediana.YYYY = year;
                    mediana.MM = month;
                    mediana.HH = hour;
                    mediana.RangeNumber = i;

                    switch (type)
                    {
                        case "f0F2":
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

                calcDate = calcDate.AddDays(5);

                if(countDays == 31)
                {
                    if (calcDate.Day == 29)
                    {
                        calcDate = calcDate.AddDays(1);
                    }
                }
                
                if(calcDate.Month == 2)
                {
                    if (i == 4)
                    {
                        if (countDays == 28)
                        {
                            calcDate = new DateTime(year, month, 27);
                        }
                        else
                        {
                            calcDate = new DateTime(year, month, 28);
                        }
                    }
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
                    listValues.Clear();
                    break;
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