using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace GeospaceEntity.Models
{
    public class Average
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
       
        public virtual int F2_05 { get; set; }
        public virtual int F2_07 { get; set; }
        public virtual int F2_10 { get; set; }
        public virtual int F2_20 { get; set; }
        public virtual int F2_27 { get; set; }
        public virtual int F2_30 { get; set; }
        public virtual int M3000_05 { get; set; }
        public virtual int M3000_07 { get; set; }
        public virtual int M3000_10 { get; set; }
        public virtual int M3000_20 { get; set; }
        public virtual int M3000_27 { get; set; }
        public virtual int M3000_30 { get; set; }
    


        public Average()
        {
            
        }

       
    }
}
