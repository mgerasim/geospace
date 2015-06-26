using GeospaceEntity.Common;
using GeospaceEntity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GeospaceMediana.Controllers
{
    public class StationsController : Controller
    {
        //
        // GET: /Stations/

        public ActionResult Index()
        {
            GeospaceEntity.Models.Station theStation = new GeospaceEntity.Models.Station();
            return View(theStation.GetAll());
        }

        public ActionResult Edit(int id)
        {
            GeospaceEntity.Models.Station theStation = new GeospaceEntity.Models.Station();
            return View(theStation.GetById(id));
        }
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            
            try
            {   
                Station model = new Station();
                model = model.GetById(id);            
                model.Name = collection.Get("Name");
                model.Update();
                return RedirectToAction("Index");
            }
            catch
            {
                return this.Edit(id);
            }
        }

        public ActionResult Delete(int id)
        {
            Station model = new Station();
            model = model.GetById(id);

            return View(model);
        }

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                Station model = new Station();
                model = model.GetById(id);

                model.Delete();

                return RedirectToAction("Index");
            }
            catch(Exception e)
            {
                ViewBag.error = e.ToString();

                return View();
            }
        }

    }
}
