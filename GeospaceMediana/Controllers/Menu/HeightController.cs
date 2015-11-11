using GeospaceEntity.Common;
using GeospaceEntity.Models;
using GeospaceEntity.Models.Codes;
using GeospaceMediana.Common;
using GeospaceMediana.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GeospaceMediana.Controllers
{
    public class HeightController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index(int stationCode = 43501, string start = "", int limit = 5, int step = 5, string type = "f0F2", int year = -1, int month = -1, int day = -1)
        {

            ViewBag.IsLocal = Utils.Util.IsLocal();

            @ViewBag.Title = "Геофизические данные";

            ViewBag.NameMenu = "Данные наблюдений (высоты)";

            if (type == "M3000F2")
            {
                ViewBag.Type = "M3000";
            }
            if (type == "f0F2")
            {
                ViewBag.Type = "f0";
            }

            DateTime nowDateTime;
            if (year < 0 && month < 0 && day < 0)
            {
                nowDateTime = DateTime.Now.AddDays(-3);
            }
            else nowDateTime = new DateTime(year, month, day);
            ViewBag.Date = nowDateTime;

            if (start == "")
            {
                start = String.Format("{0:yyyyMMdd}", nowDateTime);
            }
            ViewHeight Model = new ViewHeight(stationCode, start, limit, step);
            
            ViewBag.Station = Station.GetByCode(stationCode);

            return View(Model);
        }
    }
}