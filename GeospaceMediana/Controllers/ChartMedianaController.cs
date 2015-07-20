using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GeospaceMediana.Controllers
{
    public class ChartMedianaController : Controller
    {
        //
        // GET: /ChartMediana/

        public ActionResult Index(int year = -1, int month = -1, int station = 43501)
        {
            DateTime nowDateTime = DateTime.Now;

            if (month == -1)
            {
                month = nowDateTime.Month;
            }

            if (year == -1)
            {
                year = nowDateTime.Year;
            }

            var listSeries = new[]
            { 
                new { Number = 10, Name = "Smith" },
                new { Number = 10, Name = "John" } 
            }.ToList();

            return View();
        }

    }
}
