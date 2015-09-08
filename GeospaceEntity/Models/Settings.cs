using GeospaceEntity.Common;
using GeospaceEntity.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GeospaceEntity.Models
{
    public class Settings
    {
        public virtual int ID { get; set; }
       public virtual string GeospaceTrackExe {get; set;}
        public Settings()
        {

        }

        public virtual void Save()
        {
            IRepository<Settings> repo = new SettingsRepository();
            repo.Save(this);
        }

        public virtual void Update()
        {
            IRepository<Settings> repo = new SettingsRepository();
            repo.Update(this);
        }

        public virtual List<Settings> GetAll()
        {
            IRepository<Settings> repo = new SettingsRepository();
            repo.GetAll();
            return (List<Settings>)repo.GetAll();
        }
    }
}
