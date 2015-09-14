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
    public class MedianaRepository : IRepository<GeospaceEntity.Models.Mediana>
    {
        #region IRepository<Mediana> Members

        void IRepository<GeospaceEntity.Models.Mediana>.Save(GeospaceEntity.Models.Mediana entity)
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

        void IRepository<GeospaceEntity.Models.Mediana>.Update(GeospaceEntity.Models.Mediana entity)
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

        void IRepository<GeospaceEntity.Models.Mediana>.Delete(GeospaceEntity.Models.Mediana entity)
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

        GeospaceEntity.Models.Mediana IRepository<GeospaceEntity.Models.Mediana>.GetById(int id)
        {
            using (ISession session = NHibernateHelper.OpenSession())
                return session.CreateCriteria<GeospaceEntity.Models.Mediana>().Add(Restrictions.Eq("ID", id)).UniqueResult<GeospaceEntity.Models.Mediana>();
        }

        public IList<GeospaceEntity.Models.Mediana> GetByRangeNumber(Station station, int YYYY, int MM, int rangeNumber)
        {
            using (ISession session = NHibernateHelper.OpenSession())
                return session.CreateCriteria<GeospaceEntity.Models.Mediana>()
                        .Add(Restrictions.Eq("Station", station))
                        .Add(Restrictions.Eq("YYYY", YYYY))
                        .Add(Restrictions.Eq("MM", MM))
                        .Add(Restrictions.Eq("RangeNumber", rangeNumber))
                        .AddOrder(Order.Asc("HH"))
                        .List<GeospaceEntity.Models.Mediana>();
        }

        public IList<GeospaceEntity.Models.Mediana> GetByMonth(Station station, int YYYY, int MM)
        {
            using (ISession session = NHibernateHelper.OpenSession())
                return session.CreateCriteria<GeospaceEntity.Models.Mediana>()
                        .Add(Restrictions.Eq("Station", station))
                        .Add(Restrictions.Eq("YYYY", YYYY))
                        .Add(Restrictions.Eq("MM", MM))
                        .List<GeospaceEntity.Models.Mediana>();
        }

        public GeospaceEntity.Models.Mediana GetByDate(Station station, int YYYY, int MM, int HH, int rangeNumber)
        {
            GeospaceEntity.Models.Mediana mediana = null;
            
            using (ISession session = NHibernateHelper.OpenSession())
            {
                try
                {
                    mediana = session.CreateCriteria<GeospaceEntity.Models.Mediana>()
                        .Add(Restrictions.Eq("Station", station))
                        .Add(Restrictions.Eq("YYYY", YYYY))
                        .Add(Restrictions.Eq("MM", MM))
                        .Add(Restrictions.Eq("HH", HH))
                        .Add(Restrictions.Eq("RangeNumber", rangeNumber))
                        .UniqueResult<GeospaceEntity.Models.Mediana>();
                }
                catch(Exception)
                {
                    mediana = null;
                }
            }

            if (mediana == null)
                mediana = new Mediana();

            return mediana;
        }

        public IList<GeospaceEntity.Models.Mediana> GetByDate2(Station station, int YYYY, int MM, int rangeNumber)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                return session.CreateCriteria<GeospaceEntity.Models.Mediana>()
                    .Add(Restrictions.Eq("Station", station))
                    .Add(Restrictions.Eq("YYYY", YYYY))
                    .Add(Restrictions.Eq("MM", MM))
                    .Add(Restrictions.Eq("RangeNumber", rangeNumber))
                    .List<GeospaceEntity.Models.Mediana>();
            }
        }

        IList<GeospaceEntity.Models.Mediana> IRepository<GeospaceEntity.Models.Mediana>.GetAll()
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                ICriteria criteria = session.CreateCriteria(typeof(GeospaceEntity.Models.Station));
                return criteria.List<GeospaceEntity.Models.Mediana>();
            }
        }

        #endregion
    }
}
