using GeospaceEntity.Common;
using GeospaceEntity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GeospaceMediana.Controllers
{
    public class ForecastMonthTrackController : Controller
    {

        public ActionResult Index()
        {
            ViewBag.Title = "Месячный прогноз радиотрасс";

            return View( GeospaceEntity.Models.Consumer.GetAll() ); 
        }

        public ActionResult Calc()
        {
            return View("Index", GeospaceEntity.Models.Consumer.GetAll());
        }

        [HttpPost]
        public ActionResult Calc(FormCollection collection)
        {
            int id = Convert.ToInt32(collection.Get("Consumer"));
            int W = Convert.ToInt32(collection.Get("W"));
            Consumer consumer = GeospaceEntity.Models.Consumer.GetById(id);

            List<string> output = new List<string>();
            output.Add("");

            foreach( Track track in consumer.Tracks )
            {
                string param = track.PointA.Longitude.ToString().Replace(",", ".") + " "
                + track.PointA.Latitude.ToString().Replace(",", ".") + " "
                + track.PointB.Longitude.ToString().Replace(",", ".") + " "
                + track.PointB.Latitude.ToString().Replace(",", ".") + " "
                + W.ToString() + " "
                + DateTime.Now.AddMonths(1).Month.ToString();

                GeospaceEntity.Helper.HelperTrack.Start(output, param);
            }

            return this.Calc();//View("Index", GeospaceEntity.Models.Consumer.GetAll());
        }
    }
}
