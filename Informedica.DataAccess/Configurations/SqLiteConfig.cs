using System.Data;
using System.Data.SQLite;
using FluentNHibernate.Cfg.Db;
using StructureMap;

namespace Informedica.DataAccess.Configurations
{
    public class SqLiteConfig: IDatabaseConfig
    {
        private string _connectionString;
        public static string InMemoryDbConnectionString = "Data Source=:memory:;Version=3;New=True;Pooling=True;Max Pool Size=1;";

        public SqLiteConfig()
        {
            _connectionString = InMemoryDbConnectionString;
        }

        public SqLiteConfig(string connectionString)
        {
            _connectionString = string.IsNullOrWhiteSpace(connectionString) ? InMemoryDbConnectionString : connectionString;
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
            if (!InMemoryDatabase()) return ReturnNewSqLiteConnection();

            var cache = TryGetCache();
            return cache == null ? ReturnNewSqLiteConnection() : ReturnConnectionFromCache(cache);
        }

        private bool InMemoryDatabase()
        {
            return _connectionString.Contains(":memory");
        }

        private IDbConnection ReturnConnectionFromCache(IConnectionCache cache)
        {
            if (cache.HasNoConnection)
            {
                cache.SetConnection(new SQLiteConnection(_connectionString));
            }

            if (cache.GetConnection().State == ConnectionState.Closed) cache.GetConnection().Open();
            return cache.GetConnection();
        }

        private IDbConnection ReturnNewSqLiteConnection()
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
