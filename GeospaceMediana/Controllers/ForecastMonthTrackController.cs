using GeospaceEntity.Common;
using GeospaceEntity.Models;
using GeospaceEntity.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
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
            int month = Convert.ToInt32(collection.Get("month"));
            Consumer consumer = GeospaceEntity.Models.Consumer.GetById(id);

            List<List<string>> listOutput = new List<List<string>>();
            List<string> title = new List<string>();
            DateTime dt = new DateTime(DateTime.Now.Year, month, 1);
            List<string> log = new List<string>();
            log.Add("");

            IRepository<Settings> repo = new SettingsRepository();
            string exePath = repo.GetAll().Select(x => x.MonthForecastTrack).ToList()[0];
            

            foreach( Track track in consumer.Tracks )
            {
                log[0] += track.Name + "\n";
                List<string> output = new List<string>();
                output.Add("[");   //MUF
                output.Add("[");   //OPF
                output.Add("");    //параметры: D

                string param = track.PointA.Longitude.ToString().Replace(",", ".") + " "
                + track.PointA.Latitude.ToString().Replace(",", ".") + " "
                + track.PointB.Longitude.ToString().Replace(",", ".") + " "
                + track.PointB.Latitude.ToString().Replace(",", ".") + " "
                + W.ToString() + " "
                //+ DateTime.Now.AddMonths(1).Month.ToString();
                + month.ToString();

                GeospaceEntity.Helper.HelperTrack.Start(log, output, param, "\\MonthForecast\\MonthForecast.f90", exePath, true, true);

                output[0] += "]";
                output[1] += "]";
                

                string [] str = output[2].Split(' ');
                title.Add(track.Name + "\nКоординаты: " + track.PointA.Longitude.ToString() + " "
                    + track.PointA.Latitude.ToString() + " - "
                    + track.PointB.Longitude.ToString() + " "
                    + track.PointB.Latitude.ToString() + "\n"
                    + "Длина трассы: " + str[0] + " км\n" 
                    + "Прогноз на: " + dt.ToString("MMM yyyy") );                

                listOutput.Add(output);
                log[0] += "\n";
            }

            StreamWriter sw = new StreamWriter("C:\\inetpub\\wwwroot\\mediana\\bin2\\MonthForecast_log.txt");
            foreach (string s in log)
                sw.WriteLine(s);
            sw.Close();

            ViewBag.title = title;
            ViewBag.listOutput = listOutput;
            ViewBag.quantity = consumer.Tracks.Count;

            return this.Calc();//View("Index", GeospaceEntity.Models.Consumer.GetAll());
        }
    }
}
