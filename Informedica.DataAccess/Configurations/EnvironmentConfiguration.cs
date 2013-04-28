using System.Data;
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

        public EnvironmentConfiguration(string name, Configuration config, IDatabaseConfig dbConfig)
        {
            _databaseConfig = dbConfig;
            _configuration = config;
            EnvironmentName = name;
        }

        public string EnvironmentName { get; private set; }

        public ISessionFactory GetSessionFactory()
        {
            if (_factory == null)
            {
                _factory = _configuration.BuildSessionFactory();
            }

            return _factory;
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
            try
            {
                new SchemaExport(_configuration).Execute(true, true, false, connection, null);

            }
            catch (System.Exception e)
            {
                throw e;
            }
        }


        public IDbConnection GetConnection()
        {
            return _databaseConfig.GetConnection();
        }
    }
}