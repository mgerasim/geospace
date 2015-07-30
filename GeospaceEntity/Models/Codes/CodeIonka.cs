using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace GeospaceEntity.Models.Codes
{
    public class CodeIonka
    {
        private string strSession;

        public CodeIonka()
        {
            ID = -1;

            Diffusio = -1;
            f0F2 = -1;
            hF2 = -1;
            M3000F2 = -1;
            fmin = -1;
            f0Es = -1;
            hEs = -1;
            f0F1 = -1;
            hF1 = -1;
            M3000F1 = -1;
            hMF2 = -1;
            f0E = -1;
            hE = -1;
            fbEs = -1;
            Es = -1;
            fx1 = -1;
            Raw = "";

            created_at = DateTime.Now;
            updated_at = DateTime.Now;
        }

        //public void SetCodeIonka( int val)
        //{
        //    ID = -1;

        //    Diffusio = val;
        //    f0F2 = val;
        //    hF2 = val;
        //    M3000F2 = val;
        //    fmin = val;
        //    f0Es = val;
        //    hEs = val;
        //    f0F1 = val;
        //    hF1 = val;
        //    M3000F1 = val;
        //    hMF2 = val;
        //    f0E = val;
        //    hE = val;
        //    fbEs = val;
        //    Es = val;
        //    fx1 = val;

        //    created_at = DateTime.Now;
        //    updated_at = DateTime.Now;
        //}
        
        public virtual void Decode(List<string> sessionGroup)
        {
            // TODO: Complete member initialization
            this.created_at = DateTime.Now;
            this.updated_at = DateTime.Now;
            HH = GeospaceEntity.Helper.HelperIonka.Ionka_Group05_HH(sessionGroup[0]);
            MI = GeospaceEntity.Helper.HelperIonka.Ionka_Group05_MI(sessionGroup[0]);
            if (sessionGroup.Count >= 2)
            {
                //bool flag = false;

                //for (int k = 0; k < sessionGroup[0].Length; k++)
                //{
                //    if (sessionGroup[0][k] > 61)
                //        flag = true;
                //}
                //if (flag)
                //{
                //    this.SetCodeIonka(1003);
                //    return;
                //}


                f0F2 = GeospaceEntity.Helper.HelperIonka.Ionka_Group06_f0F2(sessionGroup[1], this);
                hF2 = GeospaceEntity.Helper.HelperIonka.Ionka_Group06_hF2(sessionGroup[1], this);
            }
            if (sessionGroup.Count >= 3)
            {
                M3000F2 = GeospaceEntity.Helper.HelperIonka.Ionka_Group07_M3000F2(sessionGroup[2], this);
                fmin = GeospaceEntity.Helper.HelperIonka.Ionka_Group07_fmin(sessionGroup[2], this);

                Diffusio = GeospaceEntity.Helper.HelperIonka.Ionka_Group07_diffusio(sessionGroup[2], this);
            }
            if (sessionGroup.Count >= 4)
            {
                f0Es = GeospaceEntity.Helper.HelperIonka.Ionka_Group08_f0Es(sessionGroup[3], this);
                hEs = GeospaceEntity.Helper.HelperIonka.Ionka_Group08_hEs(sessionGroup[3], this);
            }
            if (sessionGroup.Count >= 5)
            {
                f0F1 = GeospaceEntity.Helper.HelperIonka.Ionka_Group09_f0F1(sessionGroup[4], this);
                hF1 = GeospaceEntity.Helper.HelperIonka.Ionka_Group09_hF1(sessionGroup[4], this);
            }
            if (sessionGroup.Count >= 6)
            {
                M3000F1 = GeospaceEntity.Helper.HelperIonka.Ionka_Group10_M3000F1(sessionGroup[5], this);
                hMF2 = GeospaceEntity.Helper.HelperIonka.Ionka_Group10_hMF2(sessionGroup[5], this);
            }
            if (sessionGroup.Count >= 7)
            {
                f0E = GeospaceEntity.Helper.HelperIonka.Ionka_Group11_f0E(sessionGroup[6], this);
                hE = GeospaceEntity.Helper.HelperIonka.Ionka_Group11_hE(sessionGroup[6], this);
            }
            if (sessionGroup.Count >= 8)
            {
                fbEs = GeospaceEntity.Helper.HelperIonka.Ionka_Group12_fbEs(sessionGroup[7], this);
                Es = GeospaceEntity.Helper.HelperIonka.Ionka_Group12_Es(sessionGroup[7], this);
            }
            if (sessionGroup.Count >= 9)
            {
                fx1 = GeospaceEntity.Helper.HelperIonka.Ionka_Group13_fx1(sessionGroup[8], this);
            }
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
        public virtual int Diffusio { get; set; }
        public virtual string Raw { get; set; }
        public virtual string ErrorMessage { get; set; }

        // Display For Web Form
        public virtual string _f0F1 
        { 
            get 
            {
                if (this.ID < 0 || this.f0F1 < 0) 
                    return ""; 
                else 
                    return DisplayValue(this.f0F1); 
            } 
        }
        public virtual string _f0F2 { get { if (this.ID < 0 || this.f0F2 < 0) return ""; else return DisplayValue(this.f0F2); } }
        public virtual string _M3000F2 { get { if (this.ID < 0 || this.M3000F2 < 0) return ""; else return DisplayValue(this.M3000F2); } }
        public virtual string _M3000F1 { get { if (this.ID < 0 || this.M3000F1 < 0) return ""; else return DisplayValue(this.M3000F1); } }
        public virtual string _hMF2 { get { if (this.ID < 0 || this.hMF2 < 0) return ""; else return DisplayValue(this.hMF2); } }
        public virtual string _hF2 { get { if (this.ID < 0 || this.hF2 < 0) return ""; else return DisplayValue(this.hF2); } }
        public virtual string _hF1 { get { if (this.ID < 0 || this.hF1 < 0) return ""; else return DisplayValue(this.hF1); } }
        public virtual string _hEs { get { if (this.ID < 0 || this.hEs < 0) return ""; else return DisplayValue(this.hEs); } }
        public virtual string _hE { get { if (this.ID < 0 || this.hE < 0) return ""; else return DisplayValue(this.hE); } }
        public virtual string _fx1 { get { if (this.ID < 0 || this.fx1 < 0) return ""; else return DisplayValue(this.fx1); } }
        public virtual string _fmin { get { if (this.ID < 0 || this.fmin < 0) return ""; else return DisplayValue(this.fmin); } }
        public virtual string _fbEs { get { if (this.ID < 0 || this.fbEs < 0) return ""; else return DisplayValue(this.fbEs); } }
        public virtual string _f0Es { get { if (this.ID < 0 || this.f0Es < 0) return ""; else return DisplayValue(this.f0Es); } }
        public virtual string _f0E { get { if (this.ID < 0 || this.f0E < 0) return ""; else return DisplayValue(this.f0E); } }
        public virtual string _Es { get { if (this.ID < 0 || this.Es < 0) return ""; else return DisplayValue(this.Es); } }
        public virtual string _Diffusio { get { if (this.ID < 0 || this.Diffusio < 0) return ""; else return DisplayValue(this.Diffusio); } }
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
        public static Codes.CodeIonka GetByDateUTC(Station station, int YYYY, int MM, int DD, int HH, int MI)
        {
            Repositories.CodeIonkaRepository repo = new Repositories.CodeIonkaRepository();
            return repo.GetByDateUTC(station, YYYY, MM, DD, HH, MI);
        }
        public static Codes.CodeIonka GetByDate(Station station, int YYYY, int MM, int DD, int HH)
        {
            Repositories.CodeIonkaRepository repo = new Repositories.CodeIonkaRepository();
            return repo.GetByDate(station, YYYY, MM, DD, HH);
        }
        public static IList<Codes.CodeIonka> GetByPeriod(Station station, int startYYYY, int startMM, int startDD, int endYYYY, int endMM, int endDD)
        {
            Repositories.CodeIonkaRepository repo = new Repositories.CodeIonkaRepository();
            return repo.GetByPeriod(station, startYYYY, startMM, startDD, endYYYY, endMM, endDD);
        }

        public static IList<Codes.CodeIonka> GetByPeriod(Station station, DateTime startDate, DateTime endDate)
        {
            Repositories.CodeIonkaRepository repo = new Repositories.CodeIonkaRepository();
            return repo.GetByPeriod(station, startDate.Year, startDate.Month, startDate.Day, endDate.Year, endDate.Month, endDate.Day);
        }

        public static IList<Codes.CodeIonka> GetAll()
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

        public virtual void PrintToFile(StreamWriter sw)
        {
            sw.WriteLine(Raw);
            sw.WriteLine("Hour:{0} Min:{1} Day:{2} Month:{18} Year:{19} f0F2:{3} hF2:{4} M3000F2:{5} fmin:{6} f0Es:{7} hEs:{8} f0F1:{9} hF1:{10} M3000F1:{11} hMF2:{12} f0E:{13} hE:{14} fbEs:{15} Es:{16} fx1:{17}",
                    HH, MI, DD,
                    _f0F2, _hF2,
                    _M3000F2, _fmin,
                    _f0Es, _hEs,
                    _f0F1, _hF1,
                    _M3000F1, _hMF2,
                    _f0E, _hE,
                    _fbEs, _Es,
                    _fx1, MM, YYYY);
            sw.WriteLine("_______________________________________________________________");
        }

        private static string StringEditedManually = "<Изменено вручную>";

        public virtual void SetValueByType(string type, int value)
        {
            if (Raw.Contains(StringEditedManually) == false)
            {
                Raw = StringEditedManually + Raw;
            }

            switch (type)
            {
                case "f0F2":
                    f0F2 = value;
                    break;
                case "M3000F2":
                    M3000F2 = value;
                    break;
                case "f0F1":
                    f0F1 = value;
                    break;
                case "M3000F1":
                    M3000F1 = value;
                    break;
                case "f0Es":
                    if (value == 0)
                    {
                        f0Es = 1002;
                    }
                    else
                    {
                        f0Es = value;
                    }
                    break;
                case "fmin":
                    fmin = value;
                    break;
                case "D":
                    Diffusio = value;
                    break;
            }
        }

        public static int ConvertCodeToInt(string code)
        {
            switch (code.ToUpper())
            {
                case "0":
                case "00": return 1000;
                case "A": return 1001;
                case "B": return  1002;
                case "C": return 1003;
                case "D": return 1004;
                case "E": return 1005;
                case "F": return 1006;
                case "G": return 1007;
                case "N": return 1008;
                case "R": return 1009;
                case "": return -1;
                default: return Int32.Parse(code);
            }
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
