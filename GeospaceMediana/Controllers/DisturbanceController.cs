using GeospaceEntity.Models;
using GeospaceMediana.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GeospaceMediana.Controllers
{
    public class DisturbanceController : Controller
    {
        //
        // GET: /Disturbance/

        public ActionResult Index(int YYYY = -1, int MM = -1)
        {
            if (YYYY < 0)
            {
                YYYY = DateTime.Now.Year;
            }
            if (MM < 0)
            {
                MM = DateTime.Now.Month;
            }

            ViewDisturbanceList theViewData = new ViewDisturbanceList(YYYY, MM);            
            List<ViewDisturbance> theDisturbanceList = new List<ViewDisturbance>();
            ViewBag.YYYY = YYYY;
            ViewBag.MM = MM;


            Station stationSalekhard = Station.GetByCode(37701);
            theViewData.theStationList.Add(stationSalekhard);
            foreach (var item in Disturbance.GetByMonth(stationSalekhard, YYYY, MM))
            {
                ViewDisturbance theDisturbance = new ViewDisturbance(item);
                theDisturbanceList.Add(theDisturbance);
            }

            Station stationMagadan = Station.GetByCode(45601);
            theViewData.theStationList.Add(stationMagadan);
            foreach (var item in Disturbance.GetByMonth(stationMagadan, YYYY, MM))
            {
                ViewDisturbance theDisturbance = new ViewDisturbance(item);
                theDisturbanceList.Add(theDisturbance);
            }


            Station stationKhabarovsk = Station.GetByCode(43501);
            theViewData.theStationList.Add(stationKhabarovsk);
            foreach (var item in Disturbance.GetByMonth(stationKhabarovsk, YYYY, MM))
            {
                ViewDisturbance theDisturbance = new ViewDisturbance(item);
                theDisturbanceList.Add(theDisturbance);
            }          

            Station stationParatunka = Station.GetByCode(46501);
            theViewData.theStationList.Add(stationParatunka);
            foreach (var item in Disturbance.GetByMonth(stationParatunka, YYYY, MM))
            {
                ViewDisturbance theDisturbance = new ViewDisturbance(item);
                theDisturbanceList.Add(theDisturbance);
            }
            theViewData.theDisturbanceList = theDisturbanceList;
            return View(theViewData);
        }
        public ActionResult Submit(int station, int year, int month, int day, int hour,int duration)
        {
            try
            {
                Station stationCode = Station.GetByCode(station);
                for (int i = hour; i < hour + duration; ++i)
                {
                    GeospaceEntity.Models.Disturbance disturbanceSave = GeospaceEntity.Models.Disturbance.GetByTime(stationCode, year, month, day, i, 0);

                    if (disturbanceSave == null)
                    {
                        disturbanceSave = new GeospaceEntity.Models.Disturbance();

                        disturbanceSave.Station = stationCode;
                        disturbanceSave.YYYY = year;
                        disturbanceSave.MM = month;
                        disturbanceSave.DD = day;
                        disturbanceSave.HH = i;
                        disturbanceSave.MI = 0;

                        disturbanceSave.Save();
                    }
                    else
                    {
                        disturbanceSave.HH = i;
                        disturbanceSave.MI = 0;
                        disturbanceSave.Update();
                    }
                }
               

                return Content("");
            }
            catch (Exception)
            {
                return Content("Ошибка при отправлении данных! Проверьте корректность вводимых данных.");
            }
        }

        public ActionResult Delete(int station, int year, int month, int day, int hour, int duration)
        {
            try
            {
                Station stationCode = Station.GetByCode(station);
                for (int i = hour; i < hour + duration; ++i)
                {
                    GeospaceEntity.Models.Disturbance disturbanceDelete = GeospaceEntity.Models.Disturbance.GetByTime(stationCode, year, month, day, i, 0);

                    if (disturbanceDelete != null)
                    {
                        disturbanceDelete.Delete();
                    }
                }


                return Content("");
            }
            catch (Exception)
            {
                return Content("Ошибка при отправлении данных! Проверьте корректность вводимых данных.");
            }
        }

        public ActionResult GetHoursForHtmlModalBody(int StationCode, int YYYY, int MM, int DD)
        {            
            ViewBag.StationCode = StationCode;
            ViewBag.YYYY = YYYY;
            ViewBag.MM = MM;
            ViewBag.DD = DD;
            Station theStation = Station.GetByCode(StationCode);
            if (theStation != null) {
                List<Disturbance> theList = Disturbance.GetByDay(theStation, YYYY, MM, DD);
                return View(theList);
            }
            return View(new List<Disturbance>());
        }
        public ActionResult Add(int StationCode, int YYYY, int MM, int DD, int HH)
        {
            try
            {
                Station stationCode = Station.GetByCode(StationCode);
                GeospaceEntity.Models.Disturbance disturbanceSave = GeospaceEntity.Models.Disturbance.GetByTime(stationCode, YYYY, MM, DD, HH, 0);
                
                if (disturbanceSave == null)
                {
                    disturbanceSave = new GeospaceEntity.Models.Disturbance();
                    disturbanceSave.Station = stationCode;
                    disturbanceSave.YYYY = YYYY;
                    disturbanceSave.MM = MM;
                    disturbanceSave.DD = DD;
                    disturbanceSave.HH = HH;
                    disturbanceSave.MI = 0;
                    disturbanceSave.Save();

                }
                else
                {

                    disturbanceSave.Station = stationCode;
                    disturbanceSave.YYYY = YYYY;
                    disturbanceSave.MM = MM;
                    disturbanceSave.DD = DD;
                    disturbanceSave.HH = HH;
                    disturbanceSave.MI = 0;
                    disturbanceSave.Update();
                
                }





                return Content("");
            }
            catch (Exception)
            {
                return Content("Ошибка при отправлении данных! Проверьте корректность вводимых данных.");
            }
        }

        public ActionResult Remove(int StationCode, int YYYY, int MM, int DD, int HH)
        {
            try
            {
                Station theStation = Station.GetByCode(StationCode);

                GeospaceEntity.Models.Disturbance disturbanceDelete = GeospaceEntity.Models.Disturbance.GetByTime(theStation, YYYY, MM, DD, HH, 0);

                if (disturbanceDelete != null)
                {
                    disturbanceDelete.Delete();
                }
                


                return Content("");
            }
            catch (Exception)
            {
                return Content("Ошибка при отправлении данных! Проверьте корректность вводимых данных.");
            }
        }

        public ActionResult Display(int StationCode, int YYYY, int MM, int DD)
        {
            try
            {
                Station theStation = Station.GetByCode(StationCode);


                ViewDisturbanceList theViewData = new ViewDisturbanceList(YYYY, MM);
                List<ViewDisturbance> theDisturbanceList = new List<ViewDisturbance>();
                ViewBag.YYYY = YYYY;
                ViewBag.MM = MM;


                theViewData.theStationList.Add(theStation);
                foreach (var item in Disturbance.GetByDay(theStation, YYYY, MM, DD))
                {
                    ViewDisturbance theDisturbance = new ViewDisturbance(item);
                    theDisturbanceList.Add(theDisturbance);
                }

                theViewData.theDisturbanceList = theDisturbanceList;

                return Content(theViewData.Display(StationCode, YYYY, MM, DD));
            }
            catch (Exception)
            {
                return Content("Ошибка при отправлении данных! Проверьте корректность вводимых данных.");
            }
        }

    }

}
