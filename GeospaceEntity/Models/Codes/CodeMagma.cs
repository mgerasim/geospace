using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace GeospaceEntity.Models.Codes
{
    public class CodeMagma
    {
        public virtual Station Station { get; set; }
        public virtual int ID { get; set; }
        public virtual DateTime created_at { get; set; }
        public virtual DateTime updated_at { get; set; }
        public virtual int HH { get; set; }
        public virtual int MM { get; set; }
        public virtual int DD { get; set; }
        public virtual int YYYY { get; set; }
        public virtual int MI { get; set; }

        //K-индексы
        public virtual int value { get; set; }      


        public virtual string Raw { get; set; }
        public virtual string ErrorMessage { get; set; }
        public CodeMagma()
        {
            ID = -1;
            created_at = DateTime.Now;
            updated_at = DateTime.Now;
            MI = 0;
            value = 0;    

            Raw = "";
            ErrorMessage = "";
        }

        
        
        public static Codes.CodeMagma GetByDateUTC(Station station, int YYYY, int MM, int DD, int HH, int MI)
        {
            Repositories.CodeMagmaRepository repo = new Repositories.CodeMagmaRepository();
            return repo.GetByDateUTC(station, YYYY, MM, DD, HH, MI);
        }

        public static Codes.CodeMagma GetByDate(Station station, int YYYY, int MM, int DD)
        {
            Repositories.CodeMagmaRepository repo = new Repositories.CodeMagmaRepository();
            return repo.GetByDate(station, YYYY, MM, DD);
        }

        //сохранить новую запись в БД
        public virtual void Save()
        {
            this.created_at = DateTime.Now;
            this.updated_at = DateTime.Now;
            GeospaceEntity.Common.IRepository<CodeMagma> repo = new Repositories.CodeMagmaRepository();
            repo.Save(this);
        }

        public virtual void Update()
        {
            this.updated_at = DateTime.Now;
            GeospaceEntity.Common.IRepository<CodeMagma> repo = new Repositories.CodeMagmaRepository();
            repo.Update(this);
        }

        public static IList<Codes.CodeMagma> GetByPeriod(Station station, int startYYYY, int startMM, int startDD, int endYYYY, int endMM, int endDD)
        {
            Repositories.CodeMagmaRepository repo = new Repositories.CodeMagmaRepository();
            return repo.GetByPeriod(station, startYYYY, startMM, startDD, endYYYY, endMM, endDD);
        }

        public static Codes.CodeMagma GetById(int id)
        {
            Repositories.CodeMagmaRepository repo = new Repositories.CodeMagmaRepository();
            return repo.GetById(id);
        }
       
    }
}
