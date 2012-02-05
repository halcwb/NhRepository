using System.Data;
using Informedica.DataAccess.Databases;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;

namespace Informedica.DataAccess.Configurations
{
    public class EnvironmentConfiguration: IEnvironmentConfiguration
    {
        private readonly Configuration _configuration;
        private readonly IDatabaseConfig _databaseConfig;
        private static ISessionFactory _factory;

        public EnvironmentConfiguration(string testEnv, Configuration config, IDatabaseConfig dbConfig)
        {
            _databaseConfig = dbConfig;
            _configuration = config;
            EnvironmentName = testEnv;
        }

        public string EnvironmentName { get; private set; }

        public ISessionFactory GetSessionFactory()
        {
            return _factory ?? (_factory = _configuration.BuildSessionFactory());
        }

        public void BuildSchema()
        {
            BuildSchema(_databaseConfig.GetConnection());
        }

        public void BuildSchema(ISession session)
        {
            BuildSchema(session.Connection);
        }

        public void BuildSchema(IDbConnection connection)
        {
            new SchemaExport(_configuration).Execute(true, true, false, connection, null);            
        }

    
    }
}