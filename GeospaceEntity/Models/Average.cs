using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace GeospaceEntity.Models
{
    public class Average
    {
        public virtual Station Station { get; set; }
        public virtual int ID { get; set; }
        public virtual DateTime created_at { get; set; }
        public virtual DateTime updated_at { get; set; }
        public virtual int HH { get; set; }
        public virtual int MM { get; set; }
        public virtual int DD { get; set; }
        public virtual int YYYY { get; set; }
       
        public virtual double F2_05 { get; set; }
        public virtual double F2_07 { get; set; }
        public virtual double F2_10 { get; set; }
        public virtual double F2_20 { get; set; }
        public virtual double F2_27 { get; set; }
        public virtual double F2_30 { get; set; }

        public virtual int F2_05_skip { get; set; }
        public virtual int F2_07_skip { get; set; }
        public virtual int F2_10_skip { get; set; }
        public virtual int F2_20_skip { get; set; }
        public virtual int F2_27_skip { get; set; }
        public virtual int F2_30_skip { get; set; }

        public virtual double M3000_05 { get; set; }
        public virtual double M3000_07 { get; set; }
        public virtual double M3000_10 { get; set; }
        public virtual double M3000_20 { get; set; }
        public virtual double M3000_27 { get; set; }
        public virtual double M3000_30 { get; set; }

        public virtual int M3000_05_skip { get; set; }
        public virtual int M3000_07_skip { get; set; }
        public virtual int M3000_10_skip { get; set; }
        public virtual int M3000_20_skip { get; set; }
        public virtual int M3000_27_skip { get; set; }
        public virtual int M3000_30_skip { get; set; }
    


        public Average()
        {
            ID = -1;

            created_at = DateTime.Now;
            updated_at = DateTime.Now;

            F2_05 = 0;
            F2_07 = 0;
            F2_10 = 0;
            F2_20 = 0;
            F2_27 = 0;
            F2_30 = 0;

            F2_05_skip = 0;
            F2_07_skip = 0;
            F2_10_skip = 0;
            F2_20_skip = 0;
            F2_27_skip = 0;
            F2_30_skip = 0;

            M3000_05 = 0;
            M3000_07 = 0;
            M3000_10 = 0;
            M3000_20 = 0;
            M3000_27 = 0;
            M3000_30 = 0;

            M3000_05_skip = 0;
            M3000_07_skip = 0;
            M3000_10_skip = 0;
            M3000_20_skip = 0;
            M3000_27_skip = 0;
            M3000_30_skip = 0;
        }


        public static List<Average> GetByDate(Station station, int YYYY, int MM, int DD)
        {
            Repositories.AverageRepository repo = new Repositories.AverageRepository();
            return (List<Average>)repo.GetByDate(station, YYYY, MM, DD);
        }

        //сохранить новую запись в БД
        public virtual void Save()
        {
            this.created_at = DateTime.Now;
            this.updated_at = DateTime.Now;
            GeospaceEntity.Common.IRepository<Average> repo = new Repositories.AverageRepository();
            repo.Save(this);
        }

        public virtual void Update()
        {
            this.updated_at = DateTime.Now;
            GeospaceEntity.Common.IRepository<Average> repo = new Repositories.AverageRepository();
            repo.Update(this);
        }

        public static Average GetByDateUTC(Station station, int YYYY, int MM, int DD, int HH)
        {
            Repositories.AverageRepository repo = new Repositories.AverageRepository();
            return repo.GetByDateUTC(station, YYYY, MM, DD, HH);
        }
    }
}
