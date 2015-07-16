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

            this.M3000F2 = theObj.M3000F2;
            this.f0F1 = theObj.f0F1;
            this.M3000F1 = theObj.M3000F1;
            this.f0Es = theObj.f0Es;
            this.Diffusio = theObj.Diffusio;
            this.fmin = theObj.fmin;

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

        [JsonProperty("M3000F2")]
        public int M3000F2 { get; set; }

        [JsonProperty("f0F1")]
        public int f0F1 { get; set; }

        [JsonProperty("M3000F1")]
        public int M3000F1 { get; set; }

        [JsonProperty("f0Es")]
        public int f0Es { get; set; }

        [JsonProperty("Diffusio")]
        public int Diffusio { get; set; }

        [JsonProperty("fmin")]
        public int fmin { get; set; }
    }
}