using GeospaceEntity.Models;
using GeospaceEntity.Models.Codes;
using GeospaceMediana.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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
            List<CodeIonka> theList = (List<CodeIonka>)(new CodeIonka()).GetAll();
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
            Station station = new Station();
            station = station.GetByCode(StationCode);

            List<CodeIonka> theList = (List<CodeIonka>)(new CodeIonka()).GetByPeriod(station, startYYYY, startMM, startDD, endYYYY, endMM, endDD);
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
            Station station = new Station();
            station = station.GetByCode(StationCode);

            List<CodeUmagf> theList = (List<CodeUmagf>)(new CodeUmagf()).GetByPeriod(station, startYYYY, startMM, startDD, endYYYY, endMM, endDD);
            List<ApiUmagf> theResult = new List<ApiUmagf>();
            foreach (var item in theList)
            {
                ApiUmagf jsonObj = new ApiUmagf(item);
                theResult.Add(jsonObj);
            }
            return Json(theResult, JsonRequestBehavior.AllowGet);
        }
    }
}
