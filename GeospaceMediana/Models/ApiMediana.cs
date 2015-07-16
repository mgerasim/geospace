using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GeospaceMediana.Models
{
    [JsonObject]
    public class ApiMediana
    {
        public ApiMediana(Medians.RangeDays rangeDays)
        {
            Values = new List<int>();

            Range = rangeDays.Header;

            for(int hour=0; hour<24; hour++)
            {
                Values.Add(rangeDays.Values[hour]);
            }
        }

        [JsonProperty("Range")]
        public string Range { get; set; }

        [JsonProperty("Values")]
        public List<int> Values { get; set; }
    }
}