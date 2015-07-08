using GeospaceMediana.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GeospaceMediana.Controllers
{
    public class MedianaController : Controller
    {
        //
        // GET: /Mediana/

        public ActionResult Index(int year = -1, int month = -1, int station = 43501, string type = "f0F2")
        {
            @ViewBag.Title = "Медиана";

            DateTime nowDateTime = DateTime.Now;

            if(month == -1)
            {
                month = nowDateTime.Month;
            }

            if(year == -1)
            {
                year = nowDateTime.Year;
            }

            DateTime startMonth = new DateTime(year, month, 1);

            string start = String.Format("{0:yyyyMMdd}", startMonth);
            int countDays = DateTime.DaysInMonth(startMonth.Year, startMonth.Month);
            ViewIonka Model = new ViewIonka(station, start, countDays, countDays);

            ViewBag.Date = startMonth.ToString("MMMM yyyy", System.Globalization.CultureInfo.CurrentCulture);
            ViewBag.CountDaysInMonth = countDays;

            ViewBag.Month = startMonth.Month;
            ViewBag.Year = startMonth.Year;

            ViewBag.Type = type;

            return View(Model);
        }

    }
}
