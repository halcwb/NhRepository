using System.Data;
using System.Data.SQLite;
using FluentNHibernate.Cfg.Db;

namespace Informedica.DataAccess.Configurations
{
    public class SqlLiteConfig: IDatabaseConfig
    {
        private static SQLiteConnection _connection;
        private static string _connectionString = "Data Source=:memory:;Version=3;New=True;Pooling=True;Max Pool Size=1;";

        public SqlLiteConfig() {}

        public SqlLiteConfig(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IPersistenceConfigurer Configurer(string connectString)
        {
            _connectionString = connectString;
            return SQLiteConfiguration.Standard.ConnectionString(connectString);
        }

        public IDbConnection GetConnection()
        {
            if(_connection == null || _connection.State != ConnectionState.Open )
            {
                _connection = new SQLiteConnection(_connectionString);
                _connection.Open();
            }

            return _connection;
        }

        public IPersistenceConfigurer Configurer()
        {
            return SQLiteConfiguration.Standard.ConnectionString(_connectionString);
        }
    }
}
