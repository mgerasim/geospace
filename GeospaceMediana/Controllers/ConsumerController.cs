using GeospaceEntity.Common;
using GeospaceEntity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GeospaceMediana.Controllers
{
    public class ConsumerController : Controller
    {
        //
        // GET: /Consumers/

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
            return View(GeospaceEntity.Models.Consumer.GetAll());
        }

        public ActionResult Edit(int id)
        {
            ViewBag.IsLocal = Utils.Util.IsLocal();
            ViewBag.Tracks = Track.GetAll();
            return View(GeospaceEntity.Models.Consumer.GetById(id));
        }

      
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            
            try
            {
                ViewBag.IsLocal = Utils.Util.IsLocal();
                Consumer model = Consumer.GetById(id);
                model.Name = collection.Get("Name");

                string tracksFomView = collection.Get("tracksFromView");
                if( tracksFomView != null )
                {
                    model.ClearTracks();
                    foreach( string strID in tracksFomView.Split(',') )
                    {
                        model.Tracks.Add( Track.GetById( Convert.ToInt32( strID ) ) );
                    }
                }

                
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
            ViewBag.IsLocal = Utils.Util.IsLocal();
            ViewBag.Tracks = Track.GetAll();
            return View();
        }
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                ViewBag.IsLocal = Utils.Util.IsLocal();
                Consumer model = new GeospaceEntity.Models.Consumer(); ;
                model.Name = collection.Get("Name");
                string tracksFomView = collection.Get("tracksFromView");
                if (tracksFomView != null)
                {
                    model.ClearTracks();
                    foreach (string strID in tracksFomView.Split(','))
                    {
                        model.Tracks.Add(Track.GetById(Convert.ToInt32(strID)));
                    }
                }

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
            ViewBag.IsLocal = Utils.Util.IsLocal();
            Consumer model = Consumer.GetById(id);

            return View(model);
        }

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                ViewBag.IsLocal = Utils.Util.IsLocal();
                Consumer model = Consumer.GetById(id);

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
