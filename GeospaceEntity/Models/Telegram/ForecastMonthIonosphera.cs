using GeospaceEntity.Common;
using GeospaceEntity.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeospaceEntity.Models.Telegram
{
    public class ForecastMonthIonosphera
    {
        public virtual int ID { get; set; }
        public virtual DateTime created_at { get; set; }
        public virtual DateTime updated_at { get; set; }
        public virtual Station Station { get; set; }
        public virtual int MM { get; set; }
        public virtual int YYYY { get; set; }
        public virtual string IONFO { get; set; }
        public virtual string IONES { get; set; }
        public virtual string IONDP { get; set; }
        public virtual string IONFF { get; set; }
        public virtual string MAGPO { get; set; }
        public virtual int iFORECAST { get; set; }
        public virtual int mFORECAST { get; set; }

        public ForecastMonthIonosphera()
        {
            ID = -1;
            IONFO = "";
            IONES = "";
            IONDP = "";
            IONFF = "";
            MAGPO = "";
            iFORECAST = -1;
            mFORECAST = -1;
            created_at = DateTime.Now;
            updated_at = DateTime.Now;
        }
        public virtual void Save()
        {
            this.created_at = DateTime.Now;
            this.updated_at = DateTime.Now;
            GeospaceEntity.Common.IRepository<ForecastMonthIonosphera> repo = new Repositories.ForecastMonthIonospheraRepository();
            repo.Save(this);
        }

        public virtual void Update()
        {
            this.updated_at = DateTime.Now;
            GeospaceEntity.Common.IRepository<ForecastMonthIonosphera> repo = new Repositories.ForecastMonthIonospheraRepository();
            repo.Update(this);
        }
        public static Telegram.ForecastMonthIonosphera GetByDateUTC(Station station, int YYYY, int MM)
        {
            Repositories.ForecastMonthIonospheraRepository repo = new Repositories.ForecastMonthIonospheraRepository();
            return repo.GetByDateUTC(station, YYYY, MM);
        }

        public static IList<Telegram.ForecastMonthIonosphera> GetAll()
        {
            GeospaceEntity.Common.IRepository<ForecastMonthIonosphera> repo = new Repositories.ForecastMonthIonospheraRepository();
            return repo.GetAll();
        }
        public static IList<Telegram.ForecastMonthIonosphera> GetAllByDateUTC(int YYYY, int MM)
        {
            Repositories.ForecastMonthIonospheraRepository repo = new Repositories.ForecastMonthIonospheraRepository();
            return repo.GetAllByDateUTC(YYYY, MM);
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
                case "IONDP":
                    IONDP = value;
                    break;
                case "IONFF":
                    IONFF = value;
                    break;
                case "MAGPO":
                    MAGPO = value;
                    break;
                case "iFORECAST":
                    iFORECAST = Convert.ToInt32(value);
                    break;
                case "mFORECAST":
                    iFORECAST = Convert.ToInt32(value);
                    break;
            }
        }

    }
}
