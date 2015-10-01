using GeospaceEntity.Common;
using GeospaceEntity.Models;
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

        public ActionResult Index(int stationCode = 43501, string type = "f0F2", int year = -1, int month = -1, int day = -1)
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
            return View(GeospaceEntity.Models.Track.GetAll());
        }

        public ActionResult Calc(int id)
        {
            try
            {
                List<string> output = new List<string>();
                List<string> log = new List<string>();
                log.Add("");

                output.Add("[");   //MUF
                output.Add("[");   //OPF
                output.Add("");    //параметры: D
                int W = 10;

                Track track = Track.GetById(id);
                string param = track.PointA.Longitude.ToString().Replace(",", ".") + " "
                    + track.PointA.Latitude.ToString().Replace(",", ".") + " "
                    + track.PointB.Longitude.ToString().Replace(",", ".") + " "
                    + track.PointB.Latitude.ToString().Replace(",", ".") + " "
                    + W.ToString() + " "
                    + "1";
                    //+ DateTime.Now.AddMonths(1).Month.ToString();
                
                //GeospaceEntity.Helper.HelperTrack.Start(log, output, param, true, true);

                ViewBag.debug = log[0];

                return View("Index", GeospaceEntity.Models.Track.GetAll());
            }
            catch (System.Exception ex)
            {
                return Content("ошибка " + ex.Message);
            }       
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
