using GeospaceEntity.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace GeospaceMediana.Models
{
    [JsonObject]
    public class ApiProduct
    {
        public ApiProduct(Product product)
        {
            forecast_month_ionosphera = product.forecast_month_ionosphera ;
            forecast_days_fives = product.forecast_days_fives ;
            review_geoenv = product.review_geoenv ;    
            review_geoenv_month = product.review_geoenv_month ;
            subday_forecast = product.subday_forecast;
            description = product.description;
        }

       

        [JsonProperty("forecast_month_ionosphera")]
        public virtual string forecast_month_ionosphera { get; set; }

        [JsonProperty("forecast_days_fives")]
        public virtual string forecast_days_fives { get; set; }

        [JsonProperty("review_geoenv")]
        public virtual string review_geoenv { get; set; }

        [JsonProperty("review_geoenv_month")]
        public virtual string review_geoenv_month { get; set; }

        [JsonProperty("subday_forecast")]
        public virtual string subday_forecast { get; set; }

        [JsonProperty("description")]
        public virtual string description { get; set; }     


    }
}