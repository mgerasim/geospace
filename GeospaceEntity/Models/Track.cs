using GeospaceEntity.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeospaceEntity.Repositories;

namespace GeospaceEntity.Models
{
    public class Track
    {
        public virtual int ID { get; set; }
        public virtual DateTime created_at { get; set; }
        public virtual DateTime updated_at { get; set; }
        public virtual string Name { get; set; }   
        public virtual Post PointA { get; set; }
        public virtual Post PointB { get; set; }

        public virtual int lon1 { get; set; }
        public virtual int lat1 { get; set; }
        public virtual int lon2 { get; set; }
        public virtual int lat2 { get; set; }
        public virtual int lon3 { get; set; }
        public virtual int lat3 { get; set; }
        public virtual int lon4 { get; set; }
        public virtual int lat4 { get; set; }
        public virtual int lon5 { get; set; }
        public virtual int lat5 { get; set; }
        public virtual int lon6 { get; set; }
        public virtual int lat6 { get; set; }

        private ICollection<Consumer> _Consumers;
        public Track()
        {
            ID = -1;
            created_at = DateTime.Now;
            updated_at = DateTime.Now;

            lon1 = -1;
            lat1 = -1;
            lon2 = -1;
            lat2 = -1;
            lon3 = -1;
            lat3 = -1;
            lon4 = -1;
            lat4 = -1;
            lon5 = -1;
            lat5 = -1;
            lon6 = -1;
            lat6 = -1;

            Name = "";
            this._Consumers = new System.Collections.Generic.HashSet<Consumer>();
        }

        public virtual ICollection<Consumer> Consumers
        {
            get
            {
                return this._Consumers;
            }
            set
            {
                this._Consumers = value;
            }
        }
        public virtual void ClearConsumers()
        {
            this.Consumers.Clear();
        }

        public virtual Boolean IsExistConsumer(int ConsumerID)
        {
            foreach (var theConsumer in this.Consumers)
            {
                if (theConsumer.ID == ConsumerID)
                {
                    return true;
                }
            }
            return false;
        }  

       
        public static GeospaceEntity.Models.Track GetById(int id)
        {
            IRepository<Track> repo = new Repositories.TrackRepository();
            return repo.GetById(id);
        }


        public virtual void Save()
        {
            IRepository<Track> repo = new TrackRepository();
            this.created_at = DateTime.Now;
            this.updated_at = DateTime.Now;
            repo.Save(this);
        }

        public virtual void Update()
        {
            IRepository<Track> repo = new TrackRepository();
            this.updated_at = DateTime.Now;
            repo.Update(this);
        }

        public static List<Track> GetAll()
        {
            IRepository<Track> repo = new TrackRepository();
            return (List<Track>)(repo.GetAll());
        }

        public virtual void Delete()
        {
            IRepository<Track> repo = new TrackRepository();

            repo.Delete(this);
        }
    }
}
