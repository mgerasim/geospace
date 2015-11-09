using GeospaceEntity.Common;
using GeospaceEntity.Models;
using GeospaceMediana.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GeospaceMediana.Controllers
{
    public class ForecastFiveDayTrackController : Controller
    {
        public ActionResult Index( int id =-1, int month = -1, int W = -1 )
        {
            ViewBag.IsLocal = Utils.Util.IsLocal();
            ViewBag.Title = "Пятидневный прогноз радиотрасс";

            return View(new ViewFiveDayForecastTrack(id, month, W));
        }

        public ActionResult Calc(ViewFiveDayForecastTrack forecast)
        {
            ViewBag.IsLocal = Utils.Util.IsLocal();
            return View("Index", forecast);
        }

        [HttpPost]
        public ActionResult Calc(FormCollection collection)
        {
            ViewBag.IsLocal = Utils.Util.IsLocal();
            int id = Convert.ToInt32(collection.Get("Consumer"));
            int W = Convert.ToInt32(collection.Get("W"));
            int month = Convert.ToInt32(collection.Get("month"));

            ViewFiveDayForecastTrack forecast = new ViewFiveDayForecastTrack( id, month, W );
            forecast.Calc( id );

            return this.Calc(forecast);
        }
    }
}
