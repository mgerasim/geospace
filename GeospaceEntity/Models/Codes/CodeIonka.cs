using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeospaceEntity.Models.Codes
{
    public class CodeIonka
    {
        public CodeIonka()
        {
            created_at = DateTime.Now;
            updated_at = DateTime.Now;
        }
        public CodeIonka(string strIonka)  
        {
            this.created_at = DateTime.Now;
            this.updated_at = DateTime.Now;
            HH = GeospaceEntity.Helper.HelperIonka.Ionka_Group05_HH(strIonka);
            MI = GeospaceEntity.Helper.HelperIonka.Ionka_Group05_MI(strIonka);
            f0F2 = GeospaceEntity.Helper.HelperIonka.Ionka_Group06_f0F2(strIonka);
            hF2 = GeospaceEntity.Helper.HelperIonka.Ionka_Group06_hF2(strIonka);
            M3000F2 = GeospaceEntity.Helper.HelperIonka.Ionka_Group07_M3000F2(strIonka);
            fmin = GeospaceEntity.Helper.HelperIonka.Ionka_Group07_fmin(strIonka);
            f0Es = GeospaceEntity.Helper.HelperIonka.Ionka_Group08_f0Es(strIonka);
            hEs = GeospaceEntity.Helper.HelperIonka.Ionka_Group08_hEs(strIonka);
            f0F1 = GeospaceEntity.Helper.HelperIonka.Ionka_Group09_f0F1(strIonka);
            hF1 = GeospaceEntity.Helper.HelperIonka.Ionka_Group09_hF1(strIonka);
            M3000F1 = GeospaceEntity.Helper.HelperIonka.Ionka_Group10_M3000F1(strIonka);
            hMF2 = GeospaceEntity.Helper.HelperIonka.Ionka_Group10_hMF2(strIonka);
            f0E = GeospaceEntity.Helper.HelperIonka.Ionka_Group11_f0E(strIonka);
            hE = GeospaceEntity.Helper.HelperIonka.Ionka_Group11_hE(strIonka);
            fbEs = GeospaceEntity.Helper.HelperIonka.Ionka_Group12_fbEs(strIonka);
            Es = GeospaceEntity.Helper.HelperIonka.Ionka_Group12_Es(strIonka);
            fx1 = GeospaceEntity.Helper.HelperIonka.Ionka_Group13_fx1(strIonka);
            Raw = "";
            ErrorMessage = "";
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
    }
}
