using GeospaceEntity.Common;
using GeospaceEntity.Helper;
using GeospaceEntity.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeospaceEntity.Models
{
    public class Product
    {
        public virtual int ID { get; set; }
        public virtual DateTime created_at {get; set;}
        public virtual DateTime updated_at {get; set;}
        public virtual string forecast_month_ionosphera {get; set;}
        public virtual string forecast_days_fives {get; set;}
        public virtual string review_geoenv {get; set;}    
        public virtual string review_geoenv_month {get; set;}
        public virtual string disturbance_radio { get; set; }
        public virtual string table_sun { get; set; }
        public virtual string subday_forecast { get; set; }
        public virtual string description { get; set; }

        public Product()
        {

        }

        public virtual void Save()
        {
            IRepository<Product> repo = new ProductRepository();
            this.created_at = DateTime.Now;
            this.updated_at = DateTime.Now;
            repo.Save(this);
        }

        public virtual void Update()
        {
            IRepository<Product> repo = new ProductRepository();
            this.updated_at = DateTime.Now;
            repo.Update(this);
        }

        public virtual List<Product> GetAll()
        {
            IRepository<Product> repo = new ProductRepository();
            repo.GetAll();
            return (List<Product>)repo.GetAll();
        }

        public virtual void Send_SubdayForecast() 
        {
            string telegram = "ЗЦЗЦ 025 040001/=Н288\nЗИРА40 ХБРВ";
            DateTime TimeNow = DateTimeKhabarovsk.Now;
            DateTime Time14 = new DateTime(TimeNow.Year, TimeNow.Month,TimeNow.Day,14,0,0);

            string time = "";
            if (Time14 > TimeNow)
            {
                time = "0000";
            }
            else
            {
                time = "0900";
            }
            telegram += " " + TimeNow.ToString("dd") + time + "\n"; 
            Other.SendToAspd("Полусуточный прогноз", telegram + this.subday_forecast + "=\nНННН");
        }

        public virtual void Send_MonthForecast()
        {
            DateTime TimeNow = DateTimeKhabarovsk.Now;
            string telegram = "ЗЦЗЦ 010 5200/=Н235\nААЩБЛГ ХБРВ " + TimeNow.ToString("ddHHmm") + "\n" +
                "ХАБАРОВСКА " + TimeNow.ToString("dd/MM HHmm=") + "\n01 МОСКВА ИПГ ДЕНИСОВОЙ=\nМЕСЯЧНАЯ СПРАВКА\n";
            Other.SendToAspd("Месячный прогноз", telegram + this.forecast_month_ionosphera + "=\nНННН");
        }

        public virtual void Send_FivedaysForecast()
        {
            DateTime TimeNow = DateTime.Now;
            string telegram = "ЗЦЗЦ 025 040001/=Н288\nЗИРА40 ХБРВ " + TimeNow.ToString("ddHHmm") + "\n";
            Other.SendToAspd("Пятисуточный прогноз", telegram + this.forecast_days_fives + "=\nНННН");
        }
    }
}
