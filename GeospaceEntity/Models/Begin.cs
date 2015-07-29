using GeospaceEntity.Common;
using GeospaceEntity.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeospaceEntity.Models
{
    public class Begin
    {
        public Begin()
        {
            ID = -1;
        }
        public virtual int ID { get; set; }
        public virtual DateTime created_at { get; set; }
        public virtual DateTime updated_at { get; set; }
        public virtual string Raw { get; set; }

        public virtual void Save()
        {
            IRepository<Begin> repo = new BeginRepository();
            this.created_at = DateTime.Now;
            this.updated_at = DateTime.Now;
            repo.Save(this);
        }
        
        public static void Save_From_File( string fileName )
        {
            StreamReader sr = new StreamReader(fileName);
            string str = sr.ReadToEnd();
            string[] arrString = str.Split();

            foreach( string s in arrString)
            {
                if( GetByRaw(s) == null && s.Length > 0)
                {
                    Begin begin = new Begin();
                    begin.Raw = s;
                    begin.Save();
                }
            }

            sr.Close();
        }


        public virtual void Update()
        {
            IRepository<Begin> repo = new BeginRepository();
            this.updated_at = DateTime.Now;
            repo.Update(this);
        }

        public static GeospaceEntity.Models.Begin GetByRaw(string Raw)
        {
            BeginRepository repo = new BeginRepository();
            return repo.GetByRaw(Raw);
        }

        public static List<string> GetAll()
        {
            IRepository<Begin> repo = new BeginRepository();
            List<Begin> list = (List<Begin>)repo.GetAll();

            List<string> listString = new List<string>();
            foreach( var item in list )
            {
                listString.Add( item.Raw );
            }

            return listString;
        }
    }
}
