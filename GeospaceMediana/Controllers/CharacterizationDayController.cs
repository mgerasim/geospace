using GeospaceEntity.Helper;
using GeospaceEntity.Models;
using GeospaceMediana.Common;
using GeospaceMediana.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GeospaceMediana.Controllers
{
    public class CharacterizationDayController : Controller
    {
        //
        // GET: /CharacterizationDay/

        public ActionResult Index(int stationCode = 43501, int year = -1, int month = -1, int rangeNumber = -1, string type = "f0F2", int day = -1)
        {
            if (type == "M3000F2")
            {
                ViewBag.Type = "M3000";
            }
            if (type == "f0F2")
            {
                ViewBag.Type = "f0";
            }

            DateTime nowDateTime;
            if (year < 0 && month < 0 && day < 0)
            {
                nowDateTime = DateTime.Now.AddDays(-1);
            }
            else
            {
                if(year < 0) year = DateTime.Now.Year;
                if(month < 0) month = DateTime.Now.Month;
                if(day < 0) day = (DateTime.Now.AddDays(-1)).Day;
                nowDateTime = new DateTime(year, month, day);
            }
            ViewBag.Date = nowDateTime;

            ViewBag.NameMenu = "Суточные отклонения " + ViewBag.Type;

            var curDay = DateTime.Now.Day;

            if (rangeNumber == -1)
            {
                for (int i = 0; i < 6; i++)
                {
                    var range = MedianaCalculator.GetRangeFromNumber(DateTime.Now, i);

                    if (curDay >= range.Min && curDay <= range.Max)
                    {
                        rangeNumber = i;
                        break;
                    }
                }
            }

            year = nowDateTime.Year;
            month = nowDateTime.Month;

            Station station = Station.GetByCode(stationCode);

            var viewMediana = new ViewMediana( Mediana.GetByMonth(station, year, month) );

            var currRange = MedianaCalculator.GetRangeFromNumber(new DateTime(year, month, 1), rangeNumber);

            DateTime currStartDay = new DateTime(year, month, currRange.Min);
            DateTime nextStartDate;
            DateTime prevStartDate;

            int nextRangeNumber = rangeNumber + 1;
            int prevRangeNumber = rangeNumber - 1;

            if(nextRangeNumber >= 6) {
                nextStartDate = currStartDay.AddMonths(1);
                nextRangeNumber = 0;
            } else {
                nextStartDate = currStartDay;
            }

            if (prevRangeNumber < 0) {
                prevStartDate = currStartDay.AddMonths(-1);
                prevRangeNumber = 5;
            } else {
                prevStartDate = currStartDay;
            }

            ViewBag.DayStart = currRange.Min;
            ViewBag.DayEnd = currRange.Max;
            ViewBag.CharacterizationDay = new CharacterizationDay(station, rangeNumber, year, month, type);

            ViewBag.Year = year;
            ViewBag.Month = month;
            ViewBag.Stations = Station.GetAll();
            ViewBag.Station = station;
            ViewBag.ViewMediana = viewMediana;
            ViewBag.RangeNumber = rangeNumber;

            ViewBag.PrevStartDate = prevStartDate;
            ViewBag.NextStartDate = nextStartDate;

            ViewBag.PrevRangeNumber = prevRangeNumber;
            ViewBag.NextRangeNumber = nextRangeNumber;

            ViewBag.Type = type;

            ViewBag.DateString = currStartDay.ToString("MMMM yyyy", System.Globalization.CultureInfo.CurrentCulture);

            return View();
        }

    }
}
