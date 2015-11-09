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
            ViewBag.IsLocal = Utils.Util.IsLocal();
           return View();
        }

        public ActionResult FiveDay(int stationCode = 43501, string date = "", int rangeNumber = -1)
       {
           ViewBag.IsLocal = Utils.Util.IsLocal();
            DateTime nowDateTime;
            bool def = false;
            if (date == "")
            {
                nowDateTime = GeospaceEntity.Helper.DateTimeKhabarovsk.Now;
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
            DateTime rangeTime = MedianaCalculator.GetFromDate(nowDateTime);
            ViewBag.range = MedianaCalculator.GetRangeFromNumber(nowDateTime, rangeNumber);
           //if (def == true && rangeNumber == 0)
           //     nowDateTime = nowDateTime.AddMonths(1);
            ViewBag.Date = rangeTime;
            ViewBag.Year = rangeTime.Year;
            ViewBag.Month = rangeTime.Month;
            //получен информации Медианнапо заданным станциям
           int[] station = new int[]{45601,43501,46501};//страница не тяница если добавить больше станцый нужно переделать верстку!!!!
           string[] namePrognoz = { "IONFO", "IONES", "MAGPO" };
           ViewBag.NameForecast = namePrognoz;
           ViewBag.NumStation = station;
           return View();
        }

        public ActionResult SubmitFiveDay(int stationcode, int year, int month, int range_number, string type, string newValue)
        {
            ViewBag.IsLocal = Utils.Util.IsLocal();
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
            catch (Exception x )
            {
                return View( x);
            }
        }
        public ActionResult SubmitFiveDayTelegram(int year, int month, int range_number,  string numberTel = "" )
        {
            ViewBag.IsLocal = Utils.Util.IsLocal();
            try
            {
                ViewBag.number = range_number;
                String telegram = "ПЯТИДНЕВНЫЙ ПРОГНОЗ\r\n\r\n";
                DateTime day = new DateTime(year, month, 1);
                ViewBag.Date = day;
                var range = MedianaCalculator.GetRangeFromNumber(day, range_number);
                int[] station = new int[] { 45601, 43501, 46501 };
                foreach (var item in station)
                {
                    Station stat = Station.GetByCode(item);
                    telegram += ("MEDIAN  " + item + "  " + range.Min.ToString("D2") + "" + range.Max.ToString("D2") + "" + day.Month % 10);
                    telegram += "\r\n";
                    IList<Mediana> vMedian = Mediana.GetByRangeNumber(stat, year, month, range_number);
                    int i = 0;
                    string strFFF = ("FFF");
                    string strMM = ("MM");
                    foreach (var median in vMedian)
                    {
                        strFFF += median.f0F2.ToString("D3");
                        strMM += median.M3000F2.ToString("D2");
                    }
                    foreach (string str5 in new List<string>(System.Text.RegularExpressions.Regex.Split(strFFF, @"(?<=\G.{5})", System.Text.RegularExpressions.RegexOptions.Singleline)))
                    {
                        if (i == 10)
                            telegram += "\r\n";
                        telegram += str5 + " ";
                        i++;

                    }
                    telegram += "\r\n";
                    foreach (string str5 in new List<string>(System.Text.RegularExpressions.Regex.Split(strMM, @"(?<=\G.{5})", System.Text.RegularExpressions.RegexOptions.Singleline)))
                    {
                        telegram += str5 + " ";
                    }
                    telegram += "\r\n";
                    telegram += ("FORECAST  " + item + " " + range.Min.ToString("D2") + "" + range.Max.ToString("D2") + "" + day.Month % 10);
                    telegram += "\r\n";
                    GeospaceEntity.Models.Telegram.ForecastFiveDay forecastData = GeospaceEntity.Models.Telegram.ForecastFiveDay.GetByDateUTC(stat, day.Year, day.Month, range_number);
                    if(forecastData != null){
                        if (forecastData.IONFO != "") { telegram += "IONFO " + forecastData.setReScanValue(forecastData.IONFO) + "  "; }
                        if (forecastData.IONES != "") { telegram += "IONES " + forecastData.setReScanValue(forecastData.IONES) + "  "; }
                        if (forecastData.MAGPO != "") { telegram += "MAGPO " + forecastData.setReScanValue(forecastData.MAGPO) + "  "; }
                    }
                    telegram += "\r\n\r\n";
                }
                telegram += "ст. Магадан   – 45601\r\n";
                telegram += "ст. Хабаровск – 43501\r\n";
                telegram += "cт. Паратунка - 46501\r\n";
                
                List<GeospaceEntity.Models.Product> theList = (new GeospaceEntity.Models.Product()).GetAll();
                GeospaceEntity.Models.Product theProduct = null;

                if (theList.Count == 0)
                {
                    theProduct = new GeospaceEntity.Models.Product();
                    theProduct.forecast_days_fives = telegram;
                    theProduct.Save();
                }
                else
                {
                    theProduct = theList[0];
                    theProduct.forecast_days_fives = telegram;
                    theProduct.Update();
                }

                if (theProduct != null) {
                    theProduct.Send_FivedaysForecast(numberTel);
                }
                return Content("");
            }
            catch (Exception)
            {
                return Content("Ошибка сохранинии ! Проверьте корректность вводимых данных.");
            }

        }
    }
}
