using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GeospaceEntity.Repositories;
using GeospaceEntity.Common;


namespace GeospaceEntity.Models
{
    public class ConsolidatedTable
    {
        public virtual int ID { get; set; }
        public virtual DateTime created_at { get; set; }
        public virtual DateTime updated_at { get; set; }
        public virtual int YYYY { get; set; }
        public virtual int MM { get; set; }
        public virtual int DD  { get; set; }
        public virtual string Th2_W     { get; set; }
        public virtual string Th3_Sp    { get; set; }
        public virtual string Th4_F     { get; set; }
        public virtual string Th5_90M   { get; set; }
        public virtual string Th6_CountEvent { get; set; }

        private ICollection<EnergeticEvent> _EnergeticEvents;

        public virtual string Th11_     { get; set; }
        public virtual string Th12_AP   { get; set; }
        public virtual string Th13_Amag { get; set; }
        public virtual string Th14_Apar { get; set; }
        public virtual string Th15_Akha { get; set; }
        public virtual string Th16_K    { get; set; }
        public virtual string Th17_iSal { get; set; }
        public virtual string Th18_iMag { get; set; }
        public virtual string Th19_iKha { get; set; }
        public virtual string Th20_iPar { get; set; }

        public virtual ICollection<EnergeticEvent> EnergeticEvents
        {
            get
            {
                return this._EnergeticEvents;
            }
            set
            {
                this._EnergeticEvents = value;
            }
        }
        public ConsolidatedTable()
        {
            ID = -1;
            this.created_at = DateTime.Now;
            this.updated_at = DateTime.Now;
            _EnergeticEvents = new System.Collections.Generic.HashSet<EnergeticEvent>();
            YYYY = -1;
            MM = -1;
            DD = -1;
            Th2_W = "";
            Th3_Sp = "";
            Th4_F = "";
            Th5_90M = "";
            Th6_CountEvent = "";
            Th11_ = "";
            Th12_AP  = "";
            Th13_Amag= "";
            Th14_Apar= "";
            Th15_Akha= "";
            Th16_K   = "";
            Th17_iSal= "";
            Th18_iMag= "";
            Th19_iKha= "";
            Th20_iPar= "";
            
        }
        public virtual void Save()
        {
            IRepository<ConsolidatedTable> repo = new ConsolidatedTableRepository();
            this.created_at = DateTime.Now;
            this.updated_at = DateTime.Now;
            repo.Save(this);
        }
        public virtual void Update()
        {
            IRepository<ConsolidatedTable> repo = new ConsolidatedTableRepository();
            this.updated_at = DateTime.Now;
            repo.Update(this);
        }
        public virtual void Delete()
        {
            IRepository<ConsolidatedTable> repo = new ConsolidatedTableRepository();
            repo.Delete(this);
        }
        public static ConsolidatedTable GetById(int id)
        {
            IRepository<ConsolidatedTable> repo = new ConsolidatedTableRepository();
            return repo.GetById(id);
        }
        public virtual ConsolidatedTable GetByDateUTC(int YYYY, int MM, int DD)
        {
            Repositories.ConsolidatedTableRepository repo = new Repositories.ConsolidatedTableRepository();
            return repo.GetByDateUTC( YYYY, MM, DD );
        }
        public static IList<ConsolidatedTable> GetByDateMM(int YYYY, int MM)
        {
            Repositories.ConsolidatedTableRepository repo = new Repositories.ConsolidatedTableRepository();
            return repo.GetByDateMM(YYYY, MM);
        }
    }
}
