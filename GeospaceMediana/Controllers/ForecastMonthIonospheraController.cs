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
                nowDateTime = nowDateTime.AddMonths(1);
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
            string[] namePrognozRU = { "ИОНФО", "ИОНЕС", "ИОНДП", "ИНОФФ", "МАГПО" };
            ViewBag.NameForecast = namePrognoz;
            ViewBag.NameForecastRU = namePrognozRU;
            ViewBag.NumStation = station;
            
            //List<GeospaceEntity.Models.Telegram.ForecastMonthIonosphera> telegrams = GeospaceEntity.Models.Telegram.ForecastMonthIonosphera.GetAllByDateUTC(nowDateTime.Year, nowDateTime.Month);
            return View();
        }
        public ActionResult Submit(int station, int year, int month, string type, string value)
        {

            try
            {
                Station stationCode = Station.GetByCode(station);

                GeospaceEntity.Models.Telegram.ForecastMonthIonosphera forecastFiveDay = GeospaceEntity.Models.Telegram.ForecastMonthIonosphera.GetByDateUTC(stationCode, year, month);

                if (forecastFiveDay == null)
                {
                    forecastFiveDay = new GeospaceEntity.Models.Telegram.ForecastMonthIonosphera();

                    forecastFiveDay.Station = stationCode;
                    forecastFiveDay.YYYY = year;
                    forecastFiveDay.MM = month;
                    forecastFiveDay.SetValueByType(type, value);

                    forecastFiveDay.Save();
                }
                else
                {
                    forecastFiveDay.SetValueByType(type, value);
                    forecastFiveDay.Update();
                }

                return Content("");
            }
            catch (Exception)
            {
                return Content("Ошибка при отправлении данных! Проверьте корректность вводимых данных.");
            }
        }
    }
}
