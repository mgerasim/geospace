using GeospaceEntity.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GeospaceMediana.Models
{

    [JsonObject]
    public class ApiDisturbance
    {
        public ApiDisturbance()
        {
            theItems = new List<ApiDisturbanceItem>();
            this.Title = "";
        }
        [JsonObject]
        public class ApiDisturbanceItem
        {
            [JsonObject]
            public class ApiDisturbanceEntity
            {
            
                [JsonProperty("YYYY")]
                public int YYYY;
                [JsonProperty("MM")]
                public int MM;
                [JsonProperty("DD")]
                public int DD;
                [JsonProperty("Display")]
                public string Display;
                [JsonProperty("HourCount")]
                public int HourCount;
            }
            
            public ApiDisturbanceItem()
            {
                theData = new List<ApiDisturbanceEntity>();
            }
            [JsonProperty]
            public List<ApiDisturbanceEntity> theData;
            [JsonProperty]
            public int StationCode;
            [JsonProperty]
            public string StationName;
        }
        [JsonProperty("Title")]
        public string Title;
        [JsonProperty("Data")]
        public List<ApiDisturbanceItem> theItems;
    }

    
}