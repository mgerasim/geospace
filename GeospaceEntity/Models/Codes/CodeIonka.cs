using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeospaceEntity.Models.Codes
{
    public class CodeIonka
    {
        private string strSession;


        public CodeIonka()
        {
            ID = -1;
            created_at = DateTime.Now;
            updated_at = DateTime.Now;
        }
        
        public CodeIonka(List<string> sessionGroup)
        {
            // TODO: Complete member initialization
            this.created_at = DateTime.Now;
            this.updated_at = DateTime.Now;
            HH = GeospaceEntity.Helper.HelperIonka.Ionka_Group05_HH(sessionGroup[0]);
            MI = GeospaceEntity.Helper.HelperIonka.Ionka_Group05_MI(sessionGroup[0]);
            if (sessionGroup.Count >= 2)
            {
                f0F2 = GeospaceEntity.Helper.HelperIonka.Ionka_Group06_f0F2(sessionGroup[1]);
                hF2 = GeospaceEntity.Helper.HelperIonka.Ionka_Group06_hF2(sessionGroup[1]);
            }
            if (sessionGroup.Count >= 3)
            {
                M3000F2 = GeospaceEntity.Helper.HelperIonka.Ionka_Group07_M3000F2(sessionGroup[2]);
                fmin = GeospaceEntity.Helper.HelperIonka.Ionka_Group07_fmin(sessionGroup[2]);
            }
            if (sessionGroup.Count >= 4)
            {
                f0Es = GeospaceEntity.Helper.HelperIonka.Ionka_Group08_f0Es(sessionGroup[3]);
                hEs = GeospaceEntity.Helper.HelperIonka.Ionka_Group08_hEs(sessionGroup[3]);
            }
            if (sessionGroup.Count >= 5)
            {
                f0F1 = GeospaceEntity.Helper.HelperIonka.Ionka_Group09_f0F1(sessionGroup[4]);
                hF1 = GeospaceEntity.Helper.HelperIonka.Ionka_Group09_hF1(sessionGroup[4]);
            }
            if (sessionGroup.Count >= 6)
            {
                M3000F1 = GeospaceEntity.Helper.HelperIonka.Ionka_Group10_M3000F1(sessionGroup[5]);
                hMF2 = GeospaceEntity.Helper.HelperIonka.Ionka_Group10_hMF2(sessionGroup[5]);
            }
            if (sessionGroup.Count >= 7)
            {
                f0E = GeospaceEntity.Helper.HelperIonka.Ionka_Group11_f0E(sessionGroup[6]);
                hE = GeospaceEntity.Helper.HelperIonka.Ionka_Group11_hE(sessionGroup[6]);
            }
            if (sessionGroup.Count >= 8)
            {
                fbEs = GeospaceEntity.Helper.HelperIonka.Ionka_Group12_fbEs(sessionGroup[7]);
                Es = GeospaceEntity.Helper.HelperIonka.Ionka_Group12_Es(sessionGroup[7]);
            }
            if (sessionGroup.Count >= 9)
            {
                fx1 = GeospaceEntity.Helper.HelperIonka.Ionka_Group13_fx1(sessionGroup[8]);
            }
            Raw = "";
            ErrorMessage = "";
        }

        public CodeIonka(string strSession)
        {
            // TODO: Complete member initialization
            this.strSession = strSession;
        }

        public virtual Station Station { get; set; }
        public virtual int ID { get; set; }
        public virtual DateTime created_at { get; set; }
        public virtual DateTime updated_at { get; set; }
        public virtual int HH { get; set; }
        public virtual int MM { get; set; }
        public virtual int DD { get; set; }
        public virtual int YYYY { get; set; }
        public virtual int MI { get; set; }
        public virtual int f0F2 { get; set; }
        public virtual int hF2 { get; set; }
        public virtual int M3000F2 { get; set; }
        public virtual int fmin { get; set; }
        public virtual int f0Es { get; set; }
        public virtual int hEs { get; set; }
        public virtual int f0F1 { get; set; }       
        public virtual int hF1 { get; set; }
        public virtual int M3000F1 { get; set; }
        public virtual int hMF2 { get; set; }
        public virtual int f0E { get; set; }
        public virtual int hE { get; set; }
        public virtual int fbEs { get; set; }
        public virtual int Es { get; set; }
        public virtual int fx1 { get; set; }
        public virtual string Raw { get; set; }
        public virtual string ErrorMessage { get; set; }

        // Display For Web Form
        public virtual string _f0F1 
        { 
            get 
            { 
                if (this.ID < 0) 
                    return ""; 
                else 
                    return DisplayValue(this.f0F1); 
            } 
        }
        public virtual string _f0F2 { get { if (this.ID < 0) return ""; else return DisplayValue(this.f0F2); } }
        public virtual string _M3000F2 { get { if (this.ID < 0) return ""; else return DisplayValue(this.M3000F2); } }
        public virtual string _M3000F1 { get { if (this.ID < 0) return ""; else return DisplayValue(this.M3000F1); } }
        public virtual string _hMF2 { get { if (this.ID < 0) return ""; else return DisplayValue(this.hMF2); } }
        public virtual string _hF2 { get { if (this.ID < 0) return ""; else return DisplayValue(this.hF2); } }
        public virtual string _hF1 { get { if (this.ID < 0) return ""; else return DisplayValue(this.hF1); } }
        public virtual string _hEs { get { if (this.ID < 0) return ""; else return DisplayValue(this.hEs); } }
        public virtual string _hE { get { if (this.ID < 0) return ""; else return DisplayValue(this.hE); } }
        public virtual string _fx1 { get { if (this.ID < 0) return ""; else return DisplayValue(this.fx1); } }
        public virtual string _fmin { get { if (this.ID < 0) return ""; else return DisplayValue(this.fmin); } }
        public virtual string _fbEs { get { if (this.ID < 0) return ""; else return DisplayValue(this.fbEs); } }
        public virtual string _f0Es { get { if (this.ID < 0) return ""; else return DisplayValue(this.f0Es); } }
        public virtual string _f0E { get { if (this.ID < 0) return ""; else return DisplayValue(this.f0E); } }
        public virtual string _Es { get { if (this.ID < 0) return ""; else return DisplayValue(this.Es); } }
        public virtual void Save()
        {
            this.created_at = DateTime.Now;
            this.updated_at = DateTime.Now;
            GeospaceEntity.Common.IRepository<CodeIonka> repo = new Repositories.CodeIonkaRepository();
            repo.Save(this);
        }

        public virtual void Update()
        {
            this.updated_at = DateTime.Now;
            GeospaceEntity.Common.IRepository<CodeIonka> repo = new Repositories.CodeIonkaRepository();
            repo.Update(this);
        }
        public virtual Codes.CodeIonka GetByDateUTC(Station station, int YYYY, int MM, int DD, int HH, int MI)
        {
            Repositories.CodeIonkaRepository repo = new Repositories.CodeIonkaRepository();
            return repo.GetByDateUTC(station, YYYY, MM, DD, HH, MI);
        }
        public virtual Codes.CodeIonka GetByDate(Station station, int YYYY, int MM, int DD, int HH)
        {
            Repositories.CodeIonkaRepository repo = new Repositories.CodeIonkaRepository();
            return repo.GetByDate(station, YYYY, MM, DD, HH);
        }
        public virtual IList<Codes.CodeIonka> GetByPeriod(Station station, int startYYYY, int startMM, int startDD, int endYYYY, int endMM, int endDD)
        {
            Repositories.CodeIonkaRepository repo = new Repositories.CodeIonkaRepository();
            return repo.GetByPeriod(station, startYYYY, startMM, startDD, endYYYY, endMM, endDD);
        }
        public virtual IList<Codes.CodeIonka> GetAll()
        {
            GeospaceEntity.Common.IRepository<CodeIonka> repo = new Repositories.CodeIonkaRepository();
            return repo.GetAll();
        }
        public virtual void PrintToConsole()
        {
            Console.WriteLine("HH:{0} DD:{1} f0F2:{2} hF2:{3} M3000F2:{4} fmin:{5} f0Es:{6} hEs:{7} f0F1:{8} hF1:{9} M3000F1:{10} hMF2:{11} f0E:{12} hE:{13} fbEs:{14} Es:{15} fx1:{16}",
                    MM, DD,
                    f0F2, hF2,
                    M3000F2, fmin,
                    f0Es, hEs,
                    f0F1, hF1,
                    M3000F1, hMF2,
                    f0E, hE,
                    fbEs, Es,
                    fx1);
        }
        public virtual string DisplayValue(int Value)
        {
            switch (Value)
            {
                case 1000: return "0";
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

    }
}
