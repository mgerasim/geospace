using GeospaceEntity.Common;
using GeospaceEntity.Models;
using GeospaceMediana.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GeospaceMediana.Controllers
{
    public class ForecastTrackController : Controller
    {

        public ActionResult Index( int year = -1, int month = -1, int day = -1)
        {
            ViewBag.Title = "Действующие значения МПЧ";

            DateTime nowDateTime;
            if (year < 0 && month < 0 && day < 0)
            {
                nowDateTime = DateTime.Now.AddDays(-1);
            }
            else nowDateTime = new DateTime(year, month, day);

            DateTime prevDate = nowDateTime.AddDays(-1);
            DateTime nextDate = nowDateTime.AddDays(1);

            ViewBag.Date = nowDateTime;
            ViewBag.prevDate = prevDate;
            ViewBag.nextDate = nextDate;

            return View( new ViewForecastTrack(nowDateTime)); 
        }       
    }
}
