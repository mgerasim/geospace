using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeospaceEntity.Models.Codes
{
    public class CodeIonkaError
    {
        public virtual int ID { get; set; }
        public virtual DateTime created_at { get; set; }
        public virtual DateTime updated_at { get; set; }
        public virtual string Raw { get; set; }
        public virtual string ErrorMessage { get; set; }

        //по умолчанию false
        //если запись проверил специалист, то устанавливается true
        public virtual bool CheckError { get; set; }

        public CodeIonkaError()
        {
            ID = -1;
            Raw = "";
            ErrorMessage = "";
            CheckError = false;

            created_at = DateTime.Now;
            updated_at = DateTime.Now;
        }

        //сохранить новую запись в БД
        public virtual void Save()
        {
            this.created_at = DateTime.Now;
            this.updated_at = DateTime.Now;
            GeospaceEntity.Common.IRepository<CodeIonkaError> repo = new Repositories.CodeIonkaErrorRepository();
            repo.Save(this);
        }

        public virtual void Update()
        {
            this.updated_at = DateTime.Now;
            GeospaceEntity.Common.IRepository<CodeIonkaError> repo = new Repositories.CodeIonkaErrorRepository();
            repo.Update(this);
        }
        //по параметрам получаем объект из БД, если его нет значит сохраниме новую запись в БД
        public virtual Codes.CodeIonkaError GetByRaw()
        {
            Repositories.CodeIonkaErrorRepository repo = new Repositories.CodeIonkaErrorRepository();
            return repo.GetByRaw(Raw);
        }

        public virtual IList<Codes.CodeIonkaError> GetAll()
        {
            GeospaceEntity.Common.IRepository<CodeIonkaError> repo = new Repositories.CodeIonkaErrorRepository();
            return repo.GetAll();
        }

        //public virtual Codes.CodeIonkaError GetById(int id)
        //{
        //    Repositories.CodeIonkaErrorRepository repo = new Repositories.CodeIonkaErrorRepository();
        //    return repo.GetById(id);
        //}
    }
}
