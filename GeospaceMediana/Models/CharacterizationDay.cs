using GeospaceEntity.Models;
using GeospaceEntity.Models.Codes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GeospaceMediana.Models
{
    public class CharacterizationDay
    {
        private class RatingDayManager
        {
            private class Range
            {
                public int minusVal;
                public int plusVal;
                public double start;
                public double end;
            }

            List<Range> _listRange = new List<Range>();

            public void AddRange(int minusVal, int plusVal, double start, double end)
            {
                _listRange.Add(new Range
                {
                    minusVal = minusVal,
                    plusVal = plusVal,
                    start = start,
                    end = end
                });
            }

            public int GetValue(double val)
            {
                double tmpVal = val < 0 ? -val : val;

                var range = _listRange.Where(x => x.start <= tmpVal && x.end >= tmpVal ).Single();

                if (val < 0)
                    return range.minusVal;

                return range.plusVal;
            }
        }

        private class HalfDay : RatingDayManager
        {
            public HalfDay()
            {
                AddRange(0, 0, 0, 2.5);
                AddRange(-1, 4, 3, 12);
                AddRange(-2, 5, 12.5, 24);
                AddRange(-3, 6, 24.5, 48);
            }
        }

        private class FullDay : RatingDayManager
        {
            public FullDay()
            {
                AddRange(0, 0, 0, 5.5);
                AddRange(-1, 4, 6, 24);
                AddRange(-2, 5, 24.5, 48);
                AddRange(-3, 6, 48.5, 96);
            }
        }


        public class CharacterizationDayValue
        {
            public int Day;
            public int Hour;

            public int f0F2 = int.MaxValue;
            public int PrevRating = int.MaxValue;
            public double Rating = double.MaxValue;

            public string viewf0F2;

            public string _f0F2
            {
                get
                {
                    if (f0F2 == int.MaxValue) return "";
                    return viewf0F2;
                }
            }

            public string _PrevRating
            {
                get
                {
                    if (PrevRating == int.MaxValue) return "";

                    string addStr = "";

                    if (PrevRating > 0)
                        addStr = "+";
                    else if (PrevRating == 0)
                        addStr = "-";

                    return addStr + PrevRating.ToString();
                }
            }

            public string _Rating
            {
                get
                {
                    if (Rating == double.MaxValue) return "";

                    string addStr = "";

                    if (Rating > 0)
                        addStr = "+";

                    return addStr + Rating.ToString();
                }
            }
        }

        public class DayRating
        {
            public int Day;
            public int Rating;
        }



        private List<CharacterizationDayValue> _listValues = new List<CharacterizationDayValue>();

        private List<DayRating> _listFirstHalfDay = new List<DayRating>();
        private List<DayRating> _listSecondHalfDay = new List<DayRating>();
        private List<DayRating> _listFullDay = new List<DayRating>();

        public CharacterizationDay(Station station, Medians.RangeDays rangeMedians, int year, int month, int startDay)
        {
            var startDate = new DateTime(year, month, startDay);

            var listCodeIonka = CodeIonka.GetByPeriod(station, startDate, startDate.AddDays(5));

            var halfDayCalc = new HalfDay();
            var fullDayCalc = new FullDay();

            for(int day = startDay; day <= rangeMedians.Max; day++)
            {
                double firstHalf = 0;
                double secondHalf = 0;

                for (int hour = 0; hour < 24; hour++)
                {
                    CodeIonka codeIonka;

                    if (rangeMedians.Values[hour] == 0)
                        continue;

                    try
                    {
                        codeIonka = listCodeIonka.Where(x => x.YYYY == year && x.MM == month && x.DD == day && x.HH == hour)
                            .OrderBy(x => x.MI)
                            .ToList()[0];
                    }
                    catch (Exception)
                    {
                        continue;
                    }

                   

                    var characterizationDayValue = new CharacterizationDayValue();

                    characterizationDayValue.f0F2 = codeIonka.f0F2;
                    characterizationDayValue.viewf0F2 = codeIonka._f0F2;
                    characterizationDayValue.Day = day;
                    characterizationDayValue.Hour = hour;

                    if (codeIonka.f0F2 == -1 || codeIonka.f0F2 >= 1000)
                    {
                        _listValues.Add(characterizationDayValue);
                        continue;
                    }

                    int median = rangeMedians.Values[hour];

                    characterizationDayValue.PrevRating = (int) Math.Round( ((characterizationDayValue.f0F2 - median) / ((double)median)) * 100 );
                    characterizationDayValue.Rating = calcRating(characterizationDayValue.PrevRating);

                    if(hour >= 0 && hour < 12)
                    {
                        firstHalf += characterizationDayValue.Rating;
                    }
                    else
                    {
                        secondHalf += characterizationDayValue.Rating;
                    }
                    
                    

                    _listValues.Add(characterizationDayValue);
                }

                _listFirstHalfDay.Add(new DayRating
                {
                    Day = day,
                    Rating =  halfDayCalc.GetValue( firstHalf )
                });

                _listSecondHalfDay.Add(new DayRating
                {
                    Day = day,
                    Rating = halfDayCalc.GetValue(secondHalf)
                });

                _listFullDay.Add(new DayRating
                {
                    Day = day,
                    Rating = fullDayCalc.GetValue(firstHalf + secondHalf)
                });
            }
        }

        public CharacterizationDayValue GetValue(int day, int hour)
        {
            var list = _listValues.Where(x => x.Day == day && x.Hour == hour).ToList();

            if(list.Count != 0)
            {
                return list[0];
            }

            return new CharacterizationDayValue();
        }

        public int GetFirstHalfDayValue(int day)
        {
            return _listFirstHalfDay.Where(x => x.Day == day).Single().Rating;
        }

        public int GetSecondHalfDayValue(int day)
        {
            return _listSecondHalfDay.Where(x => x.Day == day).Single().Rating;
        }

        public int GetFullDayValue(int day)
        {
            return _listFullDay.Where(x => x.Day == day).Single().Rating;
        }

        private double calcRating(int val)
        {
            double rating;

            if(val >= 0 && val <= 10) {
                rating = 0;
            }
            else if (val >= 11 && val <= 20) {
                rating = 0.5;
            }
            else if(val >= 61) {
                rating = 5;
            } else {
                val = val % 5 == 0 ? val - 1 : val;

                rating = 1 + (((val + 5 - (val % 5)) - 25) / 5.0) * 0.5;
            }

            if (val < 0)
                rating = -rating;

            return rating;
        }

    }
}