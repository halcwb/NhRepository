using System.Data;
using System.Data.SQLite;
using FluentNHibernate.Cfg.Db;

namespace Informedica.GenForm.DataAccess.Databases
{
    public interface IDatabaseConfig
    {
        IPersistenceConfigurer Configurer(string connectString);
        IDbConnection GetConnection();
    }
}