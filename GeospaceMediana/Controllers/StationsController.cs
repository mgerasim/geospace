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
            return View(GeospaceEntity.Models.Station.GetAll());
        }

        public ActionResult Edit(int id)
        {
            return View(GeospaceEntity.Models.Station.GetById(id));
        }
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            
            try
            {   
                Station model = Station.GetById(id);
                model.Name = collection.Get("Name");
                model.Latitude = Convert.ToDouble(collection.Get("Latitude"));
                model.Longitude = Convert.ToDouble(collection.Get("Longitude"));
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
            Station model = Station.GetById(id);

            return View(model);
        }

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                Station model = Station.GetById(id);

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
