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
    class ForecastFiveDayRepository : IRepository<GeospaceEntity.Models.Telegram.ForecastFiveDay>
    {
        #region IRepository<Measurement> Members

        void IRepository<GeospaceEntity.Models.Telegram.ForecastFiveDay>.Save(GeospaceEntity.Models.Telegram.ForecastFiveDay entity)
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

        void IRepository<GeospaceEntity.Models.Telegram.ForecastFiveDay>.Update(GeospaceEntity.Models.Telegram.ForecastFiveDay entity)
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

        void IRepository<GeospaceEntity.Models.Telegram.ForecastFiveDay>.Delete(GeospaceEntity.Models.Telegram.ForecastFiveDay entity)
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

        public GeospaceEntity.Models.Telegram.ForecastFiveDay GetById(int id)
        {
            using (ISession session = NHibernateHelper.OpenSession())
                return session.CreateCriteria<GeospaceEntity.Models.Telegram.ForecastFiveDay>().Add(Restrictions.Eq("ID", id)).UniqueResult<GeospaceEntity.Models.Telegram.ForecastFiveDay>();
        }


        IList<GeospaceEntity.Models.Telegram.ForecastFiveDay> IRepository<GeospaceEntity.Models.Telegram.ForecastFiveDay>.GetAll()
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                ICriteria criteria = session.CreateCriteria(typeof(GeospaceEntity.Models.Telegram.ForecastFiveDay));
                criteria.AddOrder(Order.Desc("ID"));
                return criteria.List<GeospaceEntity.Models.Telegram.ForecastFiveDay>();
            }
        }
        public GeospaceEntity.Models.Telegram.ForecastFiveDay GetByDateUTC(Station station, int YYYY, int MM, int RangeNumber)
        {
            using (ISession session = NHibernateHelper.OpenSession())

                return session.CreateCriteria<GeospaceEntity.Models.Telegram.ForecastFiveDay>()
                    .Add(Restrictions.Eq("Station", station))
                    .Add(Restrictions.Eq("YYYY", YYYY))
                    .Add(Restrictions.Eq("MM", MM))
                    .Add(Restrictions.Eq("RangeNumber",RangeNumber)).UniqueResult<GeospaceEntity.Models.Telegram.ForecastFiveDay>();
        }
        #endregion
        }
        
}
