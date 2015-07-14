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
    public class CodeUmagfRepository : IRepository<GeospaceEntity.Models.Codes.CodeUmagf>
    {
        #region IRepository<Measurement> Members

        void IRepository<GeospaceEntity.Models.Codes.CodeUmagf>.Save(GeospaceEntity.Models.Codes.CodeUmagf entity)
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

        void IRepository<GeospaceEntity.Models.Codes.CodeUmagf>.Update(GeospaceEntity.Models.Codes.CodeUmagf entity)
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

        void IRepository<GeospaceEntity.Models.Codes.CodeUmagf>.Delete(GeospaceEntity.Models.Codes.CodeUmagf entity)
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

        public GeospaceEntity.Models.Codes.CodeUmagf GetById(int id)
        {
            using (ISession session = NHibernateHelper.OpenSession())
                return session.CreateCriteria<GeospaceEntity.Models.Codes.CodeUmagf>().Add(Restrictions.Eq("ID", id)).UniqueResult<GeospaceEntity.Models.Codes.CodeUmagf>();
        }
        
        public GeospaceEntity.Models.Codes.CodeUmagf GetByCode(int code)
        {
            using (ISession session = NHibernateHelper.OpenSession())
                return session.CreateCriteria<GeospaceEntity.Models.Codes.CodeUmagf>().Add(Restrictions.Eq("Code", code)).UniqueResult<GeospaceEntity.Models.Codes.CodeUmagf>();
        }
        
        IList<GeospaceEntity.Models.Codes.CodeUmagf> IRepository<GeospaceEntity.Models.Codes.CodeUmagf>.GetAll()
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                ICriteria criteria = session.CreateCriteria(typeof(GeospaceEntity.Models.Codes.CodeUmagf));
                criteria.AddOrder(Order.Desc("ID"));
                criteria.AddOrder(Order.Asc("MI"));
                return criteria.List<GeospaceEntity.Models.Codes.CodeUmagf>();
            }
        }
        
        public IList<GeospaceEntity.Models.Codes.CodeUmagf> GetByPeriod(Station station, int startYYYY, int startMM, int startDD, int endYYYY, int endMM, int endDD)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                ICriteria criteria = session.CreateCriteria(typeof(GeospaceEntity.Models.Codes.CodeUmagf));
                criteria.AddOrder(Order.Desc("ID"));
                criteria.Add(Restrictions.Eq("Station", station));

                System.DateTime startDate = new DateTime(startYYYY, startMM, startDD);
                System.DateTime endDate = new DateTime(endYYYY, endMM, endDD);

                var strYYYY = Projections.Cast(NHibernateUtil.String, Projections.Property("YYYY"));
                var strMM = Projections.Cast(NHibernateUtil.String, Projections.Property("MM"));
                var strDD = Projections.Cast(NHibernateUtil.String, Projections.Property("DD"));

                var sl = Projections.Cast(NHibernateUtil.String, Projections.Constant("/"));

                var projDate = Projections.SqlFunction("concat", NHibernateUtil.String, strDD, sl,
                    strMM, sl,
                    strYYYY);

                projDate = Projections.Cast(NHibernateUtil.DateTime, projDate);

                criteria.Add(Restrictions.Between(projDate, startDate, endDate));

                criteria.AddOrder(Order.Asc("MI"));

                return criteria.List<GeospaceEntity.Models.Codes.CodeUmagf>();
            }
        }

        public GeospaceEntity.Models.Codes.CodeUmagf GetByDateUTC(Station station, int YYYY, int MM, int DD, int HH, int MI)
        {
            using (ISession session = NHibernateHelper.OpenSession())

                return session.CreateCriteria<GeospaceEntity.Models.Codes.CodeUmagf>()
                    .Add(Restrictions.Eq("Station", station))
                    .Add(Restrictions.Eq("YYYY", YYYY))
                    .Add(Restrictions.Eq("MM", MM))
                    .Add(Restrictions.Eq("DD", DD))
                    .Add(Restrictions.Eq("HH", HH))
                    .Add(Restrictions.Eq("MI", MI)).UniqueResult<GeospaceEntity.Models.Codes.CodeUmagf>();

        }

        public GeospaceEntity.Models.Codes.CodeUmagf GetByDate(Station station, int YYYY, int MM, int DD)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                var list = session.CreateCriteria<GeospaceEntity.Models.Codes.CodeUmagf>()
                    .Add(Restrictions.Eq("Station", station))
                    .Add(Restrictions.Eq("YYYY", YYYY))
                    .Add(Restrictions.Eq("MM", MM))
                    .Add(Restrictions.Eq("DD", DD))
                    .AddOrder(Order.Asc("MI"))
                    .List<GeospaceEntity.Models.Codes.CodeUmagf>();

                if (list.Count != 0)
                {
                    return list[0];
                }

                return null;

            }

        }
        
        #endregion
    }

}
