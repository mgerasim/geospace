using GeospaceEntity.Models;
using GeospaceEntity.Models.Codes;
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

            ViewBag.Medians = new Medians(startMonth.Year, startMonth.Month, Model, type);

            return View(Model);
        }

        public ActionResult Submit(int stationcode, int year, int month, int day, string type, int hour, string newValue)
        {
            try
            {
                int iNewValue = CodeIonka.ConvertCodeToInt(newValue);

                Station station = new Station();
                station = station.GetByCode(stationcode);

                CodeIonka codeIonka = new CodeIonka();

                codeIonka = codeIonka.GetByDate(station, year, month, day, hour);

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
