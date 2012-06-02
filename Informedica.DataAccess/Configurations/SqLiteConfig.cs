using System.Data;
using System.Data.SQLite;
using FluentNHibernate.Cfg.Db;
using Informedica.DataAccess.Databases;
using StructureMap;

namespace Informedica.DataAccess.Configurations
{
    public class SqLiteConfig: IDatabaseConfig
    {
        private static string _connectionString = "Data Source=:memory:;Version=3;New=True;Pooling=True;Max Pool Size=1;";

        public SqLiteConfig() {}

        public SqLiteConfig(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IPersistenceConfigurer Configurer(string connectString)
        {
            _connectionString = connectString;
            return SQLiteConfiguration.Standard.ConnectionString(connectString);
        }

        private static IConnectionCache TryGetCache()
        {
            var cache =  ObjectFactory.TryGetInstance<IConnectionCache>();
            if (cache != null) ObjectFactory.EjectAllInstancesOf<IConnectionCache>();
            return cache;
        }

        public IDbConnection GetConnection()
        {
            var cache = TryGetCache();
            return cache == null ? ReturnNewSqLiteConnection() : ReturnConnectionFromCache(cache);
        }

        private static IDbConnection ReturnConnectionFromCache(IConnectionCache cache)
        {
            if (cache.HasNoConnection)
            {
                cache.SetConnection(new SQLiteConnection(_connectionString));
            }

            if (cache.GetConnection().State == ConnectionState.Closed) cache.GetConnection().Open();
            return cache.GetConnection();
        }

        private static IDbConnection ReturnNewSqLiteConnection()
        {
            var conn = new SQLiteConnection(_connectionString);
            conn.Open();
            return conn;
        }

        public IPersistenceConfigurer Configurer()
        {
            return SQLiteConfiguration.Standard.ConnectionString(_connectionString);
        }
    }
}
