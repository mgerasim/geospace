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
    public class CodeIonkaErrorRepository : IRepository<GeospaceEntity.Models.Codes.CodeIonkaError>
        {
            #region IRepository<Measurement> Members

            void IRepository<GeospaceEntity.Models.Codes.CodeIonkaError>.Save(GeospaceEntity.Models.Codes.CodeIonkaError entity)
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

            void IRepository<GeospaceEntity.Models.Codes.CodeIonkaError>.Update(GeospaceEntity.Models.Codes.CodeIonkaError entity)
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

            void IRepository<GeospaceEntity.Models.Codes.CodeIonkaError>.Delete(GeospaceEntity.Models.Codes.CodeIonkaError entity)
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
            GeospaceEntity.Models.Codes.CodeIonkaError IRepository<GeospaceEntity.Models.Codes.CodeIonkaError>.GetById(int id)
            {
                using (ISession session = NHibernateHelper.OpenSession())
                    return session.CreateCriteria<GeospaceEntity.Models.Codes.CodeIonkaError>().Add(Restrictions.Eq("ID", id)).UniqueResult<GeospaceEntity.Models.Codes.CodeIonkaError>();
            }

            IList<GeospaceEntity.Models.Codes.CodeIonkaError> IRepository<GeospaceEntity.Models.Codes.CodeIonkaError>.GetAll()
            {
                using (ISession session = NHibernateHelper.OpenSession())
                {
                    ICriteria criteria = session.CreateCriteria(typeof(GeospaceEntity.Models.Codes.CodeIonkaError));
                    criteria.AddOrder(Order.Desc("ID"));
                    return criteria.List<GeospaceEntity.Models.Codes.CodeIonkaError>();
                }
            }
            #endregion
        }
    
}
