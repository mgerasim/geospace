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

            if (type == "M3000F2")
                ViewBag.Type = "M3000";
            if (type == "f0F2")
                ViewBag.Type = "f0";

            ViewBag.NameMenu = "Средние значения " + ViewBag.ViewType;

            DateTime nowDateTime;

            if (year < 0 && month < 0 && day < 0)
            {
                nowDateTime = DateTimeKhabarovsk.Now;
                nowDateTime = nowDateTime.AddDays(-1);
            }
            else nowDateTime = new DateTime(year, month, day);
            DateTime nextDate = nowDateTime.AddDays(1);
            DateTime prevDate = nowDateTime.AddDays(-1);
                        

            ViewBag.PrevDate = prevDate;
            ViewBag.NextDate = nextDate;

            ViewBag.Date = nowDateTime;
            ViewBag.Station = Station.GetByCode(stationCode);
            ViewBag.Stations = Station.GetAll();

            ViewAverage viewAverage = new ViewAverage(stationCode, nowDateTime.Year, nowDateTime.Month, nowDateTime.Day);
            ViewIonka viewIonka = new ViewIonka(stationCode, nowDateTime.Year, nowDateTime.Month, nowDateTime.Day);

            string value = "[";
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
            

            if (type == "f0F2")
            {
                for (int i = 0; i < viewAverage.theAverageValues.Count; i++)
                {
                    value_05 += (viewAverage.theAverageValues[i].F2_05.ToString()).Replace(",", ".") + ",";
                    value_05_skip += viewAverage.theAverageValues[i].F2_05_skip.ToString() + ",";

                    value_07 += (viewAverage.theAverageValues[i].F2_07.ToString()).Replace(",", ".") + ",";
                    value_07_skip += viewAverage.theAverageValues[i].F2_07_skip.ToString() + ",";

                    value_10 += (viewAverage.theAverageValues[i].F2_10.ToString()).Replace(",", ".") + ",";
                    value_10_skip += viewAverage.theAverageValues[i].F2_10_skip.ToString() + ",";

                    value_20 += (viewAverage.theAverageValues[i].F2_20.ToString()).Replace(",", ".") + ",";
                    value_20_skip += viewAverage.theAverageValues[i].F2_20_skip.ToString() + ",";

                    value_27 += (viewAverage.theAverageValues[i].F2_27.ToString()).Replace(",", ".") + ",";
                    value_27_skip += viewAverage.theAverageValues[i].F2_27_skip.ToString() + ",";

                    value_30 += (viewAverage.theAverageValues[i].F2_30.ToString()).Replace(",", ".") + ",";
                    value_30_skip += viewAverage.theAverageValues[i].F2_30_skip.ToString() + ",";
                    if (i < viewIonka.theIonkaValues.Count)
                        if (viewIonka.theIonkaValues[i].f0F2 < 1000)
                            value += viewIonka.theIonkaValues[i].f0F2.ToString() + ",";
                        else
                            value += "-0,";
                }
                
            }

            if (type == "M3000F2")
            {
                for (int i = 0; i < viewAverage.theAverageValues.Count; i++)
                {
                    value_05 += (viewAverage.theAverageValues[i].M3000_05.ToString()).Replace(",", ".") + ",";
                    value_05_skip += viewAverage.theAverageValues[i].M3000_05_skip.ToString() + ",";

                    value_07 += (viewAverage.theAverageValues[i].M3000_07.ToString()).Replace(",", ".") + ",";
                    value_07_skip += viewAverage.theAverageValues[i].M3000_07_skip.ToString() + ",";

                    value_10 += (viewAverage.theAverageValues[i].M3000_10.ToString()).Replace(",", ".") + ",";
                    value_10_skip += viewAverage.theAverageValues[i].M3000_10_skip.ToString() + ",";

                    value_20 += (viewAverage.theAverageValues[i].M3000_20.ToString()).Replace(",", ".") + ",";
                    value_20_skip += viewAverage.theAverageValues[i].M3000_20_skip.ToString() + ",";

                    value_27 += (viewAverage.theAverageValues[i].M3000_27.ToString()).Replace(",", ".") + ",";
                    value_27_skip += viewAverage.theAverageValues[i].M3000_27_skip.ToString() + ",";

                    value_30 += (viewAverage.theAverageValues[i].M3000_30.ToString()).Replace(",", ".") + ",";
                    value_30_skip += viewAverage.theAverageValues[i].M3000_30_skip.ToString() + ",";
                    if (i < viewIonka.theIonkaValues.Count)
                        if (viewIonka.theIonkaValues[i].M3000F2 < 1000)
                            value += viewIonka.theIonkaValues[i].M3000F2.ToString() + ",";
                        else
                            value += "-1,";
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
            

            return View();
        }
    }
}
