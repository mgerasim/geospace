using GeospaceEntity.Helper;
using GeospaceEntity.Models;
using GeospaceEntity.Models.Codes;
using GeospaceMediana.Common;
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

        public ActionResult Index(int year = -1, int month = -1, int stationCode = 43501, string type = "f0F2", int day =   1)
        {
            @ViewBag.Title = "Прогноз медианы";

            if (type == "M3000F2")
            {
                ViewBag.Type = "M3000F2";
            }
            if (type == "f0F2")
            {
                ViewBag.Type = "f0F2";
            }

            DateTime nowDateTime;
            if (year < 0 && month < 0 )
            {
                nowDateTime = DateTime.Now.AddDays(-1);
            }
            else nowDateTime = new DateTime(year, month, day);
            ViewBag.Date = nowDateTime;


            ViewBag.NameMenu = "Прогноз медианы " + type;


            if(month == -1)
            {
                month = nowDateTime.Month;
            }

            if(year == -1)
            {
                year = nowDateTime.Year;
            }

            DateTime startMonth = new DateTime(year, month, 1);
            DateTime nextMonth = startMonth.AddMonths(1);

            string start = String.Format("{0:yyyyMMdd}", startMonth);
            int countDays = DateTime.DaysInMonth(startMonth.Year, startMonth.Month);
            ViewIonka Model = new ViewIonka(stationCode, start, countDays, countDays);

            var objStation = Station.GetByCode(stationCode);

            ViewBag.DateString = startMonth.ToString("MMMM yyyy", System.Globalization.CultureInfo.CurrentCulture);
            ViewBag.CountDaysInMonth = countDays;
            
            ViewBag.Month = startMonth.Month;
            ViewBag.Year = startMonth.Year;

            ViewBag.Type = type;
            ViewBag.Station = objStation;

            ViewBag.ViewMediana = new ViewMediana(Mediana.GetByMonth(objStation, year, month)
                .Concat(Mediana.GetByMonth(objStation, nextMonth.Year, nextMonth.Month)).ToList());

            return View(Model);
        }

        public ActionResult Calc(int year, int month, int stationCode, string type)
        {
            try
            {
                var objStation = Station.GetByCode(stationCode);

                MedianaCalculator.Calc(objStation, year, month, type);

                return Content("");
            }
            catch(Exception ex)
            {
                return Content("Во время расчета медианы произошла ошибка.\r\n" + ex.Message + "\r\n" + ex.StackTrace);
            }
        }

        public ActionResult Submit(int stationcode, int year, int month, int day, string type, int hour, string newValue)
        {
            try
            {
                int iNewValue = CodeIonka.ConvertCodeToInt(newValue);

                Station station = Station.GetByCode(stationcode);

                CodeIonka codeIonka = CodeIonka.GetByDate(station, year, month, day, hour);

                if(codeIonka == null) // Если запись отсутствует
                {
                    codeIonka = new CodeIonka();

                    codeIonka.Station = station;
                    codeIonka.YYYY = year;
                    codeIonka.MM = month;
                    codeIonka.DD = day;
                    codeIonka.HH = hour;

                    codeIonka.SetValueByType(type, iNewValue);
                    codeIonka.Save();
                }
                else
                {
                    codeIonka.SetValueByType(type, iNewValue);
                    codeIonka.Update();
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
