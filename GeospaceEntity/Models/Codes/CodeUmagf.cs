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
        //по параметрам получаем объект из БД, если его нет значит сохраниме новую запись в БД
        public virtual Codes.CodeUmagf GetByDateUTC()
        {
            Repositories.CodeUmagfRepository repo = new Repositories.CodeUmagfRepository();
            return repo.GetByDateUTC(Station, YYYY, MM, DD, HH, MI);
        }

        //сохранить новую запись в БД
        public virtual void Save()
        {
            this.created_at = DateTime.Now;
            this.updated_at = DateTime.Now;
            GeospaceEntity.Common.IRepository<CodeUmagf> repo = new Repositories.CodeUmagfRepository();
            repo.Save(this);
        }
        public virtual IList<Codes.CodeUmagf> GetByPeriod(Station station, int startYYYY, int startMM, int startDD, int endYYYY, int endMM, int endDD)
        {
            Repositories.CodeUmagfRepository repo = new Repositories.CodeUmagfRepository();
            return repo.GetByPeriod(station, startYYYY, startMM, startDD, endYYYY, endMM, endDD);
        }
    }
}
