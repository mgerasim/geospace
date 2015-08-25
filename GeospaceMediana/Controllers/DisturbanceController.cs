using GeospaceEntity.Models;
using GeospaceMediana.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GeospaceMediana.Controllers
{
    public class DisturbanceController : Controller
    {
        //
        // GET: /Disturbance/

        public ActionResult Index(int YYYY = -1, int MM = -1)
        {
            if (YYYY < 0)
            {
                YYYY = DateTime.Now.Year;
            }
            if (MM < 0)
            {
                MM = DateTime.Now.Month;
            }

            ViewDisturbanceList theViewData = new ViewDisturbanceList(YYYY, MM);            
            List<ViewDisturbance> theDisturbanceList = new List<ViewDisturbance>();
            ViewBag.YYYY = YYYY;
            ViewBag.MM = MM;
            Station stationKhabarovsk = Station.GetByCode(43501);
            theViewData.theStationList.Add(stationKhabarovsk);
            foreach (var item in Disturbance.GetByMonth(stationKhabarovsk, YYYY, MM))
            {
                ViewDisturbance theDisturbance = new ViewDisturbance(item);
                theDisturbanceList.Add(theDisturbance);
            }
            
            Station stationMagadan = Station.GetByCode(45601);
            theViewData.theStationList.Add(stationMagadan);
            foreach (var item in Disturbance.GetByMonth(stationMagadan, YYYY, MM))
            {
                ViewDisturbance theDisturbance = new ViewDisturbance(item);
                theDisturbanceList.Add(theDisturbance);
            }


            Station stationSalekhard = Station.GetByCode(37701);
            theViewData.theStationList.Add(stationSalekhard);
            foreach (var item in Disturbance.GetByMonth(stationSalekhard, YYYY, MM))
            {
                ViewDisturbance theDisturbance = new ViewDisturbance(item);
                theDisturbanceList.Add(theDisturbance);
            }


            Station stationParatunka = Station.GetByCode(46501);
            theViewData.theStationList.Add(stationParatunka);
            foreach (var item in Disturbance.GetByMonth(stationParatunka, YYYY, MM))
            {
                ViewDisturbance theDisturbance = new ViewDisturbance(item);
                theDisturbanceList.Add(theDisturbance);
            }
            theViewData.theDisturbanceList = theDisturbanceList;
            return View(theViewData);
        }

    }
}
