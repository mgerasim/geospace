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
        public virtual string Th7_Balls { get; set; }
        public virtual string Th8_Coordinates { get; set; }
        public virtual string Th9_Time { get; set; } 
        public virtual string Th10_RadioBursts { get; set; }
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

        public ConsolidatedTable()
        {
            ID = -1;
            this.created_at = DateTime.Now;
            this.updated_at = DateTime.Now;
            YYYY = -1;
            MM = -1;
            DD = -1;
            Th2_W = "";
            Th3_Sp = "";
            Th4_F = "";
            Th5_90M = "";
            Th6_CountEvent = "";
            Th7_Balls = "";
            Th8_Coordinates = "";
            Th9_Time = "";
            Th10_RadioBursts = "";
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
        public static ConsolidatedTable GetByDateUTC(int YYYY, int MM, int DD)
        {
            Repositories.ConsolidatedTableRepository repo = new Repositories.ConsolidatedTableRepository();
            return repo.GetByDateUTC( YYYY, MM, DD );
        }
        public static IList<ConsolidatedTable> GetByDateMM(int YYYY, int MM)
        {
            Repositories.ConsolidatedTableRepository repo = new Repositories.ConsolidatedTableRepository();
            return repo.GetByDateMM(YYYY, MM);
        }

        public virtual void SetValueByType(string type, string newvalue)
        {
            switch (type)
            {
                case "Th2":
                    {
                        this.Th2_W = newvalue;
                        break;
                    }
                case "Th3":
                    {
                        this.Th3_Sp = newvalue;
                        break;
                    }
                case "Th4":
                    {
                        this.Th4_F = newvalue;
                        break;
                    }
                case "Th5":
                    {
                        this.Th5_90M = newvalue;
                        break;
                    }
                case "Th6":
                    {
                        this.Th6_CountEvent = newvalue;
                        break;
                    }
                case "Th11":
                    {
                        this.Th11_ = newvalue;
                        break;
                    }
                case "Th12":
                    {
                        this.Th12_AP = newvalue;
                        break;
                    }
                case "Th13":
                    {
                        this.Th13_Amag = newvalue;
                        break;
                    }
                case "Th14":
                    {
                        this.Th14_Apar = newvalue;
                        break;
                    }
                case "Th15":
                    {
                        this.Th15_Akha = newvalue;
                        break;
                    }
                case "Th16":
                    {
                        this.Th16_K = newvalue;
                        break;
                    }
                case "Th17":
                    {
                        this.Th17_iSal = newvalue;
                        break;
                    }
                case "Th18":
                    {
                        this.Th18_iMag = newvalue;
                        break;
                    }
                case "Th19":
                    {
                        this.Th19_iKha = newvalue;
                        break;
                    }
                case "Th20":
                    {
                        this.Th20_iPar = newvalue;
                        break;
                    }
                default:
                    break;
            }
        }
    }
}
