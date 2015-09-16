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
    public class ConsolidatedTableRepository : IRepository<GeospaceEntity.Models.ConsolidatedTable>
    {
        #region IRepository<Measurement> Members

        void IRepository<GeospaceEntity.Models.ConsolidatedTable>.Save(GeospaceEntity.Models.ConsolidatedTable entity)
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

        void IRepository<GeospaceEntity.Models.ConsolidatedTable>.Update(GeospaceEntity.Models.ConsolidatedTable entity)
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

        void IRepository<GeospaceEntity.Models.ConsolidatedTable>.Delete(GeospaceEntity.Models.ConsolidatedTable entity)
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

        public GeospaceEntity.Models.ConsolidatedTable GetById(int id)
        {
            using (ISession session = NHibernateHelper.OpenSession())
                return session.CreateCriteria<GeospaceEntity.Models.ConsolidatedTable>().Add(Restrictions.Eq("ID", id)).UniqueResult<GeospaceEntity.Models.ConsolidatedTable>();
        }
        IList<GeospaceEntity.Models.ConsolidatedTable> IRepository<GeospaceEntity.Models.ConsolidatedTable>.GetAll()
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                ICriteria criteria = session.CreateCriteria(typeof(GeospaceEntity.Models.ConsolidatedTable));
                criteria.AddOrder(Order.Desc("ID"));
                return criteria.List<GeospaceEntity.Models.ConsolidatedTable>();
            }
        }
       
        public GeospaceEntity.Models.ConsolidatedTable GetByDateUTC(int YYYY, int MM, int DD)
        {
            using (ISession session = NHibernateHelper.OpenSession())

                return session.CreateCriteria<GeospaceEntity.Models.ConsolidatedTable>()
                    .Add(Restrictions.Eq("YYYY", YYYY))
                    .Add(Restrictions.Eq("MM", MM))
                    .Add(Restrictions.Eq("DD", DD))
                    .UniqueResult<GeospaceEntity.Models.ConsolidatedTable>();

        }

        public IList<GeospaceEntity.Models.ConsolidatedTable> GetByDateMM( int YYYY, int MM )
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
              return session.CreateCriteria<GeospaceEntity.Models.ConsolidatedTable>()
                    .Add(Restrictions.Eq("YYYY", YYYY))
                    .Add(Restrictions.Eq("MM", MM))
                    .AddOrder(Order.Asc("DD"))
                    .List<GeospaceEntity.Models.ConsolidatedTable>();
            }

        }
        
        #endregion
    }

}
