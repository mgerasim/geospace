using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GeospaceMediana.Controllers
{
    public class TelegramsubController : Controller
    {
        //
        // GET: /Telegramsub/

        public ActionResult Index()
        {
            ViewBag.IsLocal = Utils.Util.IsLocal();
            return View();
        }

    }
}
