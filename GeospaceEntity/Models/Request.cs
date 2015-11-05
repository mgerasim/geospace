using GeospaceEntity.Common;
using GeospaceEntity.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeospaceEntity.Models
{
    public class Request
    {
        public virtual int ID { get; set; }
        public virtual string IP { get; set; }
        public virtual string Controller { get; set; }
        public virtual string MetodView { get; set; }
        public virtual DateTime created_at { get; set; }
        public virtual DateTime updated_at { get; set; }

        public Request()
        {
            ID = -1;
            IP = "";
            Controller = "";
            MetodView = "";
            created_at = DateTime.Now;
            updated_at = DateTime.Now;
        }
        public Request(string ip, string controller, string view)
        {
            ID = -1;
            IP = ip;
            Controller = controller;
            MetodView = view;
            created_at = DateTime.Now;
            updated_at = DateTime.Now;
        }
        public static GeospaceEntity.Models.Request GetById(int id)
        {
            IRepository<Request> repo = new Repositories.RequestRepository();
            return repo.GetById(id);
        }

        public virtual void Save()
        {
            IRepository<Request> repo = new RequestRepository();
            this.created_at = DateTime.Now;
            this.updated_at = DateTime.Now;
            repo.Save(this);
        }

        public virtual void Update()
        {
            IRepository<Request> repo = new RequestRepository();
            this.updated_at = DateTime.Now;
            repo.Update(this);
        }

        public static List<Request> GetAll()
        {
            IRepository<Request> repo = new RequestRepository();
            return (List<Request>)(repo.GetAll());
        }

        public virtual void Delete()
        {
            IRepository<Request> repo = new RequestRepository();

            repo.Delete(this);
        }

    }
}
