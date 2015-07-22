using GeospaceEntity.Models;
using GeospaceMediana.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GeospaceMediana.Controllers
{
    public class ChartMedianaController : Controller
    {
        //
        // GET: /ChartMediana/

        public ActionResult Index(int year = -1, int month = -1, int stationCode = 43501)
        {
            DateTime nowDateTime = DateTime.Now;

            if (month == -1)
            {
                month = nowDateTime.Month;
            }

            if (year == -1)
            {
                year = nowDateTime.Year;
            }

            DateTime curDate = new DateTime(year, month, 1);
            DateTime prevDate = curDate.AddMonths(-1);
            DateTime nextDate = curDate.AddMonths(1);

            Station station = Station.GetByCode(stationCode);

            IList<Mediana> listMediana = Mediana.GetByMonth(station, curDate.Year, curDate.Month)
                .Concat(Mediana.GetByMonth(station, prevDate.Year, prevDate.Month)).ToList();

            ViewMediana viewMediana = new ViewMediana(listMediana);

            ViewBag.CurDate = curDate;
            ViewBag.PrevDate = prevDate;
            ViewBag.NextDate = nextDate;

            ViewBag.ViewMediana = viewMediana;
            ViewBag.Station = station;
            ViewBag.Stations = Station.GetAll();

            return View();
        }

        public ActionResult Submit(int stationCode, int startRange, string date, int hour, int newValue)
        {
            try
            {
                Station station = Station.GetByCode(stationCode);

                DateTime dateTime = DateTime.ParseExact(date, "yyyy MMMM", System.Globalization.CultureInfo.CurrentCulture);

                int numberRange = MedianaCalculator.GetNumberFromStartRange(startRange);

                Mediana mediana = Mediana.GetByDate(station, dateTime.Year, dateTime.Month, hour, numberRange);

                mediana.f0F2 = newValue;

                mediana.Update();

                return Content("");
            }
            catch
            {
                return Content("Ошибка применения изменения!");
            }
        }

    }
}
