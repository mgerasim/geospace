using GeospaceEntity.Common;
using GeospaceEntity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GeospaceMediana.Controllers
{
    public class ForecastFiveDaysTrackController : Controller
    {

        public ActionResult Index()
        {
            ViewBag.Title = "Пятидневный прогноз радиотрасс";
            return Content("Пятидневный прогноз радиотрасс"); 
        }       
    }
}
