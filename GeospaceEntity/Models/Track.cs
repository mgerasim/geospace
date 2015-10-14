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

        public virtual double lengthTrack { get; set; }          //длина трассы
        public virtual int KTO { get; set; }                     //кол-во точек отсражения
        public virtual int KTP { get; set; }                     //кол-во точек поглащения

        //координаты точек отражения
        public virtual double lon1 { get; set; }
        public virtual double lat1 { get; set; }
        public virtual double lon2 { get; set; }
        public virtual double lat2 { get; set; }
        public virtual double lon3 { get; set; }
        public virtual double lat3 { get; set; }
        public virtual double lon4 { get; set; }
        public virtual double lat4 { get; set; }
        public virtual double lon5 { get; set; }
        public virtual double lat5 { get; set; }
        public virtual double lon6 { get; set; }
        public virtual double lat6 { get; set; }

        public virtual double lonP1 { get; set; }
        public virtual double latP1 { get; set; }
        public virtual double lonP2 { get; set; }
        public virtual double latP2 { get; set; }
        public virtual double lonP3 { get; set; }
        public virtual double latP3 { get; set; }
        public virtual double lonP4 { get; set; }
        public virtual double latP4 { get; set; }
        public virtual double lonP5 { get; set; }
        public virtual double latP5 { get; set; }
        public virtual double lonP6 { get; set; }
        public virtual double latP6 { get; set; }
        public virtual double lonP7 { get; set; }
        public virtual double latP7 { get; set; }
        public virtual double lonP8 { get; set; }
        public virtual double latP8 { get; set; }
        public virtual double lonP9 { get; set; }
        public virtual double latP9 { get; set; }
        public virtual double lonP10 { get; set; }
        public virtual double latP10 { get; set; }
        public virtual double lonP11 { get; set; }
        public virtual double latP11 { get; set; }
        public virtual double lonP12 { get; set; }
        public virtual double latP12 { get; set; }


        private ICollection<Consumer> _Consumers;        

        public Track()
        {
            ID = -1;
            created_at = DateTime.Now;
            updated_at = DateTime.Now;

            KTO = -1;
            KTP = -1;
            lengthTrack = -1;

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

            lonP1 = -1;
            latP1 = -1;
            lonP2 = -1;
            latP2 = -1;
            lonP3 = -1;
            latP3 = -1;
            lonP4 = -1;
            latP4 = -1;
            lonP5 = -1;
            latP5 = -1;
            lonP6 = -1;
            latP6 = -1;
            lonP7 = -1;
            latP7 = -1;
            lonP8 = -1;
            latP8 = -1;
            lonP9 = -1;
            latP9 = -1;
            lonP10 = -1;
            latP10 = -1;
            lonP11 = -1;
            latP11 = -1;
            lonP12 = -1;
            latP12 = -1;

            Name = "";
            this._Consumers = new System.Collections.Generic.HashSet<Consumer>();
        }
        public virtual double[,] Get_Points_O()
        {
            double[,] pointsO = new double[6, 2];

            pointsO[0, 0] = lon1;
            pointsO[0, 1] = lat1;
            pointsO[1, 0] = lon2;
            pointsO[1, 1] = lat2;
            pointsO[2, 0] = lon3;
            pointsO[2, 1] = lat3;
            pointsO[3, 0] = lon4;
            pointsO[3, 1] = lat4;
            pointsO[4, 0] = lon5;
            pointsO[4, 1] = lat5;
            pointsO[5, 0] = lon6;
            pointsO[5, 1] = lat6;

            return pointsO;
        }

        public virtual double[,] Get_Points_P()
        {
            double[,] pointsP = new double[12, 2];

            pointsP[0, 0] = lonP1;
            pointsP[0, 1] = latP1;
            pointsP[1, 0] = lonP2;
            pointsP[1, 1] = latP2;
            pointsP[2, 0] = lonP3;
            pointsP[2, 1] = latP3;
            pointsP[3, 0] = lonP4;
            pointsP[3, 1] = latP4;
            pointsP[4, 0] = lonP5;
            pointsP[4, 1] = latP5;
            pointsP[5, 0] = lonP6;
            pointsP[5, 1] = latP6;
            pointsP[6, 0] = lonP7;
            pointsP[6, 1] = latP7;
            pointsP[7, 0] = lonP8;
            pointsP[7, 1] = latP8;
            pointsP[8, 0] = lonP9;
            pointsP[8, 1] = latP9;
            pointsP[9, 0] = lonP10;
            pointsP[9, 1] = latP10;
            pointsP[10, 0] = lonP11;
            pointsP[10, 1] = latP11;
            pointsP[11, 0] = lonP12;
            pointsP[11, 1] = latP12;

            return pointsP;
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
