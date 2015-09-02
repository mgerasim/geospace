using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GeospaceEntity.Common;
using NHibernate;
using NHibernate.Criterion;

namespace GeospaceEntity.Repositories
{
    public class SettingsRepository : IRepository<GeospaceEntity.Models.Settings>
    {
        #region IRepository<Settings> Members

        void IRepository<GeospaceEntity.Models.Settings>.Save(GeospaceEntity.Models.Settings entity)
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

        void IRepository<GeospaceEntity.Models.Settings>.Update(GeospaceEntity.Models.Settings entity)
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

        void IRepository<GeospaceEntity.Models.Settings>.Delete(GeospaceEntity.Models.Settings entity)
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

        GeospaceEntity.Models.Settings IRepository<GeospaceEntity.Models.Settings>.GetById(int id)
        {
            using (ISession session = NHibernateHelper.OpenSession())
                return session.CreateCriteria<GeospaceEntity.Models.Settings>().Add(Restrictions.Eq("ID", id)).UniqueResult<GeospaceEntity.Models.Settings>();
        }

       
        IList<GeospaceEntity.Models.Settings> IRepository<GeospaceEntity.Models.Settings>.GetAll()
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                ICriteria criteria = session.CreateCriteria(typeof(GeospaceEntity.Models.Settings));
                criteria.AddOrder(Order.Asc("ID"));
                return criteria.List<GeospaceEntity.Models.Settings>();
            }
        }

        #endregion
    }
}
