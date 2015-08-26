using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GeospaceMediana.Controllers
{
    public class ForecastMonthIonospheraController : Controller
    {
        //
        // GET: /ForecastMonthIonosphera/

        public ActionResult Index(string date = "")
        {
            DateTime nowDateTime;
            if (date == "")
            {
                nowDateTime = GeospaceEntity.Helper.DateTimeKhabarovsk.Now;
            }
            else
            {
                nowDateTime = DateTime.ParseExact(date, "yyyyMM",
                                        System.Globalization.CultureInfo.InvariantCulture);
            }
            ViewBag.Date = nowDateTime;
            //получен информации Медианнапо заданным станциям
            int[] station = new int[] { 37701, 45601, 43501, 46501 };//страница не тяница если добавить больше станцый нужно переделать верстку!!!!
            string[] namePrognoz = { "IONFO", "IONES", "IONDP", "IONFF", "MAGPO" };
            ViewBag.NameForecast = namePrognoz;
            ViewBag.NumStation = station;
            IList<GeospaceEntity.Models.Telegram.ForecastMonthIonosphera> telegrams = GeospaceEntity.Models.Telegram.ForecastMonthIonosphera.GetAllByDateUTC(nowDateTime.Year, nowDateTime.Month);
            return View(telegrams);
        }
        public ActionResult Submit(int station, int year, int month, string type, string value)
        {
            
            return Content("");
        }
    }
}
