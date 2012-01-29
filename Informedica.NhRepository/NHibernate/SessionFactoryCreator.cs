using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using Informedica.GenForm.DataAccess.Databases;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Context;
using NHibernate.Tool.hbm2ddl;

namespace Informedica.NhRepository.NHibernate
{
    public class SessionFactoryCreator<TMap>
    {
        private Configuration _configuration;
        private string _exportPath;
        private string _logPath;
        private IDatabaseConfig _dbConfig;
        private string _connectionString;

        public SessionFactoryCreator(IDatabaseConfig config, string connectionString, string exportPath, string logPath)
        {
            _dbConfig = config;
            _exportPath = exportPath;
            _logPath = logPath;
            _connectionString = connectionString;
        }

        public ISessionFactory CreateSessionFactory()
        {
            return CreateSessionFactory("GenFormTest");
        }

        public void BuildSchema(ISession session)
        {
            // first drop the database to recreate a new one
            // new SchemaExport(config).Drop(false, true);
            // this NHibernate tool takes a configuration (with mapping info in)
            // and exports a database schema from it
            new SchemaExport(_configuration).Execute(false, true, false, session.Connection, null);
        }

        public ISessionFactory CreateSessionFactory(string environment)
        {
            var connectString = GetConnectionString();
            connectString = connectString.Replace("\\\\", "\\");

            return GetFluentConfiguration(connectString).BuildSessionFactory();
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
