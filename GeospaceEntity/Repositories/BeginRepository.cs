using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GeospaceEntity.Common;
using NHibernate;
using NHibernate.Criterion;

namespace GeospaceEntity.Repositories
{
    public class BeginRepository : IRepository<GeospaceEntity.Models.Begin>
    {
        #region IRepository<Begin> Members

        void IRepository<GeospaceEntity.Models.Begin>.Save(GeospaceEntity.Models.Begin entity)
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

        void IRepository<GeospaceEntity.Models.Begin>.Update(GeospaceEntity.Models.Begin entity)
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

        void IRepository<GeospaceEntity.Models.Begin>.Delete(GeospaceEntity.Models.Begin entity)
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

        GeospaceEntity.Models.Begin IRepository<GeospaceEntity.Models.Begin>.GetById(int id)
        {
            using (ISession session = NHibernateHelper.OpenSession())
                return session.CreateCriteria<GeospaceEntity.Models.Begin>().Add(Restrictions.Eq("ID", id)).UniqueResult<GeospaceEntity.Models.Begin>();
        }

        public GeospaceEntity.Models.Begin GetByRaw(string Raw)
        {
            using (ISession session = NHibernateHelper.OpenSession())
                return session.CreateCriteria<GeospaceEntity.Models.Begin>().Add(Restrictions.Eq("Raw", Raw)).UniqueResult<GeospaceEntity.Models.Begin>();
        }
        
        IList<GeospaceEntity.Models.Begin> IRepository<GeospaceEntity.Models.Begin>.GetAll()
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                ICriteria criteria = session.CreateCriteria(typeof(GeospaceEntity.Models.Begin));
                criteria.AddOrder(Order.Asc("ID"));
                return criteria.List<GeospaceEntity.Models.Begin>();
            }
        }

        #endregion
    }
}
