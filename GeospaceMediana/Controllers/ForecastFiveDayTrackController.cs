﻿using GeospaceEntity.Common;
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
        public ActionResult Index( int month = -1, int W = -1 )
        {
            ViewBag.Title = "Пятидневный прогноз радиотрасс";

            return View(new ViewFiveDayForecastTrack(month, W));
        }

        public ActionResult Calc(ViewFiveDayForecastTrack forecast)
        {
            return View("Index", forecast);
        }

        [HttpPost]
        public ActionResult Calc(FormCollection collection)
        {            
            int id = Convert.ToInt32(collection.Get("Consumer"));
            int W = Convert.ToInt32(collection.Get("W"));
            int month = Convert.ToInt32(collection.Get("month"));

            ViewFiveDayForecastTrack forecast = new ViewFiveDayForecastTrack( month, W );
            forecast.Calc( id );

            return this.Calc(forecast);
        }
    }
}
