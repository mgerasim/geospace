using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GeospaceMediana.Models
{
    [JsonObject]
    public class ApiUmagf
    {

        public ApiUmagf(GeospaceEntity.Models.Codes.CodeUmagf codeUmagf)
        {
            ak = codeUmagf.ak;
            k1 = codeUmagf.k1;
            k2 = codeUmagf.k2;
            k3 = codeUmagf.k3;
            k4 = codeUmagf.k4;
            k5 = codeUmagf.k5;
            k6 = codeUmagf.k6;
            k7 = codeUmagf.k7;
            k8 = codeUmagf.k8;

            MI = codeUmagf.MI;
            MM = codeUmagf.MM;
            StationCode = codeUmagf.Station.Code;
            YYYY = codeUmagf.YYYY;
            DD = codeUmagf.DD;
            HH = codeUmagf.HH;
             
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

        [JsonProperty("MI")]
        public int MI { get; set; }

        [JsonProperty("ak")]
        public int ak { get; set; }

        [JsonProperty("k1")]
        public int k1 { get; set; }

        [JsonProperty("k2")]
        public int k2 { get; set; }

        [JsonProperty("k3")]
        public int k3 { get; set; }

        [JsonProperty("k4")]
        public int k4 { get; set; }

        [JsonProperty("k5")]
        public int k5 { get; set; }

        [JsonProperty("k6")]
        public int k6 { get; set; }

        [JsonProperty("k7")]
        public int k7 { get; set; }

        [JsonProperty("k8")]
        public int k8 { get; set; }
    }
}