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
    public class IonkaController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index(int station = 43501, string start = "", int limit = 5, int step = 5 )
        {
            @ViewBag.Title = "Геофизические данные";
            if (start == "")
            {
                start = String.Format("{0:yyyyMMdd}", DateTime.Now.AddDays(-1));
            }
            ViewIonka Model = new ViewIonka(station, start, limit, step);

            return View(Model);
        }

        public ActionResult Submit(int stationcode, int year, int month, int day, string type, int hour, string newValue)
        {
            try
            {
                Station station = new Station();
                station = station.GetByCode(stationcode);

                CodeIonka codeIonka = new CodeIonka();

                codeIonka = codeIonka.GetByDate(station, year, month, day, hour);

                int iNewValue;

                switch (newValue.ToUpper())
                {
                    case "0":
                    case "00":
                        iNewValue = 1000;
                        break;
                    case "A":
                        iNewValue = 1001;
                        break;
                    case "B":
                        iNewValue = 1002;
                        break;
                    case "C":
                        iNewValue = 1003;
                        break;
                    case "D":
                        iNewValue = 1004;
                        break;
                    case "E":
                        iNewValue = 1005;
                        break;
                    case "F":
                        iNewValue = 1006;
                        break;
                    case "G":
                        iNewValue = 1007;
                        break;
                    case "N":
                        iNewValue = 1008;
                        break;
                    case "R":
                        iNewValue = 1009;
                        break;
                    default:
                        iNewValue = Int32.Parse(newValue);
                        break;
                }


                switch (type)
                {
                    case "f0F2":
                        codeIonka.f0F2 = iNewValue;
                        break;
                    case "M3000F2":
                        codeIonka.M3000F2 = iNewValue;
                        break;
                    case "f0F1":
                        codeIonka.f0F1 = iNewValue;
                        break;
                    case "M3000F1":
                        codeIonka.M3000F1 = iNewValue;
                        break;
                    case "f0Es":
                        if(iNewValue == 0)
                        {
                            codeIonka.f0Es = 1002;
                        }
                        else
                        {
                            codeIonka.f0Es = iNewValue;
                        }
                        break;
                    case "fmin":
                        codeIonka.fmin = iNewValue;
                        break;
                }

                codeIonka.Update();

                return Content("");
            }
            catch(Exception)
            {
               // return Content(e.ToString());
                return Content("Ошибка применения изменения! Проверьте корректность вводимых данных.");
            }
            
        }

    }
}