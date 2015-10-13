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

        public ActionResult Ap(int StationCode = 43501, int YYYY = -1, int MM = -1, int DD = -1)
        {
            DateTime currDate;
            DD = DateTime.Now.Day;
            try
            {
                currDate = new DateTime(YYYY, MM, DD);
            }
            catch
            {
                currDate = DateTime.Now;
            }
            ViewBag.prevDate = currDate.AddMonths(-1);
            ViewBag.nextDate = currDate.AddMonths(1);
            Station theStation = Station.GetByCode(StationCode);
            List<Station> theStations = Station.GetAll();

            if (theStation == null)
            {
                theStation = Station.GetByCode(43501);
            }
            @ViewBag.Title = "Геомагнитная обстановка за " + currDate.ToString("MMMM yyyy");
            ViewBag.NameMenu = "Геомагнитная обстановка";
            ViewBag.Date = currDate;
            ViewBag.Station = theStation;
            ViewBag.Stations = theStations;

            DateTime startDate = new DateTime(currDate.Year, currDate.Month, 1);
            DateTime endDate = new DateTime(currDate.Year, currDate.Month, DateTime.DaysInMonth(currDate.Year, currDate.Month));
            List<CodeUmagf> theUmagfList = (List<CodeUmagf>)CodeUmagf.GetByPeriod(theStation, startDate, endDate);
           
            return View(theUmagfList);
        }

    }
}
