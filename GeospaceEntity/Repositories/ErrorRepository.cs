using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GeospaceEntity.Common;
using NHibernate;
using NHibernate.Criterion;

namespace GeospaceEntity.Repositories
{
    public class ErrorRepository : IRepository<GeospaceEntity.Models.Error>
    {
        #region IRepository<Error> Members

        void IRepository<GeospaceEntity.Models.Error>.Save(GeospaceEntity.Models.Error entity)
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

        void IRepository<GeospaceEntity.Models.Error>.Update(GeospaceEntity.Models.Error entity)
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

        void IRepository<GeospaceEntity.Models.Error>.Delete(GeospaceEntity.Models.Error entity)
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

        GeospaceEntity.Models.Error IRepository<GeospaceEntity.Models.Error>.GetById(int id)
        {
            using (ISession session = NHibernateHelper.OpenSession())
                return session.CreateCriteria<GeospaceEntity.Models.Error>().Add(Restrictions.Eq("ID", id)).UniqueResult<GeospaceEntity.Models.Error>();
        }

        

        IList<GeospaceEntity.Models.Error> IRepository<GeospaceEntity.Models.Error>.GetAll()
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                ICriteria criteria = session.CreateCriteria(typeof(GeospaceEntity.Models.Error));
                criteria.AddOrder(Order.Asc("ID"));
                return criteria.List<GeospaceEntity.Models.Error>();
            }
        }

        #endregion
    }
}
