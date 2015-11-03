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
    public class MagmaController : Controller
    {
        //
        // GET: /Magma/

        public ActionResult Index(int station = 43501, string type = "f0F2", int YYYY=-1, int MM=-1, int DD=-1)
        {
            ViewBag.IsLocal = Utils.Util.IsLocal();
            if (type == "M3000F2")
            {
                ViewBag.Type = "M3000";
            }
            if (type == "f0F2")
            {
                ViewBag.Type = "f0";
            }

            DateTime nowDateTime;
            if (YYYY < 0 && MM < 0 && DD < 0)
            {
                nowDateTime = DateTime.Now;
            }
            else nowDateTime = new DateTime(YYYY, MM, DD);
            ViewBag.Date = nowDateTime;

            ViewBag.Station = Station.GetByCode(station);

            if (station < -1)
            {
                station = 43501;
            }
            if (YYYY < 0)
            {
                YYYY = DateTime.Now.AddDays(-1).Year;
            }
            if (MM < 0)
            {
                MM = DateTime.Now.AddDays(-1).Month;
            }
            if( DD < 0)
            {
                DD = DateTime.Now.AddDays(-1).Day;
            }

            Station theStation = Station.GetByCode(station);
            ViewBag.Station = theStation;
            ViewBag.YYYY = YYYY;
            ViewBag.MM = MM;
            ViewBag.DD = DD;

            DateTime currDate = new DateTime(YYYY, MM, DD);
            DateTime nextDate = currDate.AddDays(1);
            DateTime prevDate = currDate.AddDays(-1);

            ViewBag.currDate = currDate;
            ViewBag.prevDate = prevDate;
            ViewBag.nextDate = nextDate;

            if (theStation == null){
                ViewBag.Error = "Нет станции " + station.ToString() ;
                return View();
            }
            List<CodeMagma> theCodes = (List<CodeMagma>)CodeMagma.GetByPeriod(theStation, YYYY, MM, DD, YYYY, MM, DD);
            if (theCodes.Count == 0) {
                ViewBag.Error = "По станции " + station.ToString() + " нет данных";
                return View();
            }
            List<ViewMagma> theViews = new List<ViewMagma>();
            for (int i = 0; i < 8;i++ )
            {

                ViewMagma theView = null;
                if (i < theCodes.Count)
                {
                    theView =  new ViewMagma(theCodes[i]);
                }
                else
                {
                    theView = new ViewMagma(null);
                }
                theViews.Add(theView);
            }

            return View(theViews);
        }

    }
}
