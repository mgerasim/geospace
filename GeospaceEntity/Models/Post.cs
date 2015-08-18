using GeospaceEntity.Common;
using GeospaceEntity.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeospaceEntity.Models
{
    public class Post
    {
        public virtual int ID { get; set; }
        public virtual DateTime created_at { get; set; }
        public virtual DateTime updated_at { get; set; }
        public virtual string Name { get; set; }   
        public virtual double Longitude { get; set; }
        public virtual double Latitude { get; set; }


        public Post()
        {
            ID = -1;
            created_at = DateTime.Now;
            updated_at = DateTime.Now;

            Name = "";
            Longitude = 0;
            Latitude = 0;
        }
       
        public static GeospaceEntity.Models.Post GetById(int id)
        {
            IRepository<Post> repo = new Repositories.PostRepository();
            return repo.GetById(id);
        }

        public virtual void Save()
        {
            IRepository<Post> repo = new PostRepository();
            this.created_at = DateTime.Now;
            this.updated_at = DateTime.Now;
            repo.Save(this);
        }

        public virtual void Update()
        {
            IRepository<Post> repo = new PostRepository();
            this.updated_at = DateTime.Now;
            repo.Update(this);
        }

        public static List<Post> GetAll()
        {
            IRepository<Post> repo = new PostRepository();
            return (List<Post>)(repo.GetAll());
        }

        public virtual void Delete()
        {
            IRepository<Post> repo = new PostRepository();

            repo.Delete(this);
        }
    }
}
