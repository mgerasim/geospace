using GeospaceEntity.Common;
using GeospaceEntity.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeospaceEntity.Models.Telegram
{
    public class ForecastFiveDay
    {
        public virtual int ID { get; set; }
        public virtual DateTime created_at { get; set; }
        public virtual DateTime updated_at { get; set; }
        public virtual Station Station { get; set; }
        public virtual int MM { get; set; }
        public virtual int YYYY { get; set; }
        public virtual int RangeNumber { get; set; }
        public virtual string IONFO { get; set; }
        public virtual string IONES { get; set; }
        public virtual string MAGPO { get; set; }

        public ForecastFiveDay()
        {
            ID = -1;
            IONFO = "";
            IONES = "";
            MAGPO = "";
            created_at = DateTime.Now;
            updated_at = DateTime.Now;
        }
        public virtual void Save()
        {
            this.created_at = DateTime.Now;
            this.updated_at = DateTime.Now;
            GeospaceEntity.Common.IRepository<ForecastFiveDay> repo = new Repositories.ForecastFiveDayRepository();
            repo.Save(this);
        }

        public virtual void Update()
        {
            this.updated_at = DateTime.Now;
            GeospaceEntity.Common.IRepository<ForecastFiveDay> repo = new Repositories.ForecastFiveDayRepository();
            repo.Update(this);
        }
        public static Telegram.ForecastFiveDay GetByDateUTC(Station station, int YYYY, int MM, int RangeNumber)
        {
            Repositories.ForecastFiveDayRepository repo = new Repositories.ForecastFiveDayRepository();
            return repo.GetByDateUTC(station, YYYY, MM, RangeNumber);
        }

        public static IList<Telegram.ForecastFiveDay> GetAll()
        {
            GeospaceEntity.Common.IRepository<ForecastFiveDay> repo = new Repositories.ForecastFiveDayRepository();
            return repo.GetAll();
        }
        public virtual string addEmpty( int number)
        {
            var str = "";
            for(var i = 0; i < number; i++)
            {
                str += "/";
            }
            return str;
        }
        public virtual string setReScanValue(string value)
        {
            string newStr = "";
            if(value.Length > 5 )
            {
                newStr = value.Substring(0, 5) + "  " + value.Substring(5, value.Length - 5 ) + addEmpty(10 - value.Length);
            }
            else {
                if (value.Length < 5) {
                    newStr = value.Substring(0, value.Length) + addEmpty(5 - value.Length);
                }
                else
                    newStr = value;
            }

            return newStr;
        }

        public virtual void SetValueByType(string type, string value)
        {
            switch (type)
            {
                case "IONFO":
                    IONFO = value;
                    break;
                case "IONES":
                    IONES = value;
                    break;
                case "MAGPO" :
                    MAGPO = value;
                    break;
            }
        }

    }
}
