using GeospaceEntity.Common;
using GeospaceEntity.Models;
using GeospaceEntity.Repositories;
using GeospaceMediana.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GeospaceMediana.Controllers
{
    public class ForecastMonthTrackController : Controller
    {

        public ActionResult Index(int id = -1, int month = -1, int W = -1)
        {
            ViewBag.Title = "Месячный прогноз радиотрасс";

            return View(new ViewMonthForecastTrack(id, month, W)); 
        }

        public ActionResult Calc(ViewMonthForecastTrack forecast)
        {
            return View("Index", forecast);
        }

        [HttpPost]
        public ActionResult Calc(FormCollection collection)
        {
            int id = Convert.ToInt32(collection.Get("Consumer"));
            int W = Convert.ToInt32(collection.Get("W"));
            int month = Convert.ToInt32(collection.Get("month"));

            ViewMonthForecastTrack forecast = new ViewMonthForecastTrack(id, month, W);
            forecast.Calc(id);

            return this.Calc(forecast);
        }
    }
}
