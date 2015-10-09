using GeospaceEntity.Helper;
using GeospaceEntity.Models;
using GeospaceEntity.Models.Codes;
using GeospaceMediana.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;

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

        public JsonResult GetAverageByUTC(Station station, int YYYY, int MM, int DD, int HH)
        {
            ApiAverage theResult = new ApiAverage(Average.GetByDateUTC(station, YYYY, MM, DD, HH));
            return Json( theResult, JsonRequestBehavior.AllowGet);
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

        public JsonResult GetCharacterizations(int stationCode = 43501, string type = "f0F2")
        {
            DateTime currDate = DateTime.Now.AddDays(-3);
            
            int rangeNumber = -1;
            if (rangeNumber == -1)
            {
                for (int i = 0; i < 6; i++)
                {
                    var range = MedianaCalculator.GetRangeFromNumber(DateTime.Now, i);

                    if (currDate.Day >= range.Min && currDate.Day <= range.Max)
                    {
                        rangeNumber = i;
                        break;
                    }
                }
            }

            Station station = Station.GetByCode(stationCode);
            CharacterizationDay theEntityCharacterization = new CharacterizationDay(station, 
                rangeNumber, 
                currDate.Year, 
                currDate.Month, 
                type);


            ApiCharacterization theApiCharacterization = new ApiCharacterization();

            var range1 = MedianaCalculator.GetRangeFromNumber(currDate, rangeNumber);
            for (int day = range1.Min; day <= range1.Max; day++)
            {
                ApiCharacterization.ApiCharacterizationDay theApiDay = new ApiCharacterization.ApiCharacterizationDay();
                theApiDay.YYYY = currDate.Year;
                theApiDay.MM = currDate.Month;
                theApiDay.DD = day;

                theApiDay.day_rating_subfirst = theEntityCharacterization.GetFirstHalfDayValue(day).ToString();
                theApiDay.day_rating_subsecond = theEntityCharacterization.GetSecondHalfDayValue(day).ToString();
                theApiDay.day_rating = theEntityCharacterization.GetFullDayValue(day).ToString();

                theApiCharacterization.theApiCharacterizationDay.Add(theApiDay);
                for (int hour = 0; hour < 24; hour++)
                {
                    ApiCharacterization.ApiCharacterizationDay.ApiCharacterizationData data 
                    = new ApiCharacterization.ApiCharacterizationDay.ApiCharacterizationData();

                    data.day = day;
                    data.hour = hour;

                    var result_list = theEntityCharacterization.GetValues().Where(x => x.Day == day).Where(x => x.Hour == hour);
                    
                    if (result_list.Count() > 0)
                    {
                        var result = result_list.Single();
                        data.value = result._f0;
                        data.delta = result._PrevRating;
                        data.rating = result._Rating;                        
                    }

                    theApiDay.theApiCharacterizationData.Add(data);

    
                }
                
            }
            return Json(theApiCharacterization, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDisturbances(int YYYY = -1, int MM = -1)
        {
            ApiDisturbance theApi = new ApiDisturbance();
            if (YYYY == -1)
            {
                YYYY = DateTime.Now.Year;
            }
            if (MM == -1)
            {
                MM = DateTime.Now.Month;
            }

            ViewDisturbanceList theViewData = new ViewDisturbanceList(YYYY, MM);
            List<ViewDisturbance> theDisturbanceList = new List<ViewDisturbance>();
            Station stationKhabarovsk = Station.GetByCode(43501);
            theViewData.theStationList.Add(stationKhabarovsk);
            foreach (var item in Disturbance.GetByMonth(stationKhabarovsk, YYYY, MM))
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


            Station stationSalekhard = Station.GetByCode(37701);
            theViewData.theStationList.Add(stationSalekhard);
            foreach (var item in Disturbance.GetByMonth(stationSalekhard, YYYY, MM))
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

            theApi.theItems.Add(this.HelperDisturbance_Item(stationKhabarovsk, theViewData, YYYY, MM));
            theApi.theItems.Add(this.HelperDisturbance_Item(stationMagadan, theViewData, YYYY, MM));
            theApi.theItems.Add(this.HelperDisturbance_Item(stationParatunka, theViewData, YYYY, MM));
            theApi.theItems.Add(this.HelperDisturbance_Item(stationSalekhard, theViewData, YYYY, MM));
            

            theApi.Title = theViewData.Title;

            return Json(theApi, JsonRequestBehavior.AllowGet);
        }

        protected ApiDisturbance.ApiDisturbanceItem HelperDisturbance_Item(Station station, ViewDisturbanceList theViewData, int YYYY, int MM)
        {
            ApiDisturbance.ApiDisturbanceItem theItem = new ApiDisturbance.ApiDisturbanceItem();

            List<ApiDisturbance.ApiDisturbanceItem.ApiDisturbanceEntity> theEntityList = new List<ApiDisturbance.ApiDisturbanceItem.ApiDisturbanceEntity>();
            for (int i = 1; i <= DateTime.DaysInMonth(YYYY, MM); i++)
            {
                ApiDisturbance.ApiDisturbanceItem.ApiDisturbanceEntity theEntity = new ApiDisturbance.ApiDisturbanceItem.ApiDisturbanceEntity();
                theEntity.YYYY = YYYY;
                theEntity.MM = MM;
                theEntity.DD = i;
                theEntity.Display = theViewData.Display(station.Code, YYYY, MM, i);
                theEntity.HourCount = theViewData.GetCountHH(station.Code, YYYY, MM, i);
                theEntityList.Add(theEntity);
            }
            theItem.theData = theEntityList;
            theItem.StationCode = station.Code;
            theItem.StationName = station.Name;

            return theItem;
        }
        
        
        public JsonResult GetConsolidatedTable(int YYYY = -1, int MM = -1)
        {
           // GeospaceMediana.Controllers.ConsolidatedTableController
            ApiConsolidatedTable theApi = new ApiConsolidatedTable();
            if (YYYY == -1)
            {
                YYYY = DateTime.Now.Year;
            }
            if (MM == -1)
            {
                MM = DateTime.Now.Month;
            }

            IList<GeospaceEntity.Models.ConsolidatedTable> listTable = ConsolidatedTable.GetByDateMM(YYYY, MM);
            theApi.YYYY = YYYY;
            theApi.MM = MM;
            foreach (ConsolidatedTable item in listTable)
            {
                theApi.theData.Add(new ApiConsolidatedTable.ApiConsolidatedTableItem(item));
            }

            return Json(theApi, "application/json", Encoding.GetEncoding("windows-1251"), JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetConsolidatedTableView(int YYYY = -1, int MM = -1)
        {
            // GeospaceMediana.Controllers.ConsolidatedTableController
            ApiConsolidatedTable theApi = new ApiConsolidatedTable();
            if (YYYY == -1)
            {
                YYYY = DateTime.Now.Year;
            }
            if (MM == -1)
            {
                MM = DateTime.Now.Month;
            }
            var url = this.Url.Action("Index", "ConsolidatedTable", new { YYYY = YYYY , MM = MM, api = 1 }, Request.Url.Scheme);
            WebRequest request = WebRequest.Create(url);
            Stream stream = request.GetResponse().GetResponseStream();
            StreamReader streamReader = new StreamReader(stream);
            string htmlCode = streamReader.ReadToEnd();
            return Content(htmlCode, "text/html", Encoding.GetEncoding("windows-1251"));
            //return Json(htmlCode, "application/json", Encoding.GetEncoding("windows-1251"), JsonRequestBehavior.AllowGet);
        }


    }
}
