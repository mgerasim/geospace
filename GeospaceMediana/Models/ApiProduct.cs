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
            
        }

        private string Win1251ToUTF8(string source)
        {

            Encoding utf8 = Encoding.GetEncoding("utf-8");
            Encoding win1251 = Encoding.GetEncoding("windows-1251");

            byte[] utf8Bytes = win1251.GetBytes(source);
            byte[] win1251Bytes = Encoding.Convert(win1251, utf8, utf8Bytes);
            source = win1251.GetString(win1251Bytes);
            return source;

        }

        private string UTF8ToWin1251(string source)
        {
            if (source == null)
            {
                return "";
            }
            Encoding utf8 = Encoding.GetEncoding("utf-8");
            Encoding win1251 = Encoding.GetEncoding("windows-1251");

            byte[] win1251Bytes = utf8.GetBytes(source);
            byte[] utf8Bytes = Encoding.Convert(utf8, win1251, win1251Bytes);
            source = utf8.GetString(utf8Bytes);
            return source;
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
        


        [JsonProperty("subday_forecast_win1251")]
        public virtual string subday_forecast_win1251
        {
            get
            {
                string ss = this.subday_forecast;
                ss = this.UTF8ToWin1251(ss);
                return ss;
            }
        }
        [JsonProperty("forecast_month_ionosphera_win1251")]
        public virtual string forecast_month_ionosphera_win1251
        {
            get
            {
                string ss = this.forecast_month_ionosphera;
                ss = this.UTF8ToWin1251(ss);
                return ss;
            }
        }

        [JsonProperty("forecast_days_fives_win1251")]
        public virtual string forecast_days_fives_win1251
        {
            get
            {
                string ss = this.forecast_days_fives;
                ss = this.UTF8ToWin1251(ss);
                return ss;
            }
        }

        [JsonProperty("review_geoenv_win1251")]
        public virtual string review_geoenv_win1251 
        {
            get
            {
                string ss = this.review_geoenv;
                ss = this.UTF8ToWin1251(ss);
                return ss;
            }
        }

        [JsonProperty("review_geoenv_month_win1251")]
        public virtual string review_geoenv_month_win1251
        {
            get
            {
                string ss = this.review_geoenv_month;
                ss = this.UTF8ToWin1251(ss);
                return ss;
            }
        }

    }
}