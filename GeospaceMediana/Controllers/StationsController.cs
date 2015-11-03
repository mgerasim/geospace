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

        public ActionResult Index(int stationCode = 43501, string type = "f0F2", int year = -1, int month = -1, int day = -1)
        {
            ViewBag.IsLocal = Utils.Util.IsLocal();
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
                nowDateTime = DateTime.Now;
            }
            else nowDateTime = new DateTime(year, month, day);
            ViewBag.Date = nowDateTime;

            ViewBag.Station = Station.GetByCode(stationCode);
            return View(GeospaceEntity.Models.Station.GetAll());
        }

        public ActionResult Create()
        {
            ViewBag.IsLocal = Utils.Util.IsLocal();
            return View();
        }
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            ViewBag.IsLocal = Utils.Util.IsLocal();
            try
            {
                Station model = new GeospaceEntity.Models.Station(); ;
                model.Name = collection.Get("Name");
                model.Latitude = Convert.ToDouble(collection.Get("Latitude"));
                model.Longitude = Convert.ToDouble(collection.Get("Longitude"));
                model.Save();
                return RedirectToAction("Index");
            }
            catch
            {
                return this.Create();
            }
        }

        public ActionResult Edit(int id)
        {
            ViewBag.IsLocal = Utils.Util.IsLocal();
            return View(GeospaceEntity.Models.Station.GetById(id));
        }
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            ViewBag.IsLocal = Utils.Util.IsLocal();
            try
            {   
                Station model = Station.GetById(id);
                model.Name = collection.Get("Name");
                model.Code = Convert.ToInt32(collection.Get("Code"));
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
            ViewBag.IsLocal = Utils.Util.IsLocal();
            Station model = Station.GetById(id);
            return View(model);
        }

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            ViewBag.IsLocal = Utils.Util.IsLocal();
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
