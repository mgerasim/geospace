using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GeospaceEntity.Common;
using NHibernate;
using NHibernate.Criterion;

namespace GeospaceEntity.Repositories
{
    public class PostRepository : IRepository<GeospaceEntity.Models.Post>
    {
        #region IRepository<Post> Members

        void IRepository<GeospaceEntity.Models.Post>.Save(GeospaceEntity.Models.Post entity)
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

        void IRepository<GeospaceEntity.Models.Post>.Update(GeospaceEntity.Models.Post entity)
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

        void IRepository<GeospaceEntity.Models.Post>.Delete(GeospaceEntity.Models.Post entity)
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

        GeospaceEntity.Models.Post IRepository<GeospaceEntity.Models.Post>.GetById(int id)
        {
            using (ISession session = NHibernateHelper.OpenSession())
                return session.CreateCriteria<GeospaceEntity.Models.Post>().Add(Restrictions.Eq("ID", id)).UniqueResult<GeospaceEntity.Models.Post>();
        }

        public GeospaceEntity.Models.Post GetByCode(int code)
        {
            using (ISession session = NHibernateHelper.OpenSession())
                return session.CreateCriteria<GeospaceEntity.Models.Post>().Add(Restrictions.Eq("Code", code)).UniqueResult<GeospaceEntity.Models.Post>();
        }

        IList<GeospaceEntity.Models.Post> IRepository<GeospaceEntity.Models.Post>.GetAll()
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                ICriteria criteria = session.CreateCriteria(typeof(GeospaceEntity.Models.Post));
                criteria.AddOrder(Order.Asc("Code"));
                return criteria.List<GeospaceEntity.Models.Post>();
            }
        }

        #endregion
    }
}
