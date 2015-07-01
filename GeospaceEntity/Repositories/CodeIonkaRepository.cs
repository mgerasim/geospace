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
    public class CodeIonkaRepository : IRepository<GeospaceEntity.Models.Codes.CodeIonka>
        {
            #region IRepository<Measurement> Members

            void IRepository<GeospaceEntity.Models.Codes.CodeIonka>.Save(GeospaceEntity.Models.Codes.CodeIonka entity)
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

            void IRepository<GeospaceEntity.Models.Codes.CodeIonka>.Update(GeospaceEntity.Models.Codes.CodeIonka entity)
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

            void IRepository<GeospaceEntity.Models.Codes.CodeIonka>.Delete(GeospaceEntity.Models.Codes.CodeIonka entity)
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

            GeospaceEntity.Models.Codes.CodeIonka IRepository<GeospaceEntity.Models.Codes.CodeIonka>.GetById(int id)
            {
                using (ISession session = NHibernateHelper.OpenSession())
                    return session.CreateCriteria<GeospaceEntity.Models.Codes.CodeIonka>().Add(Restrictions.Eq("ID", id)).UniqueResult<GeospaceEntity.Models.Codes.CodeIonka>();
            }

            public GeospaceEntity.Models.Codes.CodeIonka GetByCode(int code)
            {
                using (ISession session = NHibernateHelper.OpenSession())
                    return session.CreateCriteria<GeospaceEntity.Models.Codes.CodeIonka>().Add(Restrictions.Eq("Code", code)).UniqueResult<GeospaceEntity.Models.Codes.CodeIonka>();
            }

            IList<GeospaceEntity.Models.Codes.CodeIonka> IRepository<GeospaceEntity.Models.Codes.CodeIonka>.GetAll()
            {
                using (ISession session = NHibernateHelper.OpenSession())
                {
                    ICriteria criteria = session.CreateCriteria(typeof(GeospaceEntity.Models.Codes.CodeIonka));
                    criteria.AddOrder(Order.Desc("ID"));
                    return criteria.List<GeospaceEntity.Models.Codes.CodeIonka>();
                }
            }

            public IList<GeospaceEntity.Models.Codes.CodeIonka> GetByPeriod(Station station, int startYYYY, int startMM, int startDD, int endYYYY, int endMM, int endDD)
            {
                using (ISession session = NHibernateHelper.OpenSession())
                {
                    ICriteria criteria = session.CreateCriteria(typeof(GeospaceEntity.Models.Codes.CodeIonka));
                    criteria.AddOrder(Order.Desc("ID"));
                    criteria.Add(Restrictions.Eq("Station", station));
                    if (endYYYY > startYYYY)
                    {
                        criteria.Add(Restrictions.Between("YYYY", startYYYY, endYYYY));
                    }
                    else
                    {
                        criteria.Add(Restrictions.Eq("YYYY", endYYYY));
                    }
                    if (endMM > startMM)
                    {
                        criteria.Add(Restrictions.Between("MM", startMM, endMM));
                    }
                    else
                    {
                        criteria.Add(Restrictions.Eq("MM", endMM));
                    }
                    if (endDD > startDD)
                    {
                        criteria.Add(Restrictions.Between("DD", startDD, endDD));
                    }
                    else
                    {
                        criteria.Add(Restrictions.Eq("DD", endDD));
                    }
                    return criteria.List<GeospaceEntity.Models.Codes.CodeIonka>();
                }
            }

            public GeospaceEntity.Models.Codes.CodeIonka GetByDateUTC(Station station, int YYYY, int MM, int DD, int HH, int MI)
            {
                using (ISession session = NHibernateHelper.OpenSession())
                
                    return session.CreateCriteria<GeospaceEntity.Models.Codes.CodeIonka>()
                        .Add(Restrictions.Eq("Station", station))
                        .Add(Restrictions.Eq("YYYY", YYYY))
                        .Add(Restrictions.Eq("MM", MM))
                        .Add(Restrictions.Eq("DD", DD))
                        .Add(Restrictions.Eq("HH", HH))
                        .Add(Restrictions.Eq("MI", MI)).UniqueResult<GeospaceEntity.Models.Codes.CodeIonka>();
                
            }

            public GeospaceEntity.Models.Codes.CodeIonka GetByDate(Station station, int YYYY, int MM, int DD, int HH)
            {
                using (ISession session = NHibernateHelper.OpenSession())

                    return session.CreateCriteria<GeospaceEntity.Models.Codes.CodeIonka>()
                        .Add(Restrictions.Eq("Station", station))
                        .Add(Restrictions.Eq("YYYY", YYYY))
                        .Add(Restrictions.Eq("MM", MM))
                        .Add(Restrictions.Eq("DD", DD))
                        .Add(Restrictions.Eq("HH", HH)).UniqueResult<GeospaceEntity.Models.Codes.CodeIonka>();
            }

            #endregion
        }
    
}
