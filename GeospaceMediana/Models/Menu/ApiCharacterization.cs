using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GeospaceMediana.Models
{
    [JsonObject("api_characterization")]
    public class ApiCharacterization
    {

        [JsonObject]
        public class ApiCharacterizationDay
        {
            [JsonObject]
            public class ApiCharacterizationData
            {
                public ApiCharacterizationData()
                {
                    this.delta = "";
                    this.value = "";
                    this.rating = "";
                }
                [JsonProperty("day")]
                public int day;

                [JsonProperty("hour")]
                public int hour;
                
                [JsonProperty("value")]
                public string value;
                [JsonProperty("delta")]
                public string delta;
                [JsonProperty("rating")]
                public string rating;
            }

            public ApiCharacterizationDay()
            {
                theApiCharacterizationData = new List<ApiCharacterizationData>();
            }
        
            [JsonProperty("YYYY")]
            public int YYYY;
            [JsonProperty("MM")]
            public int MM;
            [JsonProperty("DD")]
            public int DD;
            [JsonProperty("day_rating_subfirst")]
            public string day_rating_subfirst;
            [JsonProperty("day_rating_subsecond")]
            public string day_rating_subsecond;
            [JsonProperty("day_rating")]
            public string day_rating;
            [JsonProperty("api_characterization_data")]
            public List<ApiCharacterizationData> theApiCharacterizationData;

        }

        [JsonProperty("api_characterization_day")]
        public List<ApiCharacterizationDay> theApiCharacterizationDay;

        public ApiCharacterization()
        {
            theApiCharacterizationDay = new List<ApiCharacterizationDay>();
        }

        
    }
}