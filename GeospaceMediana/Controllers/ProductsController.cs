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

        public ActionResult Index()
        {
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
            GeospaceEntity.Models.Product theProduct = (new Product()).GetAll()[0];
            var fileName = theProduct.table_sun;
            var path = Path.Combine(Server.MapPath("~/App_Data"), fileName);

            byte[] fileBytes = System.IO.File.ReadAllBytes(path);
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }

        public FileResult DownloadDisturbanceRadio()
        {
            GeospaceEntity.Models.Product theProduct = (new Product()).GetAll()[0];
            var fileName = theProduct.disturbance_radio;
            var path = Path.Combine(Server.MapPath("~/App_Data"), fileName);

            byte[] fileBytes = System.IO.File.ReadAllBytes(path);
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }

        public ActionResult EditForecastDaysFives()
        {
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
        public ActionResult EditForecastDaysFives(FormCollection collection)
        {
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
        public ActionResult EditForecastMonthIonosphera(FormCollection collection)
        {
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
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
            }

            return RedirectToAction("Index");
        }



        public ActionResult EditReviewGeoEnv()
        {
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
        public ActionResult EditReviewGeoEnv(FormCollection collection)
        {
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
        public ActionResult EditReviewGeoEnvMonth(FormCollection collection)
        {
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
        public ActionResult EditSubdayForecast(FormCollection collection)
        {
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
                theProduct.Update();
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
            }

            return RedirectToAction("Index");
        }


        public ActionResult EditDescription()
        {
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
        public ActionResult EditDescription(FormCollection collection)
        {
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
    }
}
