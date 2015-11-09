using GeospaceEntity.Models;
using GeospaceEntity.Models.Codes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GeospaceMediana.Controllers
{
    public class UmagfIndexController : Controller
    {
        //
        // GET: /Default1/

        public ActionResult Index(int stationCode = 43501, int YYYY = -1, int MM = -1, int DD = -1)
        {
            ViewBag.IsLocal = Utils.Util.IsLocal();
            DateTime currDate;
            try
            {
                currDate = new DateTime(YYYY, MM, DD);
            }
            catch
            {
                currDate = DateTime.Now;
            }
            ViewBag.PrevDate = currDate.AddDays(-1);
            ViewBag.NextDate = currDate.AddDays(1);
            Station theStation = Station.GetByCode(stationCode);
            List<Station> theStations = Station.GetAll();

            if (theStation == null)
            {
                theStation = Station.GetByCode(43501);
            }
            ViewBag.Title = "Геомагнитная обстановка за " + currDate.ToString(" dd MMMM yyyy");
            ViewBag.NameMenu = "Геомагнитная обстановка: K-индекс";
            ViewBag.Date = currDate;
            ViewBag.Station = theStation;
            ViewBag.Stations = theStations;
            List<CodeUmagf> theUmagfList = (List<CodeUmagf>)CodeUmagf.GetByPeriod(theStation, currDate.AddDays(-2), currDate);
            //CodeUmagf theUmagfList = CodeUmagf.GetByDate(theStation, currDate.Year, currDate.Month, currDate.Day);

            return View(theUmagfList);
        }

    }
}
