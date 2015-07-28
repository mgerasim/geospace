using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GeospaceMediana.Models;
using GeospaceEntity.Models;


namespace GeospaceMediana.Controllers
{
    public class DropoutController : Controller
    {
        //
        // GET: /Test/

        public ActionResult Index()
        {

            return View(GeospaceEntity.Models.Error.GetAll());
        }

    }
}
