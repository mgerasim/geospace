using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GeospaceMediana.Models
{
    [JsonObject]
    public class ApiIonka
    {
        public ApiIonka(GeospaceEntity.Models.Codes.CodeIonka theObj)
        {
            this.StationCode = theObj.Station.Code;
            this.YYYY = theObj.YYYY;
            this.MM = theObj.MM;
            this.DD = theObj.DD;
            this.HH = theObj.HH;
            this.MI = theObj.MI;
            this.f0F2 = theObj.f0F2;

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

        [JsonProperty("f0F2")]
        public int f0F2 { get; set; }
    }
}