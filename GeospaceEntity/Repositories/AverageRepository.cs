using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GeospaceEntity.Common;
using NHibernate;
using NHibernate.Criterion;

namespace GeospaceEntity.Repositories
{
    public class AverageRepository : IRepository<GeospaceEntity.Models.Average>
    {
        #region IRepository<Average> Members

        void IRepository<GeospaceEntity.Models.Average>.Save(GeospaceEntity.Models.Average entity)
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

        void IRepository<GeospaceEntity.Models.Average>.Update(GeospaceEntity.Models.Average entity)
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

        void IRepository<GeospaceEntity.Models.Average>.Delete(GeospaceEntity.Models.Average entity)
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

        GeospaceEntity.Models.Average IRepository<GeospaceEntity.Models.Average>.GetById(int id)
        {
            using (ISession session = NHibernateHelper.OpenSession())
                return session.CreateCriteria<GeospaceEntity.Models.Average>().Add(Restrictions.Eq("ID", id)).UniqueResult<GeospaceEntity.Models.Average>();
        }

       
        IList<GeospaceEntity.Models.Average> IRepository<GeospaceEntity.Models.Average>.GetAll()
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                ICriteria criteria = session.CreateCriteria(typeof(GeospaceEntity.Models.Average));
                criteria.AddOrder(Order.Asc("ID"));
                return criteria.List<GeospaceEntity.Models.Average>();
            }
        }

        #endregion
    }
}
