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
            ViewBag.IsLocal = Utils.Util.IsLocal();
            DateTime nowDateTime;
            if (date == "")
            {
                nowDateTime = GeospaceEntity.Helper.DateTimeKhabarovsk.Now;
                int current = (DateTime.DaysInMonth(nowDateTime.Year, nowDateTime.Month));
                if (nowDateTime.Day >= current-5)
                {  
                    nowDateTime = nowDateTime.AddMonths(1);
                }
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
                ViewBag.IsLocal = Utils.Util.IsLocal();
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
        public ActionResult SubmitTelegram(int year, int month, string title, string numberTel = "")
        {

            try
            {
                ViewBag.IsLocal = Utils.Util.IsLocal();
                string telegram = "ПРЕДВАРИТЕЛЬНЫЙ ПРОГНОЗ ИОНОСФЕРНОЙ И МАГНИТНОЙ";
                DateTime Date = new DateTime(year,month,1);
                telegram += "\nВОЗМУЩЕННОСТИ НА " + Date.ToString("MMMM yyyy") + " г.\n";
                List<GeospaceEntity.Models.Telegram.ForecastMonthIonosphera> forecast = GeospaceEntity.Models.Telegram.ForecastMonthIonosphera.GetAllByDateUTC(year, month);
                foreach (var item in forecast)
                {
                    telegram += "ПРОГНОЗ " + item.Station.Code + " 01" + DateTime.DaysInMonth(Date.Year, Date.Month) + Date.Month % 10 + " ";
                    bool keyFloat = false;
                    if (item.IONFO != "")
                    {
                        
                        telegram += "ИОНФО" + item.setStringFiveIteration(item.IONFO) + "\n";
                        keyFloat = true;
                    }
                    if (item.IONES != "")
                    {
                        if (keyFloat)
                            telegram += "                    ";//отступ
                        telegram += "ИОНЕС" + item.setStringFiveIteration(item.IONES) + "\n";
                        keyFloat = true;
                    }
                    if (item.IONDP != "")
                    {
                        if (keyFloat)
                            telegram += "                    ";//отступ
                        telegram += "ИОНДП" + item.setStringFiveIteration(item.IONDP) + "\n";
                        keyFloat = true;
                    }
                    if (item.IONFF != "")
                    {
                        if (keyFloat)
                            telegram += "                    ";//отступ
                        telegram += "ИОНФФ" + item.setStringFiveIteration(item.IONFF) + "\n";
                        keyFloat = true;
                    }
                    if (item.MAGPO != "")
                    {
                        if (keyFloat)
                            telegram += "                    ";//отступ
                        telegram += "МАГПО" + item.setStringFiveIteration(item.MAGPO) + "\n";
                        keyFloat = true;
                    }
                    if (item.IONFO != "" && item.IONES != "" && item.IONDP != "" && item.IONFF != "" && item.MAGPO != "")
                        telegram += "\n";
                }
                DateTime OldMonths = Date.AddMonths(-2);
                telegram += "ОПРАВДЫВАЕМОСТЬ ИОНОСФЕРНОГО ПРОГНОЗА ЗА " + OldMonths.ToString("MMMM yyyy") + " г.\n";
                List<GeospaceEntity.Models.Telegram.ForecastMonthIonosphera> forecastOld = GeospaceEntity.Models.Telegram.ForecastMonthIonosphera.GetAllByDateUTC(OldMonths.Year, OldMonths.Month);
                foreach (var item in forecastOld)
                {
                    telegram += "СТ." + item.Station.Code + " - " + item.iFORECAST + "% ";
                }
                telegram += "\n";
                telegram += "ОПРАВДЫВАЕМОСТЬ МАГНИТНОГО ПРОГНОЗА ЗА " + OldMonths.ToString("MMMM yyyy") + " г.\n";
                foreach (var item in forecastOld)
                {
                    telegram += "СТ." + item.Station.Code + " - " + item.mFORECAST + "% ";
                }
                telegram += "\n";
                List<GeospaceEntity.Models.Product> theList = (new GeospaceEntity.Models.Product()).GetAll();
                GeospaceEntity.Models.Product theProduct = null;

                if (theList.Count == 0)
                {
                    theProduct = new GeospaceEntity.Models.Product();
                    theProduct.forecast_month_ionosphera = telegram;
                    theProduct.Save();
                }
                else
                {
                    theProduct = theList[0];
                    theProduct.forecast_month_ionosphera = telegram;
                    theProduct.Update();
                }

                if (theProduct != null)
                {
                    theProduct.Send_MonthForecast(numberTel);
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
