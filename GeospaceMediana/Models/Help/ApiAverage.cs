using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GeospaceMediana.Models
{
    [JsonObject]
    public class ApiAverage
    {
        public ApiAverage(GeospaceEntity.Models.Average theObj)
        {
            this.StationCode = theObj.Station.Code;
            this.YYYY = theObj.YYYY;
            this.MM = theObj.MM;
            this.DD = theObj.DD;
            this.HH = theObj.HH;

            this.F2_05 = (int)theObj.F2_05;
            this.F2_07 = (int)theObj.F2_07;
            this.F2_10 = (int)theObj.F2_10;
            this.F2_20 = (int)theObj.F2_20;
            this.F2_27 = (int)theObj.F2_27;
            this.F2_30 = (int)theObj.F2_30;

            this.F2_05_skip = (int)theObj.F2_05_skip;
            this.F2_07_skip = (int)theObj.F2_07_skip;
            this.F2_10_skip = (int)theObj.F2_10_skip;
            this.F2_20_skip = (int)theObj.F2_20_skip;
            this.F2_27_skip = (int)theObj.F2_27_skip;
            this.F2_30_skip = (int)theObj.F2_30_skip;

            this.M3000_05 = (int)theObj.M3000_05;
            this.M3000_07 = (int)theObj.M3000_07;
            this.M3000_10 = (int)theObj.M3000_10;
            this.M3000_20 = (int)theObj.M3000_20;
            this.M3000_27 = (int)theObj.M3000_27;
            this.M3000_30 = (int)theObj.M3000_30;

            this.M3000_05_skip = (int)theObj.M3000_05_skip;
            this.M3000_07_skip = (int)theObj.M3000_07_skip;
            this.M3000_10_skip = (int)theObj.M3000_10_skip;
            this.M3000_20_skip = (int)theObj.M3000_20_skip;
            this.M3000_27_skip = (int)theObj.M3000_27_skip;
            this.M3000_30_skip = (int)theObj.M3000_30_skip;


        }
        [JsonProperty("StationCode")]
        public int StationCode { get; set; }
        [JsonProperty("YYYY")]
        public int YYYY { get; set; }

        [JsonProperty("MM")]
        public int MM { get; set; }

        [JsonProperty("DD")]
        public int DD { get; set; }

        [JsonProperty("HH")]
        public int HH { get; set; }

        [JsonProperty("F2_05")]
        public int F2_05 { get; set; }

        [JsonProperty("F2_07")]
        public int F2_07 { get; set; }

        [JsonProperty("F2_10")]
        public int F2_10 { get; set; }

        [JsonProperty("F2_20")]
        public int F2_20 { get; set; }

        [JsonProperty("F2_27")]
        public int F2_27 { get; set; }

        [JsonProperty("F2_30")]
        public int F2_30 { get; set; }


        [JsonProperty("F2_05_skip")]
        public int F2_05_skip { get; set; }

        [JsonProperty("F2_07_skip")]
        public int F2_07_skip { get; set; }

        [JsonProperty("F2_10_skip")]
        public int F2_10_skip { get; set; }

        [JsonProperty("F2_20_skip")]
        public int F2_20_skip { get; set; }

        [JsonProperty("F2_27_skip")]
        public int F2_27_skip { get; set; }

        [JsonProperty("F2_30_skip")]
        public int F2_30_skip { get; set; }


        [JsonProperty("M3000_05")]
        public int M3000_05 { get; set; }

        [JsonProperty("M3000_07")]
        public int M3000_07 { get; set; }

        [JsonProperty("M3000_10")]
        public int M3000_10 { get; set; }

        [JsonProperty("M3000_20")]
        public int M3000_20 { get; set; }

        [JsonProperty("M3000_27")]
        public int M3000_27 { get; set; }

        [JsonProperty("M3000_30")]
        public int M3000_30 { get; set; }


        [JsonProperty("M3000_05_skip")]
        public int M3000_05_skip { get; set; }

        [JsonProperty("M3000_07_skip")]
        public int M3000_07_skip { get; set; }

        [JsonProperty("M3000_10_skip")]
        public int M3000_10_skip { get; set; }

        [JsonProperty("M3000_20_skip")]
        public int M3000_20_skip { get; set; }

        [JsonProperty("M3000_27_skip")]
        public int M3000_27_skip { get; set; }

        [JsonProperty("M3000_30_skip")]
        public int M3000_30_skip { get; set; }

        
    }
}