using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeospaceEntity.Models.Codes
{
    public class CodeUmagf
    {
        public virtual Station Station { get; set; }
        public virtual int ID { get; set; }
        public virtual DateTime created_at { get; set; }
        public virtual DateTime updated_at { get; set; }
        public virtual int HH { get; set; }
        public virtual int MM { get; set; }
        public virtual int DD { get; set; }
        public virtual int YYYY { get; set; }
        public virtual int MI { get; set; }

        //K-индексы
        public virtual int k1 { get; set; }
        public virtual int k2 { get; set; }
        public virtual int k3 { get; set; }
        public virtual int k4 { get; set; }
        public virtual int k5 { get; set; }
        public virtual int k6 { get; set; }
        public virtual int k7 { get; set; }
        public virtual int k8 { get; set; }

        //AK
        public virtual int ak { get; set; }

        public virtual string Raw { get; set; }
        public virtual string ErrorMessage { get; set; }
        public CodeUmagf()
        {
            ID = -1;
            created_at = DateTime.Now;
            updated_at = DateTime.Now;

            k1 = 1000;
            k2 = 1000;
            k3 = 1000;
            k4 = 1000;
            k5 = 1000;
            k6 = 1000;
            k7 = 1000;
            k8 = 1000;

            ak = 1000;

            Raw = "";
            ErrorMessage = "";
        }

        public CodeUmagf(string strUmagf)
        {
            this.created_at = DateTime.Now;
            this.updated_at = DateTime.Now;

            k1 = 1000;
            k2 = 1000;
            k3 = 1000;
            k4 = 1000;
            k5 = 1000;
            k6 = 1000;
            k7 = 1000;
            k8 = 1000;

            ak = 1000;

            Raw = "";
            ErrorMessage = "";
        }

        //по параметрам получаем объект из БД, если его нет значит сохраниме новую запись в БД
        public virtual Codes.CodeUmagf GetByDateUTC()
        {
            Repositories.CodeUmagfRepository repo = new Repositories.CodeUmagfRepository();
            return repo.GetByDateUTC(Station, YYYY, MM, DD, HH, MI);
        }

        //сохранить новую запись в БД
        public virtual void Save()
        {
            this.created_at = DateTime.Now;
            this.updated_at = DateTime.Now;
            GeospaceEntity.Common.IRepository<CodeUmagf> repo = new Repositories.CodeUmagfRepository();
            repo.Save(this);
        }
    }
}
