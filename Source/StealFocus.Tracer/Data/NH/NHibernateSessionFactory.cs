// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NHibernateSessionFactory.cs" company="StealFocus">
//   Copyright StealFocus. All rights reserved.
// </copyright>
// <summary>
//   Defines the NHibernateSessionFactory type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace StealFocus.Tracer.Data.NH
{
    using System.Collections.Generic;
    using System.IO;

    using NHibernate;
    
    using NHibernate.Cfg;
    using NHibernate.Connection;
    using NHibernate.Dialect;
    using NHibernate.Driver;

    public static class NHibernateSessionFactory
    {
        private static ISessionFactory factory;

        public static ISession Create()
        {
            Configuration configuration = new Configuration();
            try
            {
                configuration.Configure();
            }
            catch (HibernateConfigException e)
            {
                if (e.InnerException != null && 
                    e.InnerException is FileNotFoundException && 
                    e.InnerException.Message.Contains("hibernate.cfg.xml"))
                {
                    // Could not find default NHibernate configuration so create programmatically.
                    configuration = new Configuration()
                        .AddProperties(new Dictionary<string, string>
                        {
                            { Environment.ConnectionDriver, typeof(SQLite20Driver).FullName },
                            { Environment.Dialect, typeof(SQLiteDialect).FullName },
                            { Environment.ConnectionProvider, typeof(DriverConnectionProvider).FullName },
                            { Environment.ConnectionString, "Data Source=StealFocus.Tracer.sqlLiteDB;Version=3;" },
                            { Environment.Hbm2ddlAuto, "create" },
                            { Environment.ShowSql, true.ToString() }
                        });
                }
                else
                {
                    // Something else happened.
                    throw;
                }
            }

            configuration.AddAssembly(typeof(NHibernateSessionFactory).Assembly);
            factory = configuration.BuildSessionFactory();
            return factory.OpenSession();
        }
    }
}
