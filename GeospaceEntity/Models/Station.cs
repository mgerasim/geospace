using GeospaceEntity.Common;
using GeospaceEntity.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeospaceEntity.Models
{
    public class Station
    {
        public virtual int ID { get; set; }
        public virtual DateTime created_at { get; set; }
        public virtual DateTime updated_at { get; set; }
        public virtual string Name { get; set; }
        public virtual int Code { get; set; }

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


                if (Code == 43501)
                {
                    // Для Хабарвска код ИОНКА упращенный
                    return;
                }
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
                string err="";
                err += ex.Message + "\n";
                err += ex.StackTrace + "\n";
                err += ex.Source + "\n";

                throw new Exception(err);
            }
        }

        public virtual GeospaceEntity.Models.Station GetByCode(int code)
        {
            Repositories.StationRepository repo = new Repositories.StationRepository();
            return repo.GetByCode(code);
        }

        public virtual GeospaceEntity.Models.Station GetById(int id)
        {
            IRepository<Station> repo = new Repositories.StationRepository();
            return repo.GetById(id);
        }

        public virtual void PrintToConsole()
        {
            Console.WriteLine("Станция: {0}", this.Code);
            foreach (var item in this._IonkaValues)
            {
                item.PrintToConsole();
            }
        }

        public virtual void Save()
        {
            IRepository<Station> repo = new StationRepository();
            this.created_at = DateTime.Now;
            this.updated_at = DateTime.Now;
            repo.Save(this);
        }

        public virtual void Update()
        {
            IRepository<Station> repo = new StationRepository();
            this.updated_at = DateTime.Now;
            repo.Update(this);
        }

        public virtual List<Station> GetAll()
        {
            IRepository<Station> repo = new StationRepository();
            return (List<Station>)(repo.GetAll());
        }
    }
}
