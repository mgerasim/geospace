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

        public ActionResult Index(int stationCode = 43501, int year = -1, int month = -1, int startDay = -1)
        {
            if(year == -1)
            {
                year = DateTime.Now.Year;
                month = DateTime.Now.Month;
                startDay = 1;
            }

            Station station = Station.GetByCode(stationCode);

            Medians medians = new Medians(station, year, month, "f0F2");
            var rangeMedians = medians.Ranges.Where(x => x.Min == startDay).Single();
            
            int endDay = rangeMedians.Max;

            ViewBag.DayStart = startDay;
            ViewBag.DayEnd = endDay;
            ViewBag.CharacterizationDay = new CharacterizationDay(station, rangeMedians, year, month, startDay);

            ViewBag.Year = year;
            ViewBag.Month = month;
            ViewBag.Stations = Station.GetAll();
            ViewBag.Station = station;
            ViewBag.RangeMedians = rangeMedians;

            return View();
        }

    }
}
