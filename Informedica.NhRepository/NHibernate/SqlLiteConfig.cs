using System.Data;
using System.Data.SQLite;
using FluentNHibernate.Cfg.Db;
using Informedica.GenForm.DataAccess.Databases;

namespace Informedica.NhRepository.NHibernate
{
    public class SqlLiteConfig: IDatabaseConfig
    {
        private static SQLiteConnection _connection;
        private const string ConnectionString = "Data Source=:memory:;Version=3;New=True;Pooling=True;Max Pool Size=1;";

        public IPersistenceConfigurer Configurer(string connectString)
        {
            return SQLiteConfiguration.Standard.ConnectionString(ConnectionString);
        }

        public IDbConnection GetConnection()
        {
            if(_connection == null || _connection.State != ConnectionState.Open )
            {
                _connection = new SQLiteConnection(ConnectionString);
                _connection.Open();
            }

            return _connection;
        }
    }
}
