using GeospaceEntity.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeospaceEntity.Repositories;
using System.IO;

namespace GeospaceEntity.Models
{
    public class Consumer
    {
        public virtual int ID { get; set; }
        public virtual DateTime created_at { get; set; }
        public virtual DateTime updated_at { get; set; }
        public virtual string Name { get; set; }   

        private ICollection<Track> _Tracks;

        public Consumer()
        {
            ID = -1;
            created_at = DateTime.Now;
            updated_at = DateTime.Now;

            Name = "";
            this._Tracks = new System.Collections.Generic.HashSet<Track>();
        }

        public virtual ICollection<Track> Tracks
        {
            get
            {
                return this._Tracks;
            }
            set
            {
                this._Tracks = value;
            }
        }
        public virtual void ClearTracks()
        {
            this.Tracks.Clear();
        }

        public virtual Boolean IsExistTrack(int TrackID)
        {
            foreach (var theTrack in this.Tracks)
            {
                if (theTrack.ID == TrackID)
                {
                    return true;
                }
            }
            return false;
        }   

       
        public static GeospaceEntity.Models.Consumer GetById(int id)
        {
            IRepository<Consumer> repo = new Repositories.ConsumerRepository();
            return repo.GetById(id);
        }


        public virtual void Save()
        {
            IRepository<Consumer> repo = new ConsumerRepository();
            this.created_at = DateTime.Now;
            this.updated_at = DateTime.Now;
            repo.Save(this);
        }

        public virtual void Update()
        {
            IRepository<Consumer> repo = new ConsumerRepository();
            this.updated_at = DateTime.Now;
            repo.Update(this);
        }

        public static List<Consumer> GetAll()
        {
            IRepository<Consumer> repo = new ConsumerRepository();
            return (List<Consumer>)(repo.GetAll());
        }

        public virtual void Delete()
        {
            IRepository<Consumer> repo = new ConsumerRepository();

            repo.Delete(this);
        }

        public static void Print_All( string path )
        {
            StreamWriter sw = new StreamWriter(path);

            List<Consumer> consumers = Consumer.GetAll();


            foreach( Consumer cons in consumers)
            {
                sw.WriteLine(cons.Name);
                foreach (Track item in cons.Tracks)
                {
                    sw.WriteLine(item.Name + ": " + item.PointA.Name + " " + item.PointA.Latitude + " " + item.PointA.Longitude + " " + item.PointB.Name + " " + item.PointB.Latitude + " " + item.PointB.Longitude);
                }
                sw.WriteLine("\n");
            }

            sw.Close();
        }
    }
}
