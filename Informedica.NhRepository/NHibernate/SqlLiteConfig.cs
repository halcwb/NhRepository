using System.Data;
using System.Data.SQLite;
using FluentNHibernate.Cfg.Db;
using Informedica.GenForm.DataAccess.Databases;

namespace Informedica.NhRepository.NHibernate
{
    public class SqlLiteConfig: IDatabaseConfig
    {
        private static SQLiteConnection Connection;
        private const string ConnectionString = "Data Source=:memory:;Version=3;New=True;Pooling=True;Max Pool Size=1;";

        public IPersistenceConfigurer Configurer(string connectString)
        {
            return SQLiteConfiguration.Standard.InMemory().ConnectionString(ConnectionString);
        }

        public IDbConnection GetConnection()
        {
            if(Connection == null || Connection.State != ConnectionState.Open )
            {
                Connection = new System.Data.SQLite.SQLiteConnection(ConnectionString);
                Connection.Open();
            }

            return Connection;
        }
    }
}
