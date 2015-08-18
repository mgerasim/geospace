using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GeospaceEntity.Common;
using NHibernate;
using NHibernate.Criterion;

namespace GeospaceEntity.Repositories
{
    public class TrackRepository : IRepository<GeospaceEntity.Models.Track>
    {
        #region IRepository<Track> Members

        void IRepository<GeospaceEntity.Models.Track>.Save(GeospaceEntity.Models.Track entity)
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

        void IRepository<GeospaceEntity.Models.Track>.Update(GeospaceEntity.Models.Track entity)
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

        void IRepository<GeospaceEntity.Models.Track>.Delete(GeospaceEntity.Models.Track entity)
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

        GeospaceEntity.Models.Track IRepository<GeospaceEntity.Models.Track>.GetById(int id)
        {
            using (ISession session = NHibernateHelper.OpenSession())
                return session.CreateCriteria<GeospaceEntity.Models.Track>().Add(Restrictions.Eq("ID", id)).UniqueResult<GeospaceEntity.Models.Track>();
        }


        IList<GeospaceEntity.Models.Track> IRepository<GeospaceEntity.Models.Track>.GetAll()
        {      
            using (ISession session = NHibernateHelper.OpenSession())
            {  
                ICriteria criteria = session.CreateCriteria(typeof(GeospaceEntity.Models.Track));
                criteria.AddOrder(Order.Asc("ID"));
                return criteria.List<GeospaceEntity.Models.Track>();
            }
        }

        #endregion
    }
}
