using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeospaceEntity.Models.Codes
{
    public class CodeUmagf
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
        public virtual int k1 { get; set; }
        public virtual int k2 { get; set; }
        public virtual int k3 { get; set; }
        public virtual int k4 { get; set; }
        public virtual int k5 { get; set; }
        public virtual int k6 { get; set; }
        public virtual int k7 { get; set; }
        public virtual int k8 { get; set; }

        //AK
        public virtual int ak { get; set; }

        public virtual string Raw { get; set; }
        public virtual string ErrorMessage { get; set; }
        public CodeUmagf()
        {
            ID = -1;
            created_at = DateTime.Now;
            updated_at = DateTime.Now;
        }

        public CodeUmagf(string strUmagf)
        {
            this.created_at = DateTime.Now;
            this.updated_at = DateTime.Now;

            k1 = 0;
            k2 = 0;
            k3 = 0;
            k4 = 0;
            k5 = 0;
            k6 = 0;
            k7 = 0;
            k8 = 0;

            ak = 0;

            Raw = "";
            ErrorMessage = "";
        }
    }
}
