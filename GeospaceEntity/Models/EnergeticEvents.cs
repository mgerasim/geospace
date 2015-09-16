using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GeospaceEntity.Repositories;
using GeospaceEntity.Common;

namespace GeospaceEntity.Models
{
    public class EnergeticEvent
    {
        public virtual int ID { get; set; }
        public virtual DateTime created_at { get; set; }
        public virtual DateTime updated_at { get; set; }
        public virtual string Balls { get; set; }
        public virtual string Coordinates { get; set; }
        public virtual string Time { get; set; }
        public virtual string RadioBursts { get; set; }

        public EnergeticEvent()
        {
            ID = -1;
            this.created_at = DateTime.Now;
            this.updated_at = DateTime.Now;
            Balls = "";
            Coordinates = "";
            Time = "";
            RadioBursts = "";
        }

        public static EnergeticEvent GetById(int id)
        {
            IRepository<EnergeticEvent> repo = new Repositories.EnergeticEventRepository();
            return repo.GetById(id);
        }


        public virtual void Save()
        {
            IRepository<EnergeticEvent> repo = new EnergeticEventRepository();
            this.created_at = DateTime.Now;
            this.updated_at = DateTime.Now;
            repo.Save(this);
        }

        public virtual void Update()
        {
            IRepository<EnergeticEvent> repo = new EnergeticEventRepository();
            this.updated_at = DateTime.Now;
            repo.Update(this);
        }

        public static List<EnergeticEvent> GetAll()
        {
            IRepository<EnergeticEvent> repo = new EnergeticEventRepository();
            return (List<EnergeticEvent>)(repo.GetAll());
        }

        public virtual void Delete()
        {
            IRepository<EnergeticEvent> repo = new EnergeticEventRepository();

            repo.Delete(this);
        }
    }

}
