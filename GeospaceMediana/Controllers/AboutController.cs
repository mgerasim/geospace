using GeospaceEntity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GeospaceMediana.Controllers
{
    public class AboutController : Controller
    {
        //
        // GET: /About/

        public ActionResult Index(int stationCode=43501)
        {
            ViewBag.Station = Station.GetByCode(stationCode);
            ViewBag.Date = DateTime.Now;
            return View();
        }

        public ActionResult Api()
        {
            return View();
        }

    }
}
