using GeospaceEntity.Common;
using GeospaceEntity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GeospaceMediana.Controllers
{
    public class ForecastTrackController : Controller
    {

        public ActionResult Index()
        {
            ViewBag.Title = "Фактический прогноз радиотрасс";
            return Content("Фактический прогноз радиотрасс"); 
        }       
    }
}
