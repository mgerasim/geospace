using GeospaceEntity.Helper;
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
        public ActionResult FiveDay(int stationCode = 43501, string date = "", int rangeNumber = -1)
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
                rangeNumber = MedianaCalculator.GetRangeFromDate(nowDateTime);
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

        public ActionResult SubmitFiveDay(int stationcode, int year, int month, int range_number, string type, string newValue)
        {
            try
            {
                Station station = Station.GetByCode(stationcode);

                GeospaceEntity.Models.Telegram.ForecastFiveDay forecastFiveDay = GeospaceEntity.Models.Telegram.ForecastFiveDay.GetByDateUTC(station, year, month, range_number);

                if (forecastFiveDay == null)
                {
                    forecastFiveDay = new GeospaceEntity.Models.Telegram.ForecastFiveDay();

                    forecastFiveDay.Station = station;
                    forecastFiveDay.YYYY = year;
                    forecastFiveDay.MM = month;
                    forecastFiveDay.RangeNumber = range_number;
                    forecastFiveDay.SetValueByType(type, newValue);

                    forecastFiveDay.Save();
                }
                else
                {
                    forecastFiveDay.SetValueByType(type, newValue);
                    forecastFiveDay.Update();
                }

                return Content("");
            }
            catch (Exception)
            {
                // return Content(e.ToString());
                return Content("Ошибка применения изменения! Проверьте корректность вводимых данных.");
            }

        }
    }
}
