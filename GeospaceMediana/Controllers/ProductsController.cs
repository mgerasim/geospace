using GeospaceEntity.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GeospaceMediana.Controllers
{
    public class ProductsController : Controller
    {
        //
        // GET: /Products/

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
            List<GeospaceEntity.Models.Product> theList = (new GeospaceEntity.Models.Product()).GetAll();
            GeospaceEntity.Models.Product theProduct = null;

            if (theList.Count == 0)
            {
                theProduct = new GeospaceEntity.Models.Product();
                theProduct.Save();
            }
            else
            {
                theProduct = theList[0];
            }

            return View(theProduct);
        }
        public ActionResult SaveForecastDaysFives(string text)
        {
            try
            {
                ViewBag.IsLocal = Utils.Util.IsLocal();
                List<GeospaceEntity.Models.Product> theList = (new GeospaceEntity.Models.Product()).GetAll();
                GeospaceEntity.Models.Product theProduct = null;

                if (theList.Count == 0)
                {
                    theProduct = new GeospaceEntity.Models.Product();
                    theProduct.forecast_days_fives = text;
                    theProduct.Save();
                }
                else
                {
                    theProduct = theList[0];
                    theProduct.forecast_days_fives = text;
                    theProduct.Update();
                }
            }
            catch(Exception ex)
            {
                ViewBag.Error = ex.Message;
            }
            return View();
        }
        public ActionResult SaveReviewGeoEnvMonth(string text)
        {
            try
            {
                ViewBag.IsLocal = Utils.Util.IsLocal();
                List<GeospaceEntity.Models.Product> theList = (new GeospaceEntity.Models.Product()).GetAll();
                GeospaceEntity.Models.Product theProduct = null;

                if (theList.Count == 0)
                {
                    theProduct = new GeospaceEntity.Models.Product();
                    theProduct.review_geoenv_month = text;
                    theProduct.Save();
                }
                else
                {
                    theProduct = theList[0];
                    theProduct.review_geoenv_month = text;
                    theProduct.Update();
                }
            }
            catch(Exception ex)
            {
                ViewBag.Error = ex.Message;
            }
            return View();
        }

        public ActionResult SaveSubdayForecast(string text)
        {
            try
            {
                ViewBag.IsLocal = Utils.Util.IsLocal();
                List<GeospaceEntity.Models.Product> theList = (new GeospaceEntity.Models.Product()).GetAll();
                GeospaceEntity.Models.Product theProduct = null;

                if (theList.Count == 0)
                {
                    theProduct = new GeospaceEntity.Models.Product();
                    theProduct.subday_forecast = text;
                    theProduct.Save();
                }
                else
                {
                    theProduct = theList[0];
                    theProduct.subday_forecast = text;
                    theProduct.Update();
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
            }
            return View();
        }

        public ActionResult SaveReviewGeoEnv(string text)
        {
            try
            {
                ViewBag.IsLocal = Utils.Util.IsLocal();
                List<GeospaceEntity.Models.Product> theList = (new GeospaceEntity.Models.Product()).GetAll();
                GeospaceEntity.Models.Product theProduct = null;

                if (theList.Count == 0)
                {
                    theProduct = new GeospaceEntity.Models.Product();
                    theProduct.review_geoenv = text;
                    theProduct.Save();
                }
                else
                {
                    theProduct = theList[0];
                    theProduct.review_geoenv = text;
                    theProduct.Update();
                }
            }
            catch(Exception ex)
            {
                ViewBag.Error = ex.Message;
            }
            return View();
        }
        public ActionResult SaveForecastMonthIonosphera(string text)
        {
            try
            {
                ViewBag.IsLocal = Utils.Util.IsLocal();
                List<GeospaceEntity.Models.Product> theList = (new GeospaceEntity.Models.Product()).GetAll();
                GeospaceEntity.Models.Product theProduct = null;

                if (theList.Count == 0)
                {
                    theProduct = new GeospaceEntity.Models.Product();
                    theProduct.forecast_month_ionosphera = text;
                    theProduct.Save();
                }
                else
                {
                    theProduct = theList[0];
                    theProduct.forecast_month_ionosphera = text;
                    theProduct.Update();
                }
            }
            catch(Exception ex)
            {
                ViewBag.Error = ex.Message;
            }
            return View();
        }

        [HttpPost]
        public ActionResult SaveDisturbanceRadio(HttpPostedFileBase file)
        {
            ViewBag.IsLocal = Utils.Util.IsLocal();
            if (file != null && file.ContentLength > 0)
            {

                var fileName = Path.GetFileName(file.FileName);
                var path = Path.Combine(Server.MapPath("~/App_Data"), fileName);
                file.SaveAs(path);

                var text = fileName;

                try
                {

                    List<GeospaceEntity.Models.Product> theList = (new GeospaceEntity.Models.Product()).GetAll();
                    GeospaceEntity.Models.Product theProduct = null;

                    if (theList.Count == 0)
                    {
                        theProduct = new GeospaceEntity.Models.Product();
                        theProduct.disturbance_radio = text;
                        theProduct.Save();
                    }
                    else
                    {
                        theProduct = theList[0];
                        theProduct.disturbance_radio = text;
                        theProduct.Update();
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.Error = ex.Message;
                }


            }
            // redirect back to the index action to show the form once again
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult SaveTableSun(HttpPostedFileBase file)
        {
            ViewBag.IsLocal = Utils.Util.IsLocal();
            if (file != null && file.ContentLength > 0)
            {
                
                var fileName = Path.GetFileName(file.FileName);                
                var path = Path.Combine(Server.MapPath("~/App_Data"), fileName);
                file.SaveAs(path);

                var text = fileName;

                try
                {

                    List<GeospaceEntity.Models.Product> theList = (new GeospaceEntity.Models.Product()).GetAll();
                    GeospaceEntity.Models.Product theProduct = null;

                    if (theList.Count == 0)
                    {
                        theProduct = new GeospaceEntity.Models.Product();
                        theProduct.table_sun = text;
                        theProduct.Save();
                    }
                    else
                    {
                        theProduct = theList[0];
                        theProduct.table_sun = text;
                        theProduct.Update();
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.Error = ex.Message;
                }

                
            }
            // redirect back to the index action to show the form once again
            return RedirectToAction("Index");  
        }

        public ActionResult ShowForecastDaysFives()
        {
            ViewBag.IsLocal = Utils.Util.IsLocal();
            GeospaceEntity.Models.Product theProduct = null;
            try
            {

                List<GeospaceEntity.Models.Product> theList = (new GeospaceEntity.Models.Product()).GetAll();

                if (theList.Count == 0)
                {
                    theProduct = new GeospaceEntity.Models.Product();
                    theProduct.Save();
                }
                else
                {
                    theProduct = theList[0];
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
            }
            return View(theProduct);
        }
        public ActionResult ShowReviewGeoEnvMonth()
        {
            ViewBag.IsLocal = Utils.Util.IsLocal();
            GeospaceEntity.Models.Product theProduct = null;
            try
            {

                List<GeospaceEntity.Models.Product> theList = (new GeospaceEntity.Models.Product()).GetAll();

                if (theList.Count == 0)
                {
                    theProduct = new GeospaceEntity.Models.Product();
                    theProduct.Save();
                }
                else
                {
                    theProduct = theList[0];
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
            }
            return View(theProduct);
        }
        public ActionResult ShowReviewGeoEnv()
        {
            ViewBag.IsLocal = Utils.Util.IsLocal();
            GeospaceEntity.Models.Product theProduct = null;
            try
            {

                List<GeospaceEntity.Models.Product> theList = (new GeospaceEntity.Models.Product()).GetAll();

                if (theList.Count == 0)
                {
                    theProduct = new GeospaceEntity.Models.Product();
                    theProduct.Save();
                }
                else
                {
                    theProduct = theList[0];
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
            }
            return View(theProduct);
        }
        public ActionResult ShowForecastMonthIonosphera()
        {
            ViewBag.IsLocal = Utils.Util.IsLocal();
            GeospaceEntity.Models.Product theProduct = null;
            try
            {

                List<GeospaceEntity.Models.Product> theList = (new GeospaceEntity.Models.Product()).GetAll();

                if (theList.Count == 0)
                {
                    theProduct = new GeospaceEntity.Models.Product();
                    theProduct.Save();
                }
                else
                {
                    theProduct = theList[0];
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
            }
            return View(theProduct);
        }

        public ActionResult ShowSubdayForecast()
        {
            ViewBag.IsLocal = Utils.Util.IsLocal();
            GeospaceEntity.Models.Product theProduct = null;

            try
            {

                List<GeospaceEntity.Models.Product> theList = (new GeospaceEntity.Models.Product()).GetAll();

                if (theList.Count == 0)
                {
                    theProduct = new GeospaceEntity.Models.Product();
                    theProduct.Save();
                }
                else
                {
                    theProduct = theList[0];
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
            }


            return View(theProduct);
        }

        public ActionResult ShowDisturbanceRadio()
        {
            ViewBag.IsLocal = Utils.Util.IsLocal();
            GeospaceEntity.Models.Product theProduct = null;
            try
            {
                List<GeospaceEntity.Models.Product> theList = (new GeospaceEntity.Models.Product()).GetAll();
                if (theList.Count == 0)
                {
                    theProduct = new GeospaceEntity.Models.Product();
                    theProduct.Save();
                }
                else
                {
                    theProduct = theList[0];
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
            }
            return View(theProduct);
        }

        public ActionResult ShowTableSun()
        {
            ViewBag.IsLocal = Utils.Util.IsLocal();
            GeospaceEntity.Models.Product theProduct = null;
            try
            {
                List<GeospaceEntity.Models.Product> theList = (new GeospaceEntity.Models.Product()).GetAll();
                if (theList.Count == 0)
                {
                    theProduct = new GeospaceEntity.Models.Product();
                    theProduct.Save();
                }
                else
                {
                    theProduct = theList[0];
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
            }
            return View(theProduct);
        }

        public FileResult DownloadTableSun()
        {
            ViewBag.IsLocal = Utils.Util.IsLocal();
            GeospaceEntity.Models.Product theProduct = (new Product()).GetAll()[0];
            var fileName = theProduct.table_sun;
            var path = Path.Combine(Server.MapPath("~/App_Data"), fileName);

            byte[] fileBytes = System.IO.File.ReadAllBytes(path);
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }

        public FileResult DownloadDisturbanceRadio()
        {
            ViewBag.IsLocal = Utils.Util.IsLocal();
            GeospaceEntity.Models.Product theProduct = (new Product()).GetAll()[0];
            var fileName = theProduct.disturbance_radio;
            var path = Path.Combine(Server.MapPath("~/App_Data"), fileName);

            byte[] fileBytes = System.IO.File.ReadAllBytes(path);
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }

        public ActionResult EditForecastDaysFives()
        {
            ViewBag.IsLocal = Utils.Util.IsLocal();
            GeospaceEntity.Models.Product theProduct = null;
            try
            {
                List<GeospaceEntity.Models.Product> theList = (new GeospaceEntity.Models.Product()).GetAll();

                if (theList.Count == 0)
                {
                    theProduct = new GeospaceEntity.Models.Product();
                    theProduct.Save();
                }
                else
                {
                    theProduct = theList[0];
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
            }
            return View(theProduct);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult EditForecastDaysFives(FormCollection collection)
        {
            ViewBag.IsLocal = Utils.Util.IsLocal();
            GeospaceEntity.Models.Product theProduct = null;
            try
            {
                List<GeospaceEntity.Models.Product> theList = (new GeospaceEntity.Models.Product()).GetAll();

                if (theList.Count == 0)
                {
                    theProduct = new GeospaceEntity.Models.Product();
                    theProduct.Save();
                }
                else
                {
                    theProduct = theList[0];
                }
                theProduct.forecast_days_fives = "";
                string param = collection.Get("forecastdaysfives");
                foreach(var line in param.Split(new string[] { "\r\n" },StringSplitOptions.None))
                {
                    string ss = line;
                    ss = ss.Trim();
                    ss = ss.Replace("\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t", " ");
                    ss = ss.Replace("\t\t\t\t\t\t\t\t\t\t\t\t", " ");
                    ss = ss.Replace("\t\t\t\t\t\t\t\t\t\t", " ");
                    ss = ss.Replace("\t\t\t\t\t\t\t\t", " ");
                    theProduct.forecast_days_fives += ss + "\r\n";
                }
                theProduct.Update();
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
            }
            return RedirectToAction("Index");
        }

        public ActionResult EditForecastMonthIonosphera()
        {
            ViewBag.IsLocal = Utils.Util.IsLocal();
            GeospaceEntity.Models.Product theProduct = null;
            try
            {
                List<GeospaceEntity.Models.Product> theList = (new GeospaceEntity.Models.Product()).GetAll();

                if (theList.Count == 0)
                {
                    theProduct = new GeospaceEntity.Models.Product();
                    theProduct.Save();
                }
                else
                {
                    theProduct = theList[0];
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
            }
            return View(theProduct);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult EditForecastMonthIonosphera(FormCollection collection)
        {
            ViewBag.IsLocal = Utils.Util.IsLocal();
            GeospaceEntity.Models.Product theProduct = null;
            try
            {
                List<GeospaceEntity.Models.Product> theList = (new GeospaceEntity.Models.Product()).GetAll();

                if (theList.Count == 0)
                {
                    theProduct = new GeospaceEntity.Models.Product();
                    theProduct.Save();
                }
                else
                {
                    theProduct = theList[0];
                }
                theProduct.forecast_month_ionosphera = "";
                string param = collection.Get("forecastmonthionosphera");
                foreach (var line in param.Split(new string[] { "\r\n" }, StringSplitOptions.None))
                {
                    string ss = line;
                    ss = ss.Trim();
                    ss = ss.Replace("   ", " ");
                    ss = ss.Replace("  ", " ");
                    if (ss.Length < 5) continue;
                    switch (ss.Substring(0, 5))
                    {
                        case "ИОНЕС":
                        case "ИОНДП":
                        case "ИОНФФ":
                        case "МАГПО":
                            ss = "                    " + ss;
                            break;
                    }
                    theProduct.forecast_month_ionosphera += ss + "\r\n";
                }
                theProduct.Update();
                theProduct.Send_MonthForecast();
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
            }
            return RedirectToAction("Index");
        }

        public ActionResult EditReviewGeoEnv()
        {
            ViewBag.IsLocal = Utils.Util.IsLocal();
            GeospaceEntity.Models.Product theProduct = null;
            try
            {
                List<GeospaceEntity.Models.Product> theList = (new GeospaceEntity.Models.Product()).GetAll();

                if (theList.Count == 0)
                {
                    theProduct = new GeospaceEntity.Models.Product();
                    theProduct.Save();
                }
                else
                {
                    theProduct = theList[0];
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
            }
            return View(theProduct);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult EditReviewGeoEnv(FormCollection collection)
        {
            ViewBag.IsLocal = Utils.Util.IsLocal();
            GeospaceEntity.Models.Product theProduct = null;
            try
            {
                List<GeospaceEntity.Models.Product> theList = (new GeospaceEntity.Models.Product()).GetAll();

                if (theList.Count == 0)
                {
                    theProduct = new GeospaceEntity.Models.Product();
                    theProduct.Save();
                }
                else
                {
                    theProduct = theList[0];
                }
                theProduct.review_geoenv = "";
                string param = collection.Get("reviewgeoenv");
                foreach (var line in param.Split(new string[] { "\r\n" }, StringSplitOptions.None))
                {
                    string ss = line;
                    ss = ss.Trim();
                    theProduct.review_geoenv += ss + "\r\n";
                }
                theProduct.review_geoenv.Replace("\r\n\r\n\r\n", "");
                theProduct.Update();
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
            }
            return RedirectToAction("Index");
        }

        public ActionResult EditReviewGeoEnvMonth()
        {
            ViewBag.IsLocal = Utils.Util.IsLocal();
            GeospaceEntity.Models.Product theProduct = null;
            try
            {

                List<GeospaceEntity.Models.Product> theList = (new GeospaceEntity.Models.Product()).GetAll();

                if (theList.Count == 0)
                {
                    theProduct = new GeospaceEntity.Models.Product();
                    theProduct.Save();
                }
                else
                {
                    theProduct = theList[0];
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
            }
            return View(theProduct);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult EditReviewGeoEnvMonth(FormCollection collection)
        {
            ViewBag.IsLocal = Utils.Util.IsLocal();
            GeospaceEntity.Models.Product theProduct = null;
            try
            {
                List<GeospaceEntity.Models.Product> theList = (new GeospaceEntity.Models.Product()).GetAll();

                if (theList.Count == 0)
                {
                    theProduct = new GeospaceEntity.Models.Product();
                    theProduct.Save();
                }
                else
                {
                    theProduct = theList[0];
                }
                theProduct.review_geoenv_month = "";
                string param = collection.Get("reviewgeoenvmonth");
                foreach (var line in param.Split(new string[] { "\r\n" }, StringSplitOptions.None))
                {
                    string ss = line;
                    ss = ss.Trim();
                    theProduct.review_geoenv_month += ss + "\r\n";

                }
                theProduct.review_geoenv_month.Replace("\r\n\r\n\r\n", "");
                theProduct.Update();
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
            }

            return RedirectToAction("Index");
        }
        public ActionResult EditSubdayForecast()
        {
            ViewBag.IsLocal = Utils.Util.IsLocal();
            GeospaceEntity.Models.Product theProduct = null;
            try
            {
                List<GeospaceEntity.Models.Product> theList = (new GeospaceEntity.Models.Product()).GetAll();

                if (theList.Count == 0)
                {
                    theProduct = new GeospaceEntity.Models.Product();
                    theProduct.Save();
                }
                else
                {
                    theProduct = theList[0];
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
            }
            return View(theProduct);
        }
        
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult EditSubdayForecast(FormCollection collection)
        {
            ViewBag.IsLocal = Utils.Util.IsLocal();
            GeospaceEntity.Models.Product theProduct = null;
            try
            {
                List<GeospaceEntity.Models.Product> theList = (new GeospaceEntity.Models.Product()).GetAll();

                if (theList.Count == 0)
                {
                    theProduct = new GeospaceEntity.Models.Product();
                    theProduct.Save();
                }
                else
                {
                    theProduct = theList[0];
                }
                theProduct.subday_forecast = "";
                string param = collection.Get("subdayforecast");
                foreach (var line in param.Split(new string[] { "\r\n" }, StringSplitOptions.None))
                {
                    string ss = line;
                    ss = ss.Trim();
                    theProduct.subday_forecast += ss + "\r\n";
                }
                theProduct.subday_forecast = theProduct.subday_forecast.Replace("\r\n\r\n\r\n", "\r\n");
                theProduct.Update();
                theProduct.Send_SubdayForecast();
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
            }
            return RedirectToAction("Index");
        }
        public ActionResult EditDescription()
        {
            ViewBag.IsLocal = Utils.Util.IsLocal();
            GeospaceEntity.Models.Product theProduct = null;
            try
            {
                List<GeospaceEntity.Models.Product> theList = (new GeospaceEntity.Models.Product()).GetAll();

                if (theList.Count == 0)
                {
                    theProduct = new GeospaceEntity.Models.Product();
                    theProduct.Save();
                }
                else
                {
                    theProduct = theList[0];
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
            }
            return View(theProduct);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateInput(false)]  
        public ActionResult EditDescription(FormCollection collection)
        {
            ViewBag.IsLocal = Utils.Util.IsLocal();
            GeospaceEntity.Models.Product theProduct = null;
            try
            {
                List<GeospaceEntity.Models.Product> theList = (new GeospaceEntity.Models.Product()).GetAll();

                if (theList.Count == 0)
                {
                    theProduct = new GeospaceEntity.Models.Product();
                    theProduct.Save();
                }
                else
                {
                    theProduct = theList[0];
                }
                theProduct.description = collection.Get("description");
                
                theProduct.Update();
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
            }
            return RedirectToAction("Index");
        }

        public ActionResult EditTableSun()
        {
            ViewBag.IsLocal = Utils.Util.IsLocal();
            GeospaceEntity.Models.Product theProduct = null;
            try
            {
                List<GeospaceEntity.Models.Product> theList = (new GeospaceEntity.Models.Product()).GetAll();

                if (theList.Count == 0)
                {
                    theProduct = new GeospaceEntity.Models.Product();
                    theProduct.Save();
                }
                else
                {
                    theProduct = theList[0];
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
            }
            return View(theProduct);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateInput(false)]
        public ActionResult EditTableSun(FormCollection collection)
        {
            ViewBag.IsLocal = Utils.Util.IsLocal();
            GeospaceEntity.Models.Product theProduct = null;
            try
            {
                List<GeospaceEntity.Models.Product> theList = (new GeospaceEntity.Models.Product()).GetAll();

                if (theList.Count == 0)
                {
                    theProduct = new GeospaceEntity.Models.Product();
                    theProduct.Save();
                }
                else
                {
                    theProduct = theList[0];
                }
                theProduct.table_sun = collection.Get("table_sun");

                theProduct.Update();
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
            }
            return RedirectToAction("Index");
        }
    }
}
