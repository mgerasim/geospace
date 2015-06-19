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
    }
}
