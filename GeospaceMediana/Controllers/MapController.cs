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

        public ActionResult Stations()
        {
            List<Station> listStations = Station.GetAll();

            return View(listStations);
        }

    }
}
