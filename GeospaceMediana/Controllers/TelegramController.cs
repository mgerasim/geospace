using GeospaceEntity.Models;
using GeospaceMediana.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GeospaceMediana.Controllers
{
    public class TelegramController : Controller
    {
        //
        // GET: /Telegram/

        public ActionResult Index(int stationCode = 43501, string type = "f0F2", int year = -1, int month = -1, int day = -1)
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
                nowDateTime = DateTime.Now;
            }
            else nowDateTime = new DateTime(year, month, day);
            ViewBag.Date = nowDateTime;

            ViewBag.Station = Station.GetByCode(stationCode);
            List<GeospaceEntity.Models.Product> theList = (new GeospaceEntity.Models.Product()).GetAll();
            GeospaceEntity.Models.Product theProduct = null;

            if (theList.Count == 0)
            {
                theProduct = new GeospaceEntity.Models.Product();
                theProduct.Save();
            }
            else
            {
                theProduct = theList[0];
            }

            return View(theProduct);
        }
       public ActionResult FiveDay( string date = "", int rangeNumber = -1)
       {
            DateTime nowDateTime;
            bool def = false;
            if (date == "")
            {
                nowDateTime = DateTime.Now;
            }
            else
            {
                nowDateTime = DateTime.ParseExact(date, "yyyyMM",
                                        System.Globalization.CultureInfo.InvariantCulture);
            }
            if (rangeNumber == -1)
            {
                rangeNumber = GeospaceMediana.Models.MedianaCalculator.GetRangeFromDate(nowDateTime);
                def = true;
            }
            ViewBag.number = rangeNumber;
            ViewBag.range = MedianaCalculator.GetRangeFromNumber(nowDateTime, rangeNumber);
           if (def == true && rangeNumber == 0)
                nowDateTime = nowDateTime.AddMonths(1);
            ViewBag.Date = nowDateTime;
            //получен информации Медианнапо заданным станциям
           int[] station = new int[]{45601,43501,46501};//страница не тяница если добавить больше станцый нужно переделать верстку!!!!
           string[] namePrognoz = { "IONFO", "IONES", "MAGPO" };
           ViewBag.NameForecast = namePrognoz;
           ViewBag.NumStation = station;
           return View();
        }
    }
}
