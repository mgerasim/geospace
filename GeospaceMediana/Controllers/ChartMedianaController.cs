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
    public class ChartMedianaController : Controller
    {
        //
        // GET: /ChartMediana/

        public ActionResult Index(int year = -1, int month = -1, int stationCode = 43501, string type = "f0F2", int day = 1)
        {
            ViewBag.NameMenu = "Диаграмма медианы";

            if (type == "M3000F2")
                ViewBag.ViewType = "M3000";
            else
                ViewBag.ViewType = type;

            DateTime nowDateTime = GeospaceEntity.Helper.DateTimeKhabarovsk.Now;
            
            if (month == -1 || year == -1)
            {
                month = nowDateTime.Month;
                year = nowDateTime.Year;

                DateTime calcDate = MedianaCalculator.GetCalcDateBySeq(new DateTime(nowDateTime.Year, nowDateTime.Month, 1), 5);

                if (nowDateTime >= calcDate)
                {
                    DateTime _nextDate = nowDateTime.AddMonths(1);
                    
                    year = _nextDate.Year;
                    month = _nextDate.Month;
                }
            }


            DateTime curDate = new DateTime(year, month, 1);
            DateTime prevDate = curDate.AddMonths(-1);
            DateTime nextDate = curDate.AddMonths(1);

            Station station = Station.GetByCode(stationCode);

            IList<Mediana> listMediana = Mediana.GetByMonth(station, curDate.Year, curDate.Month)
                .Concat(Mediana.GetByMonth(station, prevDate.Year, prevDate.Month)).ToList();

            ViewMediana viewMediana = new ViewMediana(listMediana);

            ViewBag.CurDate = curDate;
            ViewBag.Date = curDate;
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
                mediana.IsFixed = true;

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
