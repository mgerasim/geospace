using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GeospaceEntity.Models;

namespace GeospaceMediana.Controllers
{
    public class ConsolidatedTableController : Controller
    {
        //
        // GET: /ConsolidatedTable/

        public ActionResult Index( int YYYY = -1, int MM = -1)
        {


            
            if (YYYY < 0)
            {
                YYYY = DateTime.Now.Year;
            }
            if (MM < 0)
            {
                MM = DateTime.Now.Month;
            }
            DateTime startMonth = new DateTime(YYYY, MM, 1);
            ViewBag.DateString = startMonth.ToString("MMMM yyyy", System.Globalization.CultureInfo.CurrentCulture);
            ViewBag.Year = YYYY;
            ViewBag.Month = MM;
            IList<ConsolidatedTable> tableView = ConsolidatedTable.GetByDateMM(2015, 8);

            return View(tableView);
        }

    }
}
