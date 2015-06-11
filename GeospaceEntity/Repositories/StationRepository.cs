using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GeospaceEntity.Common;
using NHibernate;
using NHibernate.Criterion;

namespace GeospaceEntity.Repositories
{
    public class StationRepository : IRepository<GeospaceEntity.Models.Station>
    {
        #region IRepository<Station> Members

        void IRepository<GeospaceEntity.Models.Station>.Save(GeospaceEntity.Models.Station entity)
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

        void IRepository<GeospaceEntity.Models.Station>.Update(GeospaceEntity.Models.Station entity)
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

        void IRepository<GeospaceEntity.Models.Station>.Delete(GeospaceEntity.Models.Station entity)
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

        GeospaceEntity.Models.Station IRepository<GeospaceEntity.Models.Station>.GetById(int id)
        {
            using (ISession session = NHibernateHelper.OpenSession())
                return session.CreateCriteria<GeospaceEntity.Models.Station>().Add(Restrictions.Eq("ID", id)).UniqueResult<GeospaceEntity.Models.Station>();
        }

        public GeospaceEntity.Models.Station GetByCode(int code)
        {
            using (ISession session = NHibernateHelper.OpenSession())
                return session.CreateCriteria<GeospaceEntity.Models.Station>().Add(Restrictions.Eq("Code", code)).UniqueResult<GeospaceEntity.Models.Station>();
        }

        IList<GeospaceEntity.Models.Station> IRepository<GeospaceEntity.Models.Station>.GetAll()
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                ICriteria criteria = session.CreateCriteria(typeof(GeospaceEntity.Models.Station));
                criteria.AddOrder(Order.Asc("Code"));
                return criteria.List<GeospaceEntity.Models.Station>();
            }
        }

        #endregion
    }
}
