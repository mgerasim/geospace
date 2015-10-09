using GeospaceEntity.Models;
using GeospaceEntity.Models.Codes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GeospaceMediana.Controllers
{
    public class UmagfController : Controller
    {
        //
        // GET: /Umagf/

        public ActionResult Index(int StationCode = 43501, int YYYY = -1, int MM = -1, int DD = -1)
        {
            DateTime currDate;
            try
            {
                currDate = new DateTime(YYYY, MM, DD);
            }
            catch
            {
                currDate = DateTime.Now;
            }
            Station theStation = Station.GetByCode(StationCode);
            List<Station> theStations = Station.GetAll();

            if (theStation == null)
            {
                theStation = Station.GetByCode(43501);
            }
            @ViewBag.Title = "Геомагнитные данные";
            ViewBag.NameMenu = "Геомагнитные данные";
            ViewBag.Date = currDate;
            ViewBag.Station = theStation;
            ViewBag.Stations = theStations;

            CodeUmagf theUmagf = CodeUmagf.GetByDate(Station.GetByCode(StationCode), currDate.Year, currDate.Month, currDate.Day);

            if (theUmagf == null)
            {
                ViewBag.Error = "Нет геомагнитных данных по станции " + theStation.Name + " за " + currDate.ToString("dd MMMM yyyy");
                return View();
            }
            
            ViewBag.Series = "[";
            string ss = String.Format("[Date.UTC({0}, {1}, {2}, {3}, {4}), {5}]",
                currDate.Year,
                currDate.Month,
                currDate.Day,
                0,
                0,
                theUmagf.k1);
            ViewBag.Series += ss;
            ViewBag.Series += ",";


            ss = String.Format("[Date.UTC({0}, {1}, {2}, {3}, {4}), {5}]",
                currDate.Year,
                currDate.Month,
                currDate.Day,
                3,
                0,
                theUmagf.k2);
            ViewBag.Series += ss;
            ViewBag.Series += ",";

            ss = String.Format("[Date.UTC({0}, {1}, {2}, {3}, {4}), {5}]",
                currDate.Year,
                currDate.Month,
                currDate.Day,
                6,
                0,
                theUmagf.k3);
            ViewBag.Series += ss;
            ViewBag.Series += ",";

            ss = String.Format("[Date.UTC({0}, {1}, {2}, {3}, {4}), {5}]",
                currDate.Year,
                currDate.Month,
                currDate.Day,
                9,
                0,
                theUmagf.k4);
            ViewBag.Series += ss;
            ViewBag.Series += ",";

            ss = String.Format("[Date.UTC({0}, {1}, {2}, {3}, {4}), {5}]",
                currDate.Year,
                currDate.Month,
                currDate.Day,
                12,
                0,
                theUmagf.k5);
            ViewBag.Series += ss;
            ViewBag.Series += ",";

            ss = String.Format("[Date.UTC({0}, {1}, {2}, {3}, {4}), {5}]",
                currDate.Year,
                currDate.Month,
                currDate.Day,
                15,
                0,
                theUmagf.k6);
            ViewBag.Series += ss;
            ViewBag.Series += ",";
            
            ss = String.Format("[Date.UTC({0}, {1}, {2}, {3}, {4}), {5}]",
                currDate.Year,
                currDate.Month,
                currDate.Day,
                18,
                0,
                theUmagf.k7);
            ViewBag.Series += ss;
            ViewBag.Series += ",";


            ss = String.Format("[Date.UTC({0}, {1}, {2}, {3}, {4}), {5}]",
                currDate.Year,
                currDate.Month,
                currDate.Day,
                21,
                0,
                theUmagf.k8);
            ViewBag.Series += ss;
            ViewBag.Series += ",";
            ViewBag.Series += ss;
            ViewBag.Series += ",";

            ViewBag.Series += "]";
            return View();
        }

    }
}
