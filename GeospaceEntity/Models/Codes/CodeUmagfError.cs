using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeospaceEntity.Models.Codes
{
    public class CodeUmagfError
    {
        public virtual int ID { get; set; }
        public virtual DateTime created_at { get; set; }
        public virtual DateTime updated_at { get; set; }
        public virtual string Raw { get; set; }
        public virtual string ErrorMessage { get; set; }

        //по умолчанию false
        //если запись проверил специалист, то устанавливается true
        public virtual bool CheckError { get; set; }

        public CodeUmagfError()
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
            GeospaceEntity.Common.IRepository<CodeUmagfError> repo = new Repositories.CodeUmagfErrorRepository();
            repo.Save(this);
        }

        public virtual void Update()
        {
            this.updated_at = DateTime.Now;
            GeospaceEntity.Common.IRepository<CodeUmagfError> repo = new Repositories.CodeUmagfErrorRepository();
            repo.Update(this);
        }

        //по параметрам получаем объект из БД, если его нет значит сохраниме новую запись в БД
        public virtual Codes.CodeUmagfError GetByRaw()
        {
            Repositories.CodeUmagfErrorRepository repo = new Repositories.CodeUmagfErrorRepository();
            return repo.GetByRaw( Raw );
        }

        public virtual IList<Codes.CodeUmagfError> GetAll()
        {
            GeospaceEntity.Common.IRepository<CodeUmagfError> repo = new Repositories.CodeUmagfErrorRepository();
            return repo.GetAll();
        }

        public virtual Codes.CodeUmagfError GetById(int id)
        {
            Repositories.CodeUmagfErrorRepository repo = new Repositories.CodeUmagfErrorRepository();
            return repo.GetById(id);
        }
    }
}
