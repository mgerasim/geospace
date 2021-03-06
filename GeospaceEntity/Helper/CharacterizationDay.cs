﻿using GeospaceEntity.Models;
using GeospaceEntity.Models.Codes;
using GeospaceEntity.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GeospaceEntity.Helper
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

            public int f0 = int.MaxValue;
            public int PrevRating = int.MaxValue;
            public double Rating = double.MaxValue;

            public string viewf0F2;
            public string viewf0F1;

            public string viewf0;

            public string _f0
            {
                get
                {
                    if (f0 == int.MaxValue) return "";
                    return viewf0;
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

        public List<CharacterizationDayValue> GetValues()
        {
            return _listValues;
        }

        private List<DayRating> _listFirstHalfDay = new List<DayRating>();
        private List<DayRating> _listSecondHalfDay = new List<DayRating>();
        private List<DayRating> _listFullDay = new List<DayRating>();

        public CharacterizationDay(Station station, int rangeNumber, int year, int month, string type)
        {
            var curDate = new DateTime(year, month, 1);
            var range = MedianaCalculator.GetRangeFromNumber(curDate, rangeNumber);

            var startDate = new DateTime(year, month, range.Min);

            var listCodeIonka = CodeIonka.GetByPeriod(station, startDate, startDate.AddDays(range.Max - range.Min));

            var medians = Mediana.GetByRangeNumber(station, year, month, rangeNumber);

            var halfDayCalc = new HalfDay();
            var fullDayCalc = new FullDay();

            for (int day = range.Min; day <= range.Max; day++)
            {
                double ballDay_minus = 0;
                double ballDay_plus = 0;
                double ballDay = 0;

                double firstHalf = 0;
                double secondHalf = 0;

                double firstHalf_plus = 0;
                double firstHalf_minus = 0;
                double secondHalf_plus = 0;
                double secondHalf_minus = 0;

                for (int hour = 0; hour < 24; hour++)
                {
                    CodeIonka codeIonka;
                    int medianaValue;


                    try
                    {
                        Mediana mediana = medians.Where(x => x.HH == hour).Single();

                        if(type == "f0F2")
                        {
                            medianaValue = mediana.f0F2;
                        }
                        else
                        {
                            medianaValue = mediana.M3000F2;
                        }
                        

                        codeIonka = listCodeIonka.Where(x => x.YYYY == year && x.MM == month && x.DD == day && x.HH == hour)
                            .OrderBy(x => x.MI)
                            .ToList()[0];
                    }
                    catch (Exception)
                    {
                        continue;
                    }

                    if (medianaValue == 0)
                        continue;
                   

                    var characterizationDayValue = new CharacterizationDayValue();

                    characterizationDayValue.Day = day;
                    characterizationDayValue.Hour = hour;

                    int valueF1;
                    string viewF1;
                    int valueF2;
                    string viewF2;

                    if(type == "f0F2")
                    {
                        viewF2 = codeIonka._f0F2;
                        viewF1 = codeIonka._f0F1;

                        valueF2 = codeIonka.f0F2;
                        valueF1 = codeIonka.f0F1;
                    }
                    else
                    {
                        viewF2 = codeIonka._M3000F2;
                        viewF1 = codeIonka._M3000F1;

                        valueF2 = codeIonka.M3000F2;
                        valueF1 = codeIonka.M3000F1;

                    }

                    if ((viewF2 == "C" || viewF2 == "G") && type == "f0F2")
                    {
                            characterizationDayValue.f0 = valueF1;
                            characterizationDayValue.viewf0 = "/" + viewF1;                        
                    }
                    else
                    {
                        characterizationDayValue.f0 = valueF2;
                        characterizationDayValue.viewf0 = viewF2;
                    }

                    if (characterizationDayValue.f0 == -1 || characterizationDayValue.f0 >= 1000)
                    {
                        characterizationDayValue.f0 = valueF2;
                        characterizationDayValue.viewf0 = viewF2;

                        _listValues.Add(characterizationDayValue);
                        continue;
                    }

                    int median = medianaValue;

                    characterizationDayValue.PrevRating = (int) Math.Round( ((characterizationDayValue.f0 - median) / ((double)median)) * 100 );
                    characterizationDayValue.Rating = calcRating(characterizationDayValue.PrevRating);

                    if(hour >= 0 && hour < 12)
                    {
                        if (characterizationDayValue.Rating < 0)
                        {
                            firstHalf_minus += characterizationDayValue.Rating;
                        }
                        else
                        {
                            firstHalf_plus += characterizationDayValue.Rating;
                        }
                        //firstHalf += characterizationDayValue.Rating;
                    }
                    else
                    {
                        if (characterizationDayValue.Rating < 0)
                        {
                            secondHalf_minus += characterizationDayValue.Rating;
                        }
                        else
                        {
                            secondHalf_plus += characterizationDayValue.Rating;
                        }
                        //secondHalf += characterizationDayValue.Rating;
                    }
                    
                    _listValues.Add(characterizationDayValue);
                }

                ballDay_minus = firstHalf_minus + secondHalf_minus;
                ballDay_plus = firstHalf_plus + secondHalf_plus;

                if (Math.Abs(firstHalf_minus) > Math.Abs(firstHalf_plus))
                {
                    firstHalf = firstHalf_minus;
                }
                else
                {
                    firstHalf = firstHalf_plus;
                }


                if (Math.Abs(secondHalf_minus) > Math.Abs(secondHalf_plus))
                {
                    secondHalf = secondHalf_minus;
                }
                else
                {
                    secondHalf = secondHalf_plus;
                }

                if (Math.Abs(ballDay_minus) > Math.Abs(ballDay_plus))
                {
                    ballDay = ballDay_minus;
                } 
                else
                {
                    ballDay = ballDay_plus;
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
                    Rating = fullDayCalc.GetValue(ballDay)
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
            int positiveVal = Math.Abs(val);

            double rating;

            if (positiveVal >= 0 && positiveVal <= 10)
            {
                rating = 0;
            }
            else if (positiveVal >= 11 && positiveVal <= 20)
            {
                rating = 0.5;
            }
            else if (positiveVal >= 61)
            {
                rating = 5;
            } else {
                positiveVal = positiveVal % 5 == 0 ? positiveVal - 1 : positiveVal;

                rating = 1 + (((positiveVal + 5 - (positiveVal % 5)) - 25) / 5.0) * 0.5;
            }

            if (val < 0)
                rating = -rating;

            return rating;
        }

        public int GetDeltaDayFirst(int Day)
        {
            int res = 0;
            foreach (var delta in this._listValues.Where(x => x.Day == Day).Where(x => x.Hour > 0 && x.Hour < 12).Select(x => x.PrevRating))
            {
                if (Math.Abs(delta) > Math.Abs(res) && delta != int.MaxValue) 
                {
                    res = delta;
                }
            }
            return res;
        }

        public int GetDeltaDaySecond(int Day)
        {
            int res = 0;
            foreach (var delta in this._listValues.Where(x => x.Day == Day).Where(x => x.Hour > 11 && x.Hour < 24).Select(x => x.PrevRating))
            {
                if (Math.Abs(delta) > Math.Abs(res) && delta != int.MaxValue) 
                {
                    res = delta;
                }
            }
            return res;
        }

        public int GetDeltaDayFull(int Day)
        {
            int res = 0;
            foreach (var delta in this._listValues.Where(x => x.Day == Day).Select(x => x.PrevRating))
            {
                if (Math.Abs(delta) > Math.Abs(res) && delta != int.MaxValue) 
                {
                    res = delta;
                }
            }
            return res;
        }

    }
}