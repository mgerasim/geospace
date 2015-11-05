﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace GeospaceMediana.Controllers
{
    public class PanelController : Controller
    {
        public ActionResult  Index()
        {
            ViewBag.IsLocal = Utils.Util.IsLocal();
            ViewBag.NameMenu = "Информационная панель";
            return View();
        }
    }
}
