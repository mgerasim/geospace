﻿using GeospaceEntity.Common;
using GeospaceEntity.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeospaceEntity.Models
{
    public class Error
    {
        public Error()
        {
            ID = -1;
          //  FlagCheck = false;
        }
        public virtual int ID { get; set; }
        public virtual DateTime created_at { get; set; }
        public virtual DateTime updated_at { get; set; }
        public virtual string Raw { get; set; }
       public virtual string Description { get; set; }

        //public virtual bool FlagCheck { get; set; }
        public virtual void Save()
        {
            IRepository<Error> repo = new ErrorRepository();
            this.created_at = DateTime.Now;
            this.updated_at = DateTime.Now;
            repo.Save(this);
        }

        public virtual void Update()
        {
            IRepository<Error> repo = new ErrorRepository();
            this.updated_at = DateTime.Now;
            repo.Update(this);
        }

        public virtual GeospaceEntity.Models.Error GetByRaw(string Raw)
        {
            ErrorRepository repo = new ErrorRepository();
            return repo.GetByRaw(Raw);
        }

        public virtual GeospaceEntity.Models.Error GetByDescription()
        {
            ErrorRepository repo = new ErrorRepository();
            return repo.GetByDescription(Description);
        }
        public static IList<GeospaceEntity.Models.Error> GetAll()
        {
            IRepository<Error> repo = new ErrorRepository();
            return (List<Error>)(repo.GetAll());
        }
    }
}
