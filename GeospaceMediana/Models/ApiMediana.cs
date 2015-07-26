using GeospaceEntity.Models;
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
        public ApiMediana(IList<Mediana> medians, int year, int month, int numberRange)
        {
            Values = new List<int>();

            var range = MedianaCalculator.GetRangeFromNumber(new DateTime(year, month, 1), numberRange);

            Range = range.Header;

            for(int hour=0; hour<24; hour++)
            {
                Mediana mediana = medians.FirstOrDefault(x => x.HH == hour && x.RangeNumber == numberRange);

                if (mediana == null)
                    mediana = new Mediana();

                Values.Add(mediana.f0F2);
            }
        }

        [JsonProperty("Range")]
        public string Range { get; set; }

        [JsonProperty("Values")]
        public List<int> Values { get; set; }
    }
}