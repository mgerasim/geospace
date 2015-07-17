using GeospaceEntity.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeospaceEntity.Models
{
    public class Mediana
    {
        public Mediana()
        {
            ID = -1;

            created_at = DateTime.Now;
            updated_at = DateTime.Now;
        }

        public virtual Station Station { get; set; }
        public virtual int ID { get; set; }
        public virtual DateTime created_at { get; set; }
        public virtual DateTime updated_at { get; set; }
        public virtual int HH { get; set; }
        public virtual int MM { get; set; }
        public virtual int YYYY { get; set; }
        public virtual int RangeNumber { get; set; }
        public virtual int f0F2 { get; set; }
        public virtual int M3000F2 { get; set; }

        public virtual string _f0F2 { get { if (ID < 0) return ""; return f0F2.ToString(); } }
        public virtual string _M3000F2 { get { if (ID < 0) return ""; return M3000F2.ToString(); } }

        public virtual void Save()
        {
            this.created_at = DateTime.Now;
            this.updated_at = DateTime.Now;
            GeospaceEntity.Common.IRepository<Mediana> repo = new Repositories.MedianaRepository();
            repo.Save(this);
        }

        public virtual void Update()
        {
            this.updated_at = DateTime.Now;
            GeospaceEntity.Common.IRepository<Mediana> repo = new Repositories.MedianaRepository();
            repo.Update(this);
        }

        public static IList<Mediana> GetByRangeNumber(Station station, int YYYY, int MM, int rangeNumber)
        {
            MedianaRepository medianaRepository = new MedianaRepository();
            return medianaRepository.GetByRangeNumber(station, YYYY, MM, rangeNumber);
        }

        public static IList<Mediana> GetByMonth(Station station, int YYYY, int MM)
        {
            MedianaRepository medianaRepository = new MedianaRepository();
            return medianaRepository.GetByMonth(station, YYYY, MM);
        }

        public static Mediana GetByDate(Station station, int YYYY, int MM, int HH, int rangeNumber)
        {
            MedianaRepository medianaRepository = new MedianaRepository();
            return medianaRepository.GetByDate(station, YYYY, MM, HH, rangeNumber);
        }
    }
}
