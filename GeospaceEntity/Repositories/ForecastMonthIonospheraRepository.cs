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
    class ForecastMonthIonospheraRepository : IRepository<Models.Telegram.ForecastMonthIonosphera>
    {
        #region IRepository<Measurement> Members

        void IRepository<Models.Telegram.ForecastMonthIonosphera>.Save(Models.Telegram.ForecastMonthIonosphera entity)
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

        void IRepository<Models.Telegram.ForecastMonthIonosphera>.Update(Models.Telegram.ForecastMonthIonosphera entity)
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

        void IRepository<Models.Telegram.ForecastMonthIonosphera>.Delete( Models.Telegram.ForecastMonthIonosphera entity)
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

        public Models.Telegram.ForecastMonthIonosphera GetById(int id)
        {
            using (ISession session = NHibernateHelper.OpenSession())
                return session.CreateCriteria< Models.Telegram.ForecastMonthIonosphera>().Add(Restrictions.Eq("ID", id)).UniqueResult< Models.Telegram.ForecastMonthIonosphera>();
        }


        IList<Models.Telegram.ForecastMonthIonosphera> IRepository<Models.Telegram.ForecastMonthIonosphera>.GetAll()
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                ICriteria criteria = session.CreateCriteria(typeof(Models.Telegram.ForecastMonthIonosphera));
                criteria.AddOrder(Order.Desc("ID"));
                return criteria.List< Models.Telegram.ForecastMonthIonosphera>();
            }
        }

        public Models.Telegram.ForecastMonthIonosphera GetByDateUTC(Station station, int YYYY, int MM)
        {
            using (ISession session = NHibernateHelper.OpenSession())

                return session.CreateCriteria< Models.Telegram.ForecastMonthIonosphera>()
                    .Add(Restrictions.Eq("Station", station))
                    .Add(Restrictions.Eq("YYYY", YYYY))
                    .Add(Restrictions.Eq("MM", MM)).UniqueResult< Models.Telegram.ForecastMonthIonosphera>();
        }
        #endregion

        internal List<Models.Telegram.ForecastMonthIonosphera> GetAllByDateUTC(int YYYY, int MM)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                var list = session.CreateCriteria< Models.Telegram.ForecastMonthIonosphera>()
                    .Add(Restrictions.Eq("YYYY", YYYY))
                    .Add(Restrictions.Eq("MM", MM))
                    .List< Models.Telegram.ForecastMonthIonosphera>();
                return (List< Models.Telegram.ForecastMonthIonosphera>)list;
            }
        }
    }

}
