using System.Data;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using HibernatingRhinos.Profiler.Appender.NHibernate;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Context;
using NHibernate.Tool.hbm2ddl;

namespace Informedica.DataAccess.Databases
{
    public class SessionFactoryCreator<TMap>
    {
        private static Configuration _configuration;
        private string _exportPath;
        private string _logPath;
        private IDatabaseConfig _dbConfig;
        private string _connectionString;

        static SessionFactoryCreator() { NHibernateProfiler.Initialize(); } 

        public SessionFactoryCreator() {} 

        public SessionFactoryCreator(IDatabaseConfig config, string connectionString, string exportPath, string logPath)
        {
            _dbConfig = config;
            _exportPath = exportPath;
            _logPath = logPath;
            _connectionString = connectionString;
        }

        public SessionFactoryCreator(IDatabaseConfig config)
        {
            _dbConfig = config;
        }

        public static void BuildSchema(IDbConnection connection)
        {
            new SchemaExport(_configuration).Execute(false, true, false, connection, null);
        }

        public void BuildSchema()
        {
            // first drop the database to recreate a new one
            // new SchemaExport(config).Drop(false, true);
            // this NHibernate tool takes a configuration (with mapping info in)
            // and exports a database schema from it
            new SchemaExport(_configuration).Execute(false, true, false, _dbConfig.GetConnection(), null);
        }

        public ISessionFactory CreateInMemorySqlLiteFactory()
        {
            _dbConfig = new SqlLiteConfig();
            var fact = GetFluentConfiguration().BuildSessionFactory();
            BuildSchema();
            var session = fact.OpenSession(_dbConfig.GetConnection());
            CurrentSessionContext.Bind(session);

            return fact;
        }

        public ISessionFactory CreateSessionFactory()
        {
            return GetFluentConfiguration().BuildSessionFactory();
        }

        public ISessionFactory CreateSessionFactory(string environment)
        {
            var connectString = GetConnectionString();
            connectString = connectString.Replace("\\\\", "\\");

            return GetFluentConfiguration(connectString).BuildSessionFactory();
        }


        private FluentConfiguration GetFluentConfiguration()
        {
            return Fluently.Configure()
                .Database(GetDatabase(string.Empty))
                .Mappings(x => x.FluentMappings.AddFromAssemblyOf<TMap>())
                .CurrentSessionContext<ThreadStaticSessionContext>()
                .ExposeConfiguration(x => x.SetProperty("connection.release_mode", "on_close"))
                .ExposeConfiguration(cfg => _configuration = cfg);
        }

        private FluentConfiguration GetFluentConfiguration(string connectString)
        {
            return Fluently.Configure()
                .Database(GetDatabase(connectString))
                .Mappings(x => x.FluentMappings.AddFromAssemblyOf<TMap>()
                                   .ExportTo(_exportPath))
                .CurrentSessionContext<ThreadStaticSessionContext>()
                .Diagnostics(x =>
                                 {
                                     x.Enable(true);
                                     x.OutputToFile(_logPath);
                                 })
                .ExposeConfiguration(cfg => _configuration = cfg);
        }

        private IPersistenceConfigurer GetDatabase(string connectString)
        {
            return _dbConfig.Configurer(connectString);
        }

        private string GetConnectionString()
        {
            return _connectionString;
        }
    }
}
