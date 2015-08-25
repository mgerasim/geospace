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
    public class DisturbanceRepository : IRepository<GeospaceEntity.Models.Disturbance>
    {
        #region IRepository<Disturbance> Members

        void IRepository<GeospaceEntity.Models.Disturbance>.Save(GeospaceEntity.Models.Disturbance entity)
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

        void IRepository<GeospaceEntity.Models.Disturbance>.Update(GeospaceEntity.Models.Disturbance entity)
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

        void IRepository<GeospaceEntity.Models.Disturbance>.Delete(GeospaceEntity.Models.Disturbance entity)
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

        GeospaceEntity.Models.Disturbance IRepository<GeospaceEntity.Models.Disturbance>.GetById(int id)
        {
            using (ISession session = NHibernateHelper.OpenSession())
                return session.CreateCriteria<GeospaceEntity.Models.Disturbance>().Add(Restrictions.Eq("ID", id)).UniqueResult<GeospaceEntity.Models.Disturbance>();
        }

        public IList<GeospaceEntity.Models.Disturbance> GetByMonth(Station station, int YYYY, int MM)
        {
            using (ISession session = NHibernateHelper.OpenSession())
                return session.CreateCriteria<GeospaceEntity.Models.Disturbance>()
                        .Add(Restrictions.Eq("Station", station))
                        .Add(Restrictions.Eq("YYYY", YYYY))
                        .Add(Restrictions.Eq("MM", MM))
                        .List<GeospaceEntity.Models.Disturbance>();
        }

        public GeospaceEntity.Models.Disturbance GetByDate(Station station, int YYYY, int MM, int DD, int HH, int MI = 0)
        {
            GeospaceEntity.Models.Disturbance Disturbance = null;
            
            using (ISession session = NHibernateHelper.OpenSession())
            {
                try
                {
                    Disturbance = session.CreateCriteria<GeospaceEntity.Models.Disturbance>()
                        .Add(Restrictions.Eq("Station", station))
                        .Add(Restrictions.Eq("YYYY", YYYY))
                        .Add(Restrictions.Eq("MM", MM))
                        .Add(Restrictions.Eq("DD", DD))
                        .Add(Restrictions.Eq("HH", HH))
                        .Add(Restrictions.Eq("MI", MI))
                        .UniqueResult<GeospaceEntity.Models.Disturbance>();
                }
                catch(Exception)
                {
                    Disturbance = null;
                }
            }
            
            return Disturbance;
        }

        IList<GeospaceEntity.Models.Disturbance> IRepository<GeospaceEntity.Models.Disturbance>.GetAll()
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                ICriteria criteria = session.CreateCriteria(typeof(GeospaceEntity.Models.Disturbance));
                return criteria.List<GeospaceEntity.Models.Disturbance>();
            }
        }

        #endregion
    }
}
