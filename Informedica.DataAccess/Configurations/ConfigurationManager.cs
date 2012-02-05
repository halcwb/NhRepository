using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using FluentNHibernate.Cfg;
using Informedica.DataAccess.Databases;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Context;
using NHibernate.Tool.hbm2ddl;

namespace Informedica.DataAccess.Configurations
{
    /// <summary>
    /// The responsibility of the ConfigurationManager class is
    /// to create a specific SessionFactory. To create a spedific mapping
    /// it needs a Database configurer and a Fluentconfiguration or an NHibernate
    /// fluentConfig.
    /// </summary>
    public class ConfigurationManager
    {
        private NHibernate.Cfg.Configuration _configuration;
        private readonly IDatabaseConfig _dbConfig;

        private static readonly IList<IEnvironmentConfiguration> _configurations = new List<IEnvironmentConfiguration>();
        
        private static ConfigurationManager _instance;
        private static readonly object LockThis = new object();

        public ConfigurationManager()
        {

        }



        public ConfigurationManager(IDatabaseConfig databaseConfig, FluentConfiguration fluentConfiguration)
        {
            _dbConfig = databaseConfig;
            fluentConfiguration.Database(_dbConfig.Configurer()).ExposeConfiguration(cfg => _configuration = cfg).BuildConfiguration();
        }

        public static ConfigurationManager Instance
        {
            get
            {
                if (_instance == null)
                    lock (LockThis)
                    {
                        if (_instance == null)
                        {
                            var instance = new ConfigurationManager();
                            Thread.MemoryBarrier();
                            _instance = instance;
                        }

                    }
                return _instance;
            }
        }

        public IEnumerable<IEnvironmentConfiguration> Configurations
        {
            get { return _configurations; }
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

        public static ConfigurationManager CreateInMemorySqlLiteFactoryCreator<TMap>()
        {
            var dbConfig = new SqlLiteConfig();
            var config = GetFluentConfig<TMap>();

            return new ConfigurationManager(dbConfig, config);

        }

        private static FluentConfiguration GetFluentConfig<TMap>()
        {
            var config = Fluently.Configure()
                .Mappings(x => x.FluentMappings.AddFromAssemblyOf<TMap>())
                .CurrentSessionContext<ThreadStaticSessionContext>()
                .ExposeConfiguration(x => x.SetProperty("connection.release_mode", "on_close"));
            return config;
        }

        public static ConfigurationManager CreatSqLiteFactory<TMap>(string connectionString)
        {
            var dbConfig = new SqlLiteConfig(connectionString);
            var config = GetFluentConfig<TMap>();

            return new ConfigurationManager(dbConfig, config);
        }

        public void AddConfiguration(string name, FluentConfiguration fluentConfig, IDatabaseConfig dbConfig)
        {
            AddConfiguration(name, fluentConfig.BuildConfiguration(), dbConfig);
        }

        public void AddConfiguration(string name, Configuration configuration, IDatabaseConfig dbConfig)
        {
            _configurations.Add(new EnvironmentConfiguration(name, configuration, dbConfig));
        }

        public void RemoveConfiguration(string name)
        {
            var config = _configurations.Single(c => c.EnvironmentName == name);
            RemoveConfiguration(config);
        }

        private static void RemoveConfiguration(IEnvironmentConfiguration config)
        {
            _configurations.Remove(config);
        }
    }

}
