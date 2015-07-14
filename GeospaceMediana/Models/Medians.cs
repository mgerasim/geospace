using GeospaceEntity.Models;
using GeospaceEntity.Models.Codes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GeospaceMediana.Models
{
    public class Medians
    {
        public class RangeDays
        {
            public string Header
            {
                get
                {
                    return Min == Max ? Max.ToString() : Min.ToString() + "-" + Max.ToString();
                }
            }

            public int Min;
            public int Max;

            public int[] Values;
        }

        string Type;

        public IList<RangeDays> Ranges = new List<RangeDays>();

        public Medians(Station station, int year, int month, string type)
        {
            DateTime curMonth = new DateTime(year, month, DateTime.DaysInMonth(year, month));
            DateTime tmpPrevMonth = curMonth.AddMonths(-1);
            DateTime prevMonth = new DateTime(tmpPrevMonth.Year, tmpPrevMonth.Month, 1);

            var codesIonka = CodeIonka.GetByPeriod(station, prevMonth, curMonth); 

            Type = type;

            int countDays = DateTime.DaysInMonth(year, month);
            
            int curMin = 6;
            int curMax = 10;
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

                Ranges.Add(new RangeDays
                {
                    Min = curMin,
                    Max = curMax,
                    Values = medians,
                });

                curMin += 5;
                curMax += 5;
                calcDate = calcDate.AddDays(5);

                if (calcDate.Day == 29)
                {
                    curMin = 1;
                    curMax = 5;
                }

                if(countDays == 31)
                {
                    if(calcDate.Day == 24)
                    {
                        curMax = 31;
                    }
                    else if (calcDate.Day == 29)
                    {
                        calcDate = calcDate.AddDays(1);
                    }
                }
                
                if(calcDate.Month == 2)
                {
                    if (calcDate.Day == 29)
                    {
                        if (countDays == 28)
                        {
                            calcDate = new DateTime(calcDate.Year, calcDate.Month, 27);
                        }
                        else
                        {
                            calcDate = new DateTime(calcDate.Year, calcDate.Month, 28);
                        }
                    }
                }
            }
        }

        private List<int> getValuesByRange(IList<CodeIonka> codesIonka, string type, int hour, DateTime startDate, DateTime endDate)
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