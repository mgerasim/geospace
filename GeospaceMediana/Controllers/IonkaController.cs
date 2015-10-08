using GeospaceEntity.Common;
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
    public class IonkaController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index(int stationCode = 43501, string start = "", int limit = 5, int step = 5, string type = "f0F2", int year = -1, int month = -1, int day = -1)
        {
            @ViewBag.Title = "Геофизические данные";

            ViewBag.NameMenu = "Данные наблюдений";

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
                nowDateTime = DateTime.Now.AddDays(-3);
            }
            else nowDateTime = new DateTime(year, month, day);
            ViewBag.Date = nowDateTime;

            if (start == "")
            {
                start = String.Format("{0:yyyyMMdd}", nowDateTime);
            }
            ViewIonka Model = new ViewIonka(stationCode, start, limit, step);
            
            ViewBag.Station = Station.GetByCode(stationCode);

            return View(Model);
        }

        public ActionResult Submit(int stationcode, int year, int month, int day, string type, int hour, string newValue)
        {
            try
            {
                int iNewValue = CodeIonka.ConvertCodeToInt(newValue);

                Station station = Station.GetByCode(stationcode);

                CodeIonka codeIonka = CodeIonka.GetByDate(station, year, month, day, hour);

                if (codeIonka == null)
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

        public ActionResult SubmitUmagf(int stationcode, int year, int month, int day, string type, string newValue)
        {
            try
            {
                Station station = Station.GetByCode(stationcode);

                CodeUmagf codeUmagf = CodeUmagf.GetByDate(station, year, month, day);

                bool isUpdate = true;

                if(codeUmagf == null)
                {
                    codeUmagf = new CodeUmagf();

                    codeUmagf.Station = station;
                    codeUmagf.YYYY = year;
                    codeUmagf.MM = month;
                    codeUmagf.DD = day;
                    codeUmagf.Raw = "Исправлено вручную";

                    isUpdate = false;
                }

                switch (type)
                {
                    case "k1":
                        codeUmagf.k1 = Int32.Parse(newValue);
                        break;
                    case "k2":
                        codeUmagf.k2 = Int32.Parse(newValue);
                        break;
                    case "k3":
                        codeUmagf.k3 = Int32.Parse(newValue);
                        break;
                    case "k4":
                        codeUmagf.k4 = Int32.Parse(newValue);
                        break;
                    case "k5":
                        codeUmagf.k5 = Int32.Parse(newValue);
                        break;
                    case "k6":
                        codeUmagf.k6 = Int32.Parse(newValue);
                        break;
                    case "k7":
                        codeUmagf.k7 = Int32.Parse(newValue);
                        break;
                    case "k8":
                        codeUmagf.k8 = Int32.Parse(newValue);
                        break;
                    case "ak":
                        codeUmagf.ak = Int32.Parse(newValue);
                        break;
                    case "events":
                        codeUmagf.events = newValue;
                        break;
                }

                if (isUpdate)
                {
                    codeUmagf.Update();
                }
                else
                {
                    codeUmagf.Raw = "Исправлено вручную";
                    codeUmagf.Save();
                }

                return Content("");
            } 
            catch(Exception) {
               // return Content(e.ToString());
                return Content("Ошибка применения изменения! Проверьте корректность вводимых данных.");
            }
           
        }

    }
}