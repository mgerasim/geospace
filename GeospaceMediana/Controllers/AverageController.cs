using GeospaceEntity.Models;
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
            @ViewBag.Title = "Среднее значения";

            if (type == "M3000F2")
                ViewBag.ViewType = "M3000";
            else
                ViewBag.ViewType = type;

            ViewBag.NameMenu = "Среднее за период " + ViewBag.ViewType;

            DateTime nowDateTime;

            if (year < 0 && month < 0 && day < 0)
            {
                nowDateTime = DateTimeKhabarovsk.Now;
                nowDateTime = nowDateTime.AddDays(-1);
            }
            else nowDateTime = new DateTime(year, month, day);
            DateTime prevDate = nowDateTime.AddDays(-1);
            DateTime nextDate = nowDateTime.AddDays(1);            

            ViewBag.PrevDate = prevDate;
            ViewBag.NextDate = nextDate;

            ViewBag.Date = nowDateTime;
            ViewBag.Type = type;
            ViewBag.Station = Station.GetByCode(stationCode);
            ViewBag.Stations = Station.GetAll();

            ViewAverage viewAverage = new ViewAverage(stationCode, nowDateTime.Year, nowDateTime.Month, nowDateTime.Day);
            ViewIonka viewIonka = new ViewIonka(stationCode, nowDateTime.Year, nowDateTime.Month, nowDateTime.Day);

            string value_05 = "[";
            string value_05_skip = "[";
            string value = "[";

            if (type == "f0F2")
            {
                for (int i = 0; i < viewAverage.theAverageValues.Count; i++)
                {
                    value_05 += (viewAverage.theAverageValues[i].F2_05.ToString()).Replace(",", ".") + ",";
                    value_05_skip += viewAverage.theAverageValues[i].F2_05_skip.ToString() + ",";
                    if (i < viewIonka.theIonkaValues.Count)
                        if (viewIonka.theIonkaValues[i].f0F2 < 1000)
                            value += viewIonka.theIonkaValues[i].f0F2.ToString() + ",";
                        else
                            value += "-1,";
                }
                
            }

            if (type == "M3000F2")
            {
                for (int i = 0; i < viewAverage.theAverageValues.Count; i++)
                {
                    value_05 += (viewAverage.theAverageValues[i].M3000_05.ToString()).Replace(",", ".") + ",";
                    value_05_skip += viewAverage.theAverageValues[i].M3000_05_skip.ToString() + ",";
                    if (i < viewIonka.theIonkaValues.Count)
                        if (viewIonka.theIonkaValues[i].M3000F2 < 1000)
                            value += viewIonka.theIonkaValues[i].M3000F2.ToString() + ",";
                        else
                            value += "-1,";
                }
            }

            value_05 += "]";
            value_05_skip += "]";
            value += "]";

            ViewBag.value_05 = value_05;
            ViewBag.value_05_skip = value_05_skip;
            ViewBag.value = value;

            return View();
        }

    }
}
