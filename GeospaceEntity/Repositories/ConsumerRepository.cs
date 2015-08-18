using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GeospaceEntity.Common;
using NHibernate;
using NHibernate.Criterion;

namespace GeospaceEntity.Repositories
{
    public class ConsumerRepository : IRepository<GeospaceEntity.Models.Consumer>
    {
        #region IRepository<Consumer> Members

        void IRepository<GeospaceEntity.Models.Consumer>.Save(GeospaceEntity.Models.Consumer entity)
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

        void IRepository<GeospaceEntity.Models.Consumer>.Update(GeospaceEntity.Models.Consumer entity)
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

        void IRepository<GeospaceEntity.Models.Consumer>.Delete(GeospaceEntity.Models.Consumer entity)
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

        GeospaceEntity.Models.Consumer IRepository<GeospaceEntity.Models.Consumer>.GetById(int id)
        {
            using (ISession session = NHibernateHelper.OpenSession())
                return session.CreateCriteria<GeospaceEntity.Models.Consumer>().Add(Restrictions.Eq("ID", id)).UniqueResult<GeospaceEntity.Models.Consumer>();
        }


        IList<GeospaceEntity.Models.Consumer> IRepository<GeospaceEntity.Models.Consumer>.GetAll()
        {      
            using (ISession session = NHibernateHelper.OpenSession())
            {  
                ICriteria criteria = session.CreateCriteria(typeof(GeospaceEntity.Models.Consumer));
                criteria.AddOrder(Order.Asc("ID"));
                return criteria.List<GeospaceEntity.Models.Consumer>();
            }
        }

        #endregion
    }
}
