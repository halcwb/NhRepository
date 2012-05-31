using System.Data;
using System.Data.SQLite;
using FluentNHibernate.Cfg.Db;
using Informedica.DataAccess.Databases;

namespace Informedica.DataAccess.Configurations
{
    public class SqLiteConfig: IDatabaseConfig
    {
        private static string _connectionString = "Data Source=:memory:;Version=3;New=True;Pooling=True;Max Pool Size=1;";
        private IConnectionCache _cache;

        public SqLiteConfig() {}

        public SqLiteConfig(string connectionString)
        {
            _connectionString = connectionString;
        }

        public SqLiteConfig(IConnectionCache cache)
        {
            _cache = cache;
        }

        public IPersistenceConfigurer Configurer(string connectString)
        {
            _connectionString = connectString;
            return SQLiteConfiguration.Standard.ConnectionString(connectString);
        }

        public IDbConnection GetConnection()
        {
            if(_cache.IsEmpty)
            {
                _cache.SetConnection(new SQLiteConnection(_connectionString));
            }

            if (_cache.GetConnection().State == ConnectionState.Closed) _cache.GetConnection().Open();
            return _cache.GetConnection();
        }

        public IPersistenceConfigurer Configurer()
        {
            return SQLiteConfiguration.Standard.ConnectionString(_connectionString);
        }
    }
}
