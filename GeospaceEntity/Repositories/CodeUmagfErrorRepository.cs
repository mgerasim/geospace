using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GeospaceEntity.Common;
using NHibernate;
using NHibernate.Criterion;
using GeospaceEntity.Models;

namespace GeospaceEntity.Repositories
{
    public class CodeUmagfErrorRepository : IRepository<GeospaceEntity.Models.Codes.CodeUmagfError>
    {
        #region IRepository<Measurement> Members

        void IRepository<GeospaceEntity.Models.Codes.CodeUmagfError>.Save(GeospaceEntity.Models.Codes.CodeUmagfError entity)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.Save(entity);
                    transaction.Commit();
                }
            }
        }

        void IRepository<GeospaceEntity.Models.Codes.CodeUmagfError>.Update(GeospaceEntity.Models.Codes.CodeUmagfError entity)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.Update(entity);
                    transaction.Commit();
                }
            }
        }

        void IRepository<GeospaceEntity.Models.Codes.CodeUmagfError>.Delete(GeospaceEntity.Models.Codes.CodeUmagfError entity)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.Delete(entity);
                    transaction.Commit();
                }
            }
        }
        public GeospaceEntity.Models.Codes.CodeUmagfError GetById(int id)
        {
            using (ISession session = NHibernateHelper.OpenSession())
                return session.CreateCriteria<GeospaceEntity.Models.Codes.CodeUmagfError>().Add(Restrictions.Eq("ID", id)).UniqueResult<GeospaceEntity.Models.Codes.CodeUmagfError>();
        }

        public GeospaceEntity.Models.Codes.CodeUmagfError GetByCode(int code)
        {
            using (ISession session = NHibernateHelper.OpenSession())
                return session.CreateCriteria<GeospaceEntity.Models.Codes.CodeUmagfError>().Add(Restrictions.Eq("Code", code)).UniqueResult<GeospaceEntity.Models.Codes.CodeUmagfError>();
        }

        IList<GeospaceEntity.Models.Codes.CodeUmagfError> IRepository<GeospaceEntity.Models.Codes.CodeUmagfError>.GetAll()
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                ICriteria criteria = session.CreateCriteria(typeof(GeospaceEntity.Models.Codes.CodeUmagfError));
                criteria.AddOrder(Order.Desc("ID"));
                return criteria.List<GeospaceEntity.Models.Codes.CodeUmagfError>();
            }
        }
        public GeospaceEntity.Models.Codes.CodeUmagfError GetByRaw( string raw)
        {
            using (ISession session = NHibernateHelper.OpenSession())
                return session.CreateCriteria<GeospaceEntity.Models.Codes.CodeUmagfError>().Add(Restrictions.Eq("Raw", raw)).UniqueResult<GeospaceEntity.Models.Codes.CodeUmagfError>();
        }

        GeospaceEntity.Models.Codes.CodeUmagfError IRepository<GeospaceEntity.Models.Codes.CodeUmagfError>.GetById(int id)
        {
            using (ISession session = NHibernateHelper.OpenSession())
                return session.CreateCriteria<GeospaceEntity.Models.Codes.CodeUmagfError>().Add(Restrictions.Eq("ID", id)).UniqueResult<GeospaceEntity.Models.Codes.CodeUmagfError>();
        }
        


        #endregion
    }

}
