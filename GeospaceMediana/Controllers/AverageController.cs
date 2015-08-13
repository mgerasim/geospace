﻿using GeospaceEntity.Models;
using GeospaceEntity.Models.Codes;
using GeospaceMediana.Common;
using GeospaceMediana.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GeospaceMediana.Controllers
{
    public class AverageController : Controller
    {
        //
        // GET: /Average/

        public ActionResult Index(int stationCode = 43501, string type = "f0F2", int year=-1, int month=-1, int day=-1)
        {
            @ViewBag.Title = "Средние значения";

            if (type == "M3000F2" || type == "M3000")
            {
                ViewBag.Type = "M3000";
                ViewBag.textY = "Коэффициент M3000 ⋅ 10";
            }
            if (type == "f0F2" || type == "f0")
            {
                ViewBag.Type = "f0";
                ViewBag.textY = "Критическая частота МГц ⋅ 10";
            }

            ViewBag.NameMenu = "Средние значения " + ViewBag.ViewType;

            DateTime nowDateTime;

            if (year < 0 && month < 0 && day < 0)
            {
                nowDateTime = DateTime.Now.AddDays(-1);
            }
            else nowDateTime = new DateTime(year, month, day);
            DateTime nextDate = nowDateTime.AddDays(1);
            DateTime prevDate = nowDateTime.AddDays(-1);

            int rangeNumber = 1;

            if (1 <= nowDateTime.Day && nowDateTime.Day <= 5) rangeNumber = 0;
            if (6 <= nowDateTime.Day && nowDateTime.Day <= 10) rangeNumber = 1;
            if (11 <= nowDateTime.Day && nowDateTime.Day <= 15) rangeNumber = 2;
            if (16 <= nowDateTime.Day && nowDateTime.Day <= 20) rangeNumber = 3;
            if (21 <= nowDateTime.Day && nowDateTime.Day <= 25) rangeNumber = 4;
            if (26 <= nowDateTime.Day && nowDateTime.Day <= 31) rangeNumber = 5;

            ViewBag.PrevDate = prevDate;
            ViewBag.NextDate = nextDate;

            ViewBag.Date = nowDateTime;
            ViewBag.Station = Station.GetByCode(stationCode);
            ViewBag.Stations = Station.GetAll();

            ViewAverage viewAverage = new ViewAverage(stationCode, nowDateTime.Year, nowDateTime.Month, nowDateTime.Day);
            ViewIonka viewIonka = new ViewIonka(stationCode, nowDateTime.Year, nowDateTime.Month, nowDateTime.Day);
            

            string value = "[";
            string medianaValues = "[";
            string value_05 = "[";
            string value_05_skip = "[";
            string value_07 = "[";
            string value_07_skip = "[";
            string value_10 = "[";
            string value_10_skip = "[";
            string value_20 = "[";
            string value_20_skip = "[";
            string value_27 = "[";
            string value_27_skip = "[";
            string value_30 = "[";
            string value_30_skip = "[";

            int [,] marks_05 = new int[3, 24];
            int[,] marks_07 = new int[3, 24];
            int[,] marks_10 = new int[3, 24];
            int[,] marks_20 = new int[3, 24];
            int[,] marks_27 = new int[3, 24];
            int[,] marks_30 = new int[3, 24];

            if (type == "f0F2" || type == "f0")
            {
                for (int i = 0; i < viewAverage.theAverageValues.Count; i++)
                {
                    Mediana mediana = Mediana.GetByDate(Station.GetByCode(stationCode),
                        nowDateTime.Year, nowDateTime.Month, i, rangeNumber);

                    if (mediana != null && mediana.ID > 0)
                        medianaValues += mediana.f0F2.ToString() + ",";

                    value_05 += (viewAverage.theAverageValues[i].F2_05.ToString()).Replace(",", ".") + ",";
                    value_05_skip += viewAverage.theAverageValues[i].F2_05_skip.ToString() + ",";
                    marks_05[0, i] = viewAverage.theAverageValues[i].F2_05 - mediana.f0F2;
                    if (i < viewIonka.theIonkaValues.Count && viewIonka.theIonkaValues[i].f0F2 < 1000)
                    {
                        marks_05[1, i] = viewAverage.theAverageValues[i].F2_05 - viewIonka.theIonkaValues[i].f0F2;
                        marks_05[2, i] = mediana.f0F2 - viewIonka.theIonkaValues[i].f0F2;
                    }
                    else
                    {
                        marks_05[1, i] =1000;
                        marks_05[2, i] = 1000;
                    }                   

                    value_07 += (viewAverage.theAverageValues[i].F2_07.ToString()).Replace(",", ".") + ",";
                    value_07_skip += viewAverage.theAverageValues[i].F2_07_skip.ToString() + ",";
                    marks_07[0, i] = viewAverage.theAverageValues[i].F2_07 - mediana.f0F2;
                    if (i < viewIonka.theIonkaValues.Count && viewIonka.theIonkaValues[i].f0F2 < 1000)
                    {
                        marks_07[1, i] = viewAverage.theAverageValues[i].F2_07 - viewIonka.theIonkaValues[i].f0F2;
                        marks_07[2, i] = mediana.f0F2 - viewIonka.theIonkaValues[i].f0F2;
                    }
                    else
                    {
                        marks_07[1, i] = 1000;
                        marks_07[2, i] = 1000;
                    }

                    value_10 += (viewAverage.theAverageValues[i].F2_10.ToString()).Replace(",", ".") + ",";
                    value_10_skip += viewAverage.theAverageValues[i].F2_10_skip.ToString() + ",";
                    marks_10[0, i] = viewAverage.theAverageValues[i].F2_10 - mediana.f0F2;
                    if (i < viewIonka.theIonkaValues.Count && viewIonka.theIonkaValues[i].f0F2 < 1000)
                    {
                        marks_10[1, i] = viewAverage.theAverageValues[i].F2_10 - viewIonka.theIonkaValues[i].f0F2;
                        marks_10[2, i] = mediana.f0F2 - viewIonka.theIonkaValues[i].f0F2;
                    }
                    else
                    {
                        marks_10[1, i] = 1000;
                        marks_10[2, i] = 1000;
                    }

                    value_20 += (viewAverage.theAverageValues[i].F2_20.ToString()).Replace(",", ".") + ",";
                    value_20_skip += viewAverage.theAverageValues[i].F2_20_skip.ToString() + ",";
                    marks_20[0, i] = viewAverage.theAverageValues[i].F2_20 - mediana.f0F2;
                    if (i < viewIonka.theIonkaValues.Count && viewIonka.theIonkaValues[i].f0F2 < 1000)
                    {
                        marks_20[1, i] = viewAverage.theAverageValues[i].F2_20 - viewIonka.theIonkaValues[i].f0F2;
                        marks_20[2, i] = mediana.f0F2 - viewIonka.theIonkaValues[i].f0F2;
                    }
                    else
                    {
                        marks_20[1, i] = 1000;
                        marks_20[2, i] = 1000;
                    }

                    value_27 += (viewAverage.theAverageValues[i].F2_27.ToString()).Replace(",", ".") + ",";
                    value_27_skip += viewAverage.theAverageValues[i].F2_27_skip.ToString() + ",";
                    marks_27[0, i] = viewAverage.theAverageValues[i].F2_27 - mediana.f0F2;
                    if (i < viewIonka.theIonkaValues.Count && viewIonka.theIonkaValues[i].f0F2 < 1000)
                    {
                        marks_27[1, i] = viewAverage.theAverageValues[i].F2_27 - viewIonka.theIonkaValues[i].f0F2;
                        marks_27[2, i] = mediana.f0F2 - viewIonka.theIonkaValues[i].f0F2;
                    }
                    else
                    {
                        marks_27[1, i] = 1000;
                        marks_27[2, i] = 1000;
                    }

                    value_30 += (viewAverage.theAverageValues[i].F2_30.ToString()).Replace(",", ".") + ",";
                    value_30_skip += viewAverage.theAverageValues[i].F2_30_skip.ToString() + ",";
                    marks_30[0, i] = viewAverage.theAverageValues[i].F2_30 - mediana.f0F2;
                    if (i < viewIonka.theIonkaValues.Count && viewIonka.theIonkaValues[i].f0F2 < 1000)
                    {
                        marks_30[1, i] = viewAverage.theAverageValues[i].F2_30 - viewIonka.theIonkaValues[i].f0F2;
                        marks_30[2, i] = mediana.f0F2 - viewIonka.theIonkaValues[i].f0F2;
                    }
                    else
                    {
                        marks_30[1, i] = 1000;
                        marks_30[2, i] = 1000;
                    }
                    if (i < viewIonka.theIonkaValues.Count)
                        if (viewIonka.theIonkaValues[i].f0F2 < 1000)
                            value += viewIonka.theIonkaValues[i].f0F2.ToString() + ",";
                        else
                            value += "0,";
                }
                
            }

            if (type == "M3000F2" || type == "M3000")
            {
                for (int i = 0; i < viewAverage.theAverageValues.Count; i++)
                {
                    Mediana mediana = Mediana.GetByDate(Station.GetByCode(stationCode),
                        nowDateTime.Year, nowDateTime.Month, i, rangeNumber);

                    if (mediana != null)
                        medianaValues += mediana.M3000F2.ToString() + ",";

                    value_05 += (viewAverage.theAverageValues[i].M3000_05.ToString()).Replace(",", ".") + ",";
                    value_05_skip += viewAverage.theAverageValues[i].M3000_05_skip.ToString() + ",";
                    marks_05[0, i] = viewAverage.theAverageValues[i].M3000_05 - mediana.M3000F2;
                    if (i < viewIonka.theIonkaValues.Count && viewIonka.theIonkaValues[i].M3000F2 < 1000)
                    {
                        marks_05[1, i] = viewAverage.theAverageValues[i].M3000_05 - viewIonka.theIonkaValues[i].M3000F2;
                        marks_05[2, i] = mediana.M3000F2 - viewIonka.theIonkaValues[i].M3000F2;
                    }
                    else
                    {
                        marks_05[1, i] = 1000;
                        marks_05[2, i] = 1000;
                    }

                    value_07 += (viewAverage.theAverageValues[i].M3000_07.ToString()).Replace(",", ".") + ",";
                    value_07_skip += viewAverage.theAverageValues[i].M3000_07_skip.ToString() + ",";
                    marks_07[0, i] = viewAverage.theAverageValues[i].M3000_07 - mediana.M3000F2;
                    if (i < viewIonka.theIonkaValues.Count && viewIonka.theIonkaValues[i].M3000F2 < 1000)
                    {
                        marks_07[1, i] = viewAverage.theAverageValues[i].M3000_07 - viewIonka.theIonkaValues[i].M3000F2;
                        marks_07[2, i] = mediana.M3000F2 - viewIonka.theIonkaValues[i].M3000F2;
                    }
                    else
                    {
                        marks_07[1, i] = 1000;
                        marks_07[2, i] = 1000;
                    }

                    value_10 += (viewAverage.theAverageValues[i].M3000_10.ToString()).Replace(",", ".") + ",";
                    value_10_skip += viewAverage.theAverageValues[i].M3000_10_skip.ToString() + ",";
                    marks_10[0, i] = viewAverage.theAverageValues[i].M3000_10 - mediana.M3000F2;
                    if (i < viewIonka.theIonkaValues.Count && viewIonka.theIonkaValues[i].M3000F2 < 1000)
                    {
                        marks_10[1, i] = viewAverage.theAverageValues[i].M3000_10 - viewIonka.theIonkaValues[i].M3000F2;
                        marks_10[2, i] = mediana.M3000F2 - viewIonka.theIonkaValues[i].M3000F2;
                    }
                    else
                    {
                        marks_10[1, i] = 1000;
                        marks_10[2, i] = 1000;
                    }

                    value_20 += (viewAverage.theAverageValues[i].M3000_20.ToString()).Replace(",", ".") + ",";
                    value_20_skip += viewAverage.theAverageValues[i].M3000_20_skip.ToString() + ",";
                    marks_20[0, i] = viewAverage.theAverageValues[i].M3000_20 - mediana.M3000F2;
                    if (i < viewIonka.theIonkaValues.Count && viewIonka.theIonkaValues[i].M3000F2 < 1000)
                    {
                        marks_20[1, i] = viewAverage.theAverageValues[i].M3000_20 - viewIonka.theIonkaValues[i].M3000F2;
                        marks_20[2, i] = mediana.M3000F2 - viewIonka.theIonkaValues[i].M3000F2;
                    }
                    else
                    {
                        marks_20[1, i] = 1000;
                        marks_20[2, i] = 1000;
                    }

                    value_27 += (viewAverage.theAverageValues[i].M3000_27.ToString()).Replace(",", ".") + ",";
                    value_27_skip += viewAverage.theAverageValues[i].M3000_27_skip.ToString() + ",";
                    marks_27[0, i] = viewAverage.theAverageValues[i].M3000_27 - mediana.M3000F2;
                    if (i < viewIonka.theIonkaValues.Count && viewIonka.theIonkaValues[i].M3000F2 < 1000)
                    {
                        marks_27[1, i] = viewAverage.theAverageValues[i].M3000_27 - viewIonka.theIonkaValues[i].M3000F2;
                        marks_27[2, i] = mediana.M3000F2 - viewIonka.theIonkaValues[i].M3000F2;
                    }
                    else
                    {
                        marks_27[1, i] = 1000;
                        marks_27[2, i] = 1000;
                    }

                    value_30 += (viewAverage.theAverageValues[i].M3000_30.ToString()).Replace(",", ".") + ",";
                    value_30_skip += viewAverage.theAverageValues[i].M3000_30_skip.ToString() + ",";
                    marks_30[0, i] = viewAverage.theAverageValues[i].M3000_30 - mediana.M3000F2;
                    if (i < viewIonka.theIonkaValues.Count && viewIonka.theIonkaValues[i].M3000F2 < 1000)
                    {
                        marks_30[1, i] = viewAverage.theAverageValues[i].M3000_30 - viewIonka.theIonkaValues[i].M3000F2;
                        marks_30[2, i] = mediana.M3000F2 - viewIonka.theIonkaValues[i].M3000F2;
                    }
                    else
                    {
                        marks_30[1, i] = 1000;
                        marks_30[2, i] = 1000;
                    }
                    if (i < viewIonka.theIonkaValues.Count)
                        if (viewIonka.theIonkaValues[i].M3000F2 < 1000)
                            value += viewIonka.theIonkaValues[i].M3000F2.ToString() + ",";
                        else
                            value += "0,";
                }
            }

            value += "]";
            value_05 += "]";
            value_05_skip += "]";
            value_07 += "]";
            value_07_skip += "]";
            value_10 += "]";
            value_10_skip += "]";
            value_20 += "]";
            value_20_skip += "]";
            value_27 += "]";
            value_27_skip += "]";
            value_30 += "]";
            value_30_skip += "]";
            medianaValues += "]";

            ViewBag.value = value;
            ViewBag.value_05 = value_05;
            ViewBag.value_05_skip = value_05_skip;
            ViewBag.value_07 = value_07;
            ViewBag.value_07_skip = value_07_skip;
            ViewBag.value_10 = value_10;
            ViewBag.value_10_skip = value_10_skip;
            ViewBag.value_20 = value_20;
            ViewBag.value_20_skip = value_20_skip;
            ViewBag.value_27 = value_27;
            ViewBag.value_27_skip = value_27_skip;
            ViewBag.value_30 = value_30;
            ViewBag.value_30_skip = value_30_skip;
            ViewBag.mediana = medianaValues;

            ViewBag.marks_05 = marks_05;
            ViewBag.marks_07 = marks_07;
            ViewBag.marks_10 = marks_10;
            ViewBag.marks_20 = marks_20;
            ViewBag.marks_27 = marks_27;
            ViewBag.marks_30 = marks_30;
            
            return View();
        }
    }
}
