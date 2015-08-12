using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GeospaceEntity.Models;

namespace GeospaceMediana.Controllers
{
    public class MapController : Controller
    {
        //
        // GET: /Map/Stations

        public ActionResult Stations(int stationCode = 43501, string type = "f0F2", int year = -1, int month = -1, int day = -1)
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
            ViewBag.Date = nowDateTime.AddDays(-1);

            ViewBag.Station = Station.GetByCode(stationCode);

            List<Station> listStations = Station.GetAll();

            return View(listStations);
        }

    }
}
