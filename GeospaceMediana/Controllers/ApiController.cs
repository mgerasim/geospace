using GeospaceEntity.Helper;
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
            DateTime currDate = DateTime.Now;
            
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
    }
}
