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

        public Medians(int year, int month, ViewIonka viewIonka, string type)
        {
            Type = type;

            int countDays = DateTime.DaysInMonth(year, month);

            int curMin = 1;

            while(curMin <= countDays)
            {
                int curMax = curMin + 4;

                if(curMax > countDays)
                    curMax = countDays;

                int[] medians = new int[24];

                List<int> listValues = new List<int>();

                for (int hour = 0; hour < 23; hour++)
                {
                    medians[hour] = 0;
                    listValues.Clear();

                    for (int day = curMin; day <= curMax; day++)
                    {
                        var codeIonka = viewIonka.GetValue(year, month, day, hour);

                        if (codeIonka.ID < 0)
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

                        if(value < 1000)
                        {
                            listValues.Add(value);
                        }
                    }

                    listValues.Sort();

                    if(listValues.Count != 0)
                    {
                        if (listValues.Count == 1)
                        {
                            medians[hour] = listValues[0];
                        }
                        else if (listValues.Count % 2 == 0)
                        {
                            int index1 = listValues.Count / 2 - 1;
                            int index2 = listValues.Count / 2;

                            medians[hour] = (listValues[index1] + listValues[index2]) / 2;
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

                curMin = curMax + 1;
            }
        }

    }
}