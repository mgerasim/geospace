using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

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

        //строка с явлениями вида:  явление1.ЧЧ:ММ, явление2.ЧЧ:ММ, ...
        public virtual string events { get; set; }

        public virtual string Raw { get; set; }
        public virtual string ErrorMessage { get; set; }
        public CodeUmagf()
        {
            ID = -1;
            created_at = DateTime.Now;
            updated_at = DateTime.Now;
            MI = 0;
            k1 = 1000;
            k2 = 1000;
            k3 = 1000;
            k4 = 1000;
            k5 = 1000;
            k6 = 1000;
            k7 = 1000;
            k8 = 1000;

            ak = 1000;
            events = "";            

            Raw = "";
            ErrorMessage = "";
        }

        public CodeUmagf(string strUmagf)
        {
            this.created_at = DateTime.Now;
            this.updated_at = DateTime.Now;

            k1 = 1000;
            k2 = 1000;
            k3 = 1000;
            k4 = 1000;
            k5 = 1000;
            k6 = 1000;
            k7 = 1000;
            k8 = 1000;

            ak = 1000;

            events = ""; 
            Raw = "";
            ErrorMessage = "";
        }
        public virtual string DisplayValue(int Value)
        {
            switch (Value)
            {
                case 1000: return "/";
                case 1001: return "A";
                case 1002: return "B";
                case 1003: return "C";
                case 1004: return "D";
                case 1005: return "E";
                case 1006: return "F";
                case 1007: return "G";
                case 1008: return "N";
                case 1009: return "R";
                //default: Value.ToString();
            }
            return Value.ToString();
        }
        // Display For Web Form
        public virtual string _ak
        {
            get
            {
                if (this.ID < 0)
                    return "";
                else
                    return DisplayValue(this.ak);
            }
        }

        public virtual string _k1
        {
            get
            {
                if (this.ID < 0)
                    return "//";
                else
                    return DisplayValue(this.k1);
            }
        }
        public virtual string _k2
        {
            get
            {
                if (this.ID < 0)
                    return "";
                else
                    return DisplayValue(this.k2);
            }
        }

        public virtual string _k3
        {
            get
            {
                if (this.ID < 0)
                    return "";
                else
                    return DisplayValue(this.k3);
            }
        }

        public virtual string _k4
        {
            get
            {
                if (this.ID < 0)
                    return "";
                else
                    return DisplayValue(this.k4);
            }
        }

        public virtual string _k5
        {
            get
            {
                if (this.ID < 0)
                    return "";
                else
                    return DisplayValue(this.k5);
            }
        }

        public virtual string _k6
        {
            get
            {
                if (this.ID < 0)
                    return "";
                else
                    return DisplayValue(this.k6);
            }
        }

        public virtual string _k7
        {
            get
            {
                if (this.ID < 0)
                    return "";
                else
                    return DisplayValue(this.k7);
            }
        }

        public virtual string _k8
        {
            get
            {
                if (this.ID < 0)
                    return "";
                else
                    return DisplayValue(this.k8);
            }
        }

        public virtual string _events
        {
            get
            {
                if (this.ID < 0)
                    return "";
                else
                    return this.events;
            }
        }
        
        public static Codes.CodeUmagf GetByDateUTC(Station station, int YYYY, int MM, int DD, int HH, int MI)
        {
            Repositories.CodeUmagfRepository repo = new Repositories.CodeUmagfRepository();
            return repo.GetByDateUTC(station, YYYY, MM, DD, HH, MI);
        }

        public static Codes.CodeUmagf GetByDate(Station station, int YYYY, int MM, int DD)
        {
            Repositories.CodeUmagfRepository repo = new Repositories.CodeUmagfRepository();
            return repo.GetByDate(station, YYYY, MM, DD);
        }

        //сохранить новую запись в БД
        public virtual void Save()
        {
            this.created_at = DateTime.Now;
            this.updated_at = DateTime.Now;
            GeospaceEntity.Common.IRepository<CodeUmagf> repo = new Repositories.CodeUmagfRepository();
            repo.Save(this);
        }

        public virtual void Update()
        {
            this.updated_at = DateTime.Now;
            GeospaceEntity.Common.IRepository<CodeUmagf> repo = new Repositories.CodeUmagfRepository();
            repo.Update(this);
        }

        public static IList<Codes.CodeUmagf> GetByPeriod(Station station, int startYYYY, int startMM, int startDD, int endYYYY, int endMM, int endDD)
        {
            Repositories.CodeUmagfRepository repo = new Repositories.CodeUmagfRepository();
            return repo.GetByPeriod(station, startYYYY, startMM, startDD, endYYYY, endMM, endDD);
        }

        public static IList<CodeUmagf> GetByPeriod(Station station, DateTime dateStart, DateTime dateEnd)
        {
            return CodeUmagf.GetByPeriod(station, dateStart.Year, dateStart.Month, dateStart.Day,
                                                    dateEnd.Year, dateEnd.Month, dateEnd.Day);
        }

        public static Codes.CodeUmagf GetById(int id)
        {
            Repositories.CodeUmagfRepository repo = new Repositories.CodeUmagfRepository();
            return repo.GetById(id);
        }
        public virtual void PrintToFile(StreamWriter sw)
        {
            sw.WriteLine(Raw);
            sw.WriteLine("Hour:{0} Min:{1} Day:{2} Month:{12} Year:{13} K:{3}{4}{5}{6} {7}{8}{9}{10} Ak:{11} Events:{14}", 
                HH, MI, DD,_k1,_k2,_k3,_k4,_k5,_k6,_k7,_k8, _ak, MM, YYYY, events);
            sw.WriteLine("_______________________________________________________________");
        }
    }
}
