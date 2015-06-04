using GeospaceMediana.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GeospaceMediana.Controllers
{
    public class IonkaController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index(int station = 43501, string start = "", int limit = 5, int step = 5 )
        {
            @ViewBag.Title = "Геофизические данные";
            if (start == "")
            {
                start = String.Format("{0:yyyymmdd}", DateTime.Now.AddDays(-1));
            }
            ViewIonka Model = new ViewIonka(station, start, limit, step);

            return View(Model);
        }

    }
}