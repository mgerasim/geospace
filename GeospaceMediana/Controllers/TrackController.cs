using GeospaceEntity.Common;
using GeospaceEntity.Models;
using GeospaceMediana.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GeospaceMediana.Controllers
{
    public class TrackController : Controller
    {
        //
        // GET: /Tracks/

        public ActionResult Index(int stationCode = 43501, string type = "f0F2", int year = -1, int month = -1, int day = -1, int calc = -1)
        {
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

            ViewBag.debug = "";

            ViewBag.Station = Station.GetByCode(stationCode);
            ViewTrack alltracks = new ViewTrack(calc);
            return View(alltracks);
        }

        public ActionResult Edit(int id)
        {
            ViewBag.Posts = Post.GetAll();
            return View(GeospaceEntity.Models.Track.GetById(id));
        }
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            
            try
            {   
                Track model = Track.GetById(id);
                model.Name = collection.Get("Name");
                model.PointA = Post.GetById(Convert.ToInt32(collection.Get("PostA")));
                model.PointB = Post.GetById(Convert.ToInt32(collection.Get("PostB")));
                model.Update();
                return RedirectToAction("Index");
            }
            catch
            {
                return this.Edit(id);
            }
        }

        public ActionResult Create()
        {
            ViewBag.Posts = Post.GetAll();
            return View();
        }
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                Track model = new GeospaceEntity.Models.Track(); ;
                model.Name = collection.Get("Name");
                model.PointA = Post.GetById( Convert.ToInt32( collection.Get("PostA") ));
                model.PointB = Post.GetById(Convert.ToInt32(collection.Get("PostB")));

                model.Save();
                //GeospaceEntity.return RedirectToAction("Index");
                return this.Create();
            }
            catch
            {
                return this.Create();
            }
        }

        public ActionResult Delete(int id)
        {
            Track model = Track.GetById(id);

            return View(model);
        }

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                Track model = Track.GetById(id);

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
