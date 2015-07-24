using GeospaceEntity.Models;
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

        public ActionResult Index(int stationCode = 43501, int year = -1, int month = -1, int rangeNumber = -1, string type = "f0F2")
        {
            if (type == "M3000F2")
                ViewBag.ViewType = "M3000";
            else
                ViewBag.ViewType = type;

            ViewBag.NameMenu = "Характеристика суток " + ViewBag.ViewType;

            if(year == -1)
            {
                year = DateTime.Now.Year;
                month = DateTime.Now.Month;
                rangeNumber = 0;
            }

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

            ViewBag.Date = currStartDay.ToString("MMMM yyyy", System.Globalization.CultureInfo.CurrentCulture);

            return View();
        }

    }
}
