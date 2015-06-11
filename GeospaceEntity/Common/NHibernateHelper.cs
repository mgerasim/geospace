using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NHibernate;
using NHibernate.Cfg;

namespace GeospaceEntity.Common
{
    public class NHibernateHelper
    {
        private static ISessionFactory _sessionFactory;

        private static ISessionFactory SessionFactory
        {
            get
            {
                if (_sessionFactory == null)
                {
                    var configuration = new Configuration();
                    configuration.Configure();
                    configuration.AddAssembly(typeof(GeospaceEntity.Models.Station).Assembly);                    
                    
                    _sessionFactory = configuration.BuildSessionFactory();
                }
                return _sessionFactory;
            }
        }

        public static ISession OpenSession()
        {
            return SessionFactory.OpenSession();
        }

        public static void UpdateSchema()
        {
            var configuration = new Configuration();
            configuration.Configure();
         //   configuration.AddAssembly(typeof(GeospaceEntity.Models.Station).Assembly);
            configuration.AddAssembly(typeof(GeospaceEntity.Models.Error).Assembly);
            
            NHibernate.Tool.hbm2ddl.SchemaUpdate schemaUpdate
                = new NHibernate.Tool.hbm2ddl.SchemaUpdate(configuration);
            schemaUpdate.Execute(true, true);
        }
    }
}