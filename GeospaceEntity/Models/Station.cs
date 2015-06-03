using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeospaceEntity.Models
{
    public class Station
    {
        int ID;
        int Code;
        DateTime created_at { get; set; }
        DateTime updated_at { get; set; }

        public Station()
        {
            this._IonkaValues = new System.Collections.Generic.HashSet<Codes.CodeIonka>();
        }
        private ICollection<Codes.CodeIonka> _IonkaValues;
        public virtual ICollection<Codes.CodeIonka> IonkaValues
        {
            get
            {
                return this._IonkaValues;
            }
            set
            {
                this._IonkaValues = value;
            }
        }

        public virtual void TryParser(string strIonka)
        {
            try
            {
                strIonka = Helper.HelperIonka.Check(strIonka);
                Code = Helper.HelperIonka.Ionka_Group02_Station(strIonka);
                DateTime Created_At = Helper.HelperIonka.Ionka_Group03_DateCreate(strIonka);
                int sessionCount = Helper.HelperIonka.Ionka_Group04_Count(strIonka);

                for (int i = 0; i < sessionCount; i++)
                {
                    string strSession = Helper.HelperIonka.Ionka_GroupData_Get(i, strIonka);
                    Codes.CodeIonka theIonka = new Codes.CodeIonka(strSession);
                    theIonka.DD = Created_At.Day;
                    theIonka.MM = Created_At.Month;
                    theIonka.YYYY = Created_At.Year;
                    theIonka.Station = this;
                    this._IonkaValues.Add(theIonka);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public virtual void PrintToConsole()
        {
            Console.WriteLine("Станция: {0}", this.Code);
            foreach (var item in this._IonkaValues)
            {
                item.PrintToConsole();
            }
        }
    }
}
