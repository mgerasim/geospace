using GeospaceEntity.Common;
using GeospaceEntity.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GeospaceEntity.Models
{
    public class Disturbance
    {
        public virtual int ID { get; set; }
        public virtual DateTime created_at {get; set;}
        public virtual DateTime updated_at {get; set;}

        public virtual Station Station { get; set; }
        public virtual int YYYY { get; set; }
        public virtual int MM { get; set; }
        public virtual int DD { get; set; }
        public virtual int HH { get; set; }
        public virtual int MI { get; set; }


        public Disturbance()
        {
            this.ID = -1;
        }

        public virtual void Save()
        {
            IRepository<Disturbance> repo = new DisturbanceRepository();
            this.created_at = DateTime.Now;
            this.updated_at = DateTime.Now;
            repo.Save(this);
        }

        public virtual void Update()
        {
            IRepository<Disturbance> repo = new DisturbanceRepository();
            this.updated_at = DateTime.Now;
            repo.Update(this);
        }

        public virtual List<Disturbance> GetAll()
        {
            IRepository<Disturbance> repo = new DisturbanceRepository();
            repo.GetAll();
            return (List<Disturbance>)repo.GetAll();
        }
        static public Disturbance GetByDate(Station station, int YYYY, int MM, int DD, int HH, int MI = 0)
        {
            DisturbanceRepository repo = new DisturbanceRepository();
            return repo.GetByDate(station, YYYY, MM, DD, HH, MI);
        }

        static public List<Disturbance> GetByMonth(Station station, int YYYY, int MM)
        {
            DisturbanceRepository repo = new DisturbanceRepository();
            return (List<Disturbance>)repo.GetByMonth(station, YYYY, MM);
        }
    }
}
