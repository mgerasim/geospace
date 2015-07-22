using GeospaceEntity.Models;
using GeospaceEntity.Models.Codes;
using GeospaceMediana.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace GeospaceMediana.Controllers
{
    public class ApiController : Controller
    {
        //
        // GET: /Api/

        public JsonResult GetIonka()
        {
            List<CodeIonka> theList = (List<CodeIonka>)CodeIonka.GetAll();
            List<ApiIonka> theResult = new List<ApiIonka>();
            foreach(var item in theList)
            {
                ApiIonka jsonObj = new ApiIonka(item);
                theResult.Add(jsonObj);
            }
            return Json(theResult, JsonRequestBehavior.AllowGet); 
        }

        public JsonResult GetIonkaByPeriod(int StationCode, int startYYYY, int startMM, int startDD, int endYYYY, int endMM, int endDD)
        {
            Station station = Station.GetByCode(StationCode);

            List<CodeIonka> theList = (List<CodeIonka>)CodeIonka.GetByPeriod(station, startYYYY, startMM, startDD, endYYYY, endMM, endDD);
            List<ApiIonka> theResult = new List<ApiIonka>();
            foreach (var item in theList)
            {
                ApiIonka jsonObj = new ApiIonka(item);
                theResult.Add(jsonObj);
            }
            return Json(theResult, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetUmagfByPeriod(int StationCode, int startYYYY, int startMM, int startDD, int endYYYY, int endMM, int endDD)
        {
            Station station = Station.GetByCode(StationCode);

            List<CodeUmagf> theList = (List<CodeUmagf>)CodeUmagf.GetByPeriod(station, startYYYY, startMM, startDD, endYYYY, endMM, endDD);
            List<ApiUmagf> theResult = new List<ApiUmagf>();
            foreach (var item in theList)
            {
                ApiUmagf jsonObj = new ApiUmagf(item);
                theResult.Add(jsonObj);
            }
            return Json(theResult, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetMedianaByDate(int StationCode, int YYYY, int MM)
        {
            Station station = Station.GetByCode(StationCode);

            var medians = Mediana.GetByMonth(station, YYYY, MM);

            List<ApiMediana> theResult = new List<ApiMediana>();

            for (int numberRange = 0; numberRange < 6;numberRange++ )
            {
                ApiMediana jsonObj = new ApiMediana(medians, YYYY, MM, numberRange);
                theResult.Add(jsonObj);
            }
            return Json(theResult, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetProducts()
        {
            Product product = new Product();

            ApiProduct apiProduct = new ApiProduct(product.GetAll()[0]);

            return Json(apiProduct, "application/json", Encoding.GetEncoding("windows-1251"), JsonRequestBehavior.AllowGet);
        }
    }
}
