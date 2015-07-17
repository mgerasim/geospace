using GeospaceEntity.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
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
            disturbance_radio = product.disturbance_radio ;
            table_sun = product.table_sun ;
            subday_forecast = product.subday_forecast ;

            subday_forecast_win1251 = product.subday_forecast_win1251 ;
            forecast_month_ionosphera_win1251 = product.forecast_month_ionosphera_win1251 ;
            forecast_days_fives_win1251 = product.forecast_days_fives_win1251 ;
            review_geoenv_win1251 = product.review_geoenv_win1251 ;
            review_geoenv_month_win1251 = product.review_geoenv_month_win1251 ;
            disturbance_radio_win1251 = product.disturbance_radio_win1251 ;
            table_sun_win1251 = product.table_sun_win1251;
        }

        [JsonProperty("forecast_month_ionosphera")]
        public virtual string forecast_month_ionosphera { get; set; }

        [JsonProperty("forecast_days_fives")]
        public virtual string forecast_days_fives { get; set; }

        [JsonProperty("review_geoenv")]
        public virtual string review_geoenv { get; set; }

        [JsonProperty("review_geoenv_month")]
        public virtual string review_geoenv_month { get; set; }

        [JsonProperty("disturbance_radio")]
        public virtual string disturbance_radio { get; set; }

        [JsonProperty("table_sun")]
        public virtual string table_sun { get; set; }

        [JsonProperty("subday_forecast")]
        public virtual string subday_forecast { get; set; }


        [JsonProperty("subday_forecast_win1251")]
        public virtual string subday_forecast_win1251 { get; set; }

        [JsonProperty("forecast_month_ionosphera_win1251")]
        public virtual string forecast_month_ionosphera_win1251 { get; set; }

        [JsonProperty("forecast_days_fives_win1251")]
        public virtual string forecast_days_fives_win1251 { get; set; }

        [JsonProperty("review_geoenv_win1251")]
        public virtual string review_geoenv_win1251 { get; set; }

        [JsonProperty("review_geoenv_month_win1251")]
        public virtual string review_geoenv_month_win1251 { get; set; }

        [JsonProperty("disturbance_radio_win1251")]
        public virtual string disturbance_radio_win1251 { get; set; }

        [JsonProperty("table_sun_win1251")]
        public virtual string table_sun_win1251 { get; set; }
    }
}