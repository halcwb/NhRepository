using System;
using System.Data;
using FluentNHibernate.Cfg;
using HibernatingRhinos.Profiler.Appender.NHibernate;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;

namespace Informedica.DataAccess.Databases
{
    /// <summary>
    /// The responsibility of the SessionFactoryCreator class is
    /// to create a specific SessionFactory. To create a spedific mapping
    /// it needs a Database configurer and a Fluentconfiguration or an NHibernate
    /// configuration.
    /// </summary>
    public class SessionFactoryCreator
    {
        private Configuration _configuration;
        private readonly IDatabaseConfig _dbConfig;

        [Obsolete]
        static SessionFactoryCreator() { NHibernateProfiler.Initialize(); } 

        public SessionFactoryCreator(IDatabaseConfig databaseConfig, Configuration configuration)
        {
            _dbConfig = databaseConfig;
            _configuration = configuration;
        }

        public SessionFactoryCreator(IDatabaseConfig databaseConfig, FluentConfiguration fluentConfiguration)
        {
            _dbConfig = databaseConfig;
            fluentConfiguration.Database(_dbConfig.Configurer()).ExposeConfiguration(cfg => _configuration = cfg).BuildConfiguration();
        }

        private void BuildSchema(IDbConnection connection)
        {
            new SchemaExport(_configuration).Execute(false, true, false, connection, null);
        }

        public void BuildSchema()
        {
            BuildSchema(_dbConfig.GetConnection());
        }

        public ISessionFactory CreateSessionFactory()
        {
            return _configuration.BuildSessionFactory();
        }

        public void BuildSchema(ISession session)
        {
            BuildSchema(session.Connection);
        }
    }

}
