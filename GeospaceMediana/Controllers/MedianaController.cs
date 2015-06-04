using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GeospaceMediana.Controllers
{
    public class MedianaController : Controller
    {
        //
        // GET: /Mediana/

        public ActionResult Index()
        {
            @ViewBag.Title = "Медиана";
            return View();
        }

    }
}
