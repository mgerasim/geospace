using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GeospaceEntity.Common;
using NHibernate;
using NHibernate.Criterion;

namespace GeospaceEntity.Repositories
{
    public class EnergeticEventRepository : IRepository<GeospaceEntity.Models.EnergeticEvent>
    {
        #region IRepository<EnergeticEvents> Members

        void IRepository<GeospaceEntity.Models.EnergeticEvent>.Save(GeospaceEntity.Models.EnergeticEvent entity)
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

        void IRepository<GeospaceEntity.Models.EnergeticEvent>.Update(GeospaceEntity.Models.EnergeticEvent entity)
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

        void IRepository<GeospaceEntity.Models.EnergeticEvent>.Delete(GeospaceEntity.Models.EnergeticEvent entity)
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

        GeospaceEntity.Models.EnergeticEvent IRepository<GeospaceEntity.Models.EnergeticEvent>.GetById(int id)
        {
            using (ISession session = NHibernateHelper.OpenSession())
                return session.CreateCriteria<GeospaceEntity.Models.EnergeticEvent>().Add(Restrictions.Eq("ID", id)).UniqueResult<GeospaceEntity.Models.EnergeticEvent>();
        }


        IList<GeospaceEntity.Models.EnergeticEvent> IRepository<GeospaceEntity.Models.EnergeticEvent>.GetAll()
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                ICriteria criteria = session.CreateCriteria(typeof(GeospaceEntity.Models.EnergeticEvent));
                criteria.AddOrder(Order.Asc("ID"));
                return criteria.List<GeospaceEntity.Models.EnergeticEvent>();
            }
        }



        #endregion

        public IList<GeospaceEntity.Models.EnergeticEvent> GetByDate(int YYYY, int MM, int DD)
        {
            using (ISession session = NHibernateHelper.OpenSession())

                return session.CreateCriteria<GeospaceEntity.Models.EnergeticEvent>()
                    .Add(Restrictions.Eq("YYYY", YYYY))
                    .Add(Restrictions.Eq("MM", MM))
                    .Add(Restrictions.Eq("DD", DD))
                    .AddOrder(Order.Asc("ID"))
                    .List<GeospaceEntity.Models.EnergeticEvent>();
        }
    }
}
