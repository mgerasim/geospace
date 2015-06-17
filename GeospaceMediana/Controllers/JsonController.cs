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
    public class JsonController : Controller
    {
        //
        // GET: /Api/

        public JsonResult GetIonka()
        {
            List<CodeIonka> theList = (List<CodeIonka>)(new CodeIonka()).GetAll();
            List<JsonIonka> theResult = new List<JsonIonka>();
            foreach(var item in theList)
            {
                JsonIonka jsonObj = new JsonIonka(item);
                theResult.Add(jsonObj);
            }
            return Json(theResult, JsonRequestBehavior.AllowGet); 
        }

        public JsonResult GetIonkaByPeriod(int StationCode, int startYYYY, int startMM, int startDD, int endYYYY, int endMM, int endDD)
        {
            Station station = new Station();
            station = station.GetByCode(StationCode);

            List<CodeIonka> theList = (List<CodeIonka>)(new CodeIonka()).GetByPeriod(station, startYYYY, startMM, startDD, endYYYY, endMM, endDD);
            List<JsonIonka> theResult = new List<JsonIonka>();
            foreach (var item in theList)
            {
                JsonIonka jsonObj = new JsonIonka(item);
                theResult.Add(jsonObj);
            }
            return Json(theResult, JsonRequestBehavior.AllowGet);
        }
    }
}
