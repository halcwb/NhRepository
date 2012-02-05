using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using FluentNHibernate.Cfg;
using Informedica.DataAccess.Databases;
using NHibernate.Cfg;
using NHibernate.Context;

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
        private static IDictionary<string, IEnvironmentConfiguration> _configurations; 
        
        private static ConfigurationManager _instance;
        private static readonly object LockThis = new object();

        static ConfigurationManager()
        {
            App_Start.NHibernateProfilerBootstrapper.PreStart();
        }

        public ConfigurationManager()
        {
            _configurations = new ConcurrentDictionary<string, IEnvironmentConfiguration>();
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
            get { return _configurations.Values; }
        }

        public IEnvironmentConfiguration GetConfiguration(string name)
        {
            return _configurations.Single(c => c.Key == name).Value;
        }


        public void AddInMemorySqLiteEnvironment<TMap>(string name)
        {
            var dbConfig = new SqlLiteConfig();
            var config = GetFluentConfig<TMap>();

            AddConfiguration(name, config, dbConfig);

        }

        private static FluentConfiguration GetFluentConfig<TMap>()
        {
            var config = Fluently.Configure()
                .Mappings(x => x.FluentMappings.AddFromAssemblyOf<TMap>())
                .CurrentSessionContext<ThreadStaticSessionContext>()
                .ExposeConfiguration(x => x.SetProperty("connection.release_mode", "on_close"));
            return config;
        }

        public void AddConfiguration(string name, FluentConfiguration fluentConfig, IDatabaseConfig dbConfig)
        {
            AddConfiguration(name, fluentConfig.Database(dbConfig.Configurer()).BuildConfiguration(), dbConfig);
        }

        public void AddConfiguration(string name, Configuration configuration, IDatabaseConfig dbConfig)
        {
            if (_configurations.ContainsKey(name)) return;
            _configurations.Add(name, new EnvironmentConfiguration(name, configuration, dbConfig));
        }

        public void RemoveConfiguration(string name)
        {
            var config = _configurations.Single(c => c.Key == name);
            RemoveConfiguration(config);
        }

        private static void RemoveConfiguration(KeyValuePair<string, IEnvironmentConfiguration> config)
        {
            _configurations.Remove(config);
        }
    }

}
