using System.Data;
using FluentNHibernate.Cfg.Db;

namespace Informedica.DataAccess.Databases
{
    public interface IDatabaseConfig
    {
        IPersistenceConfigurer Configurer(string connectString);
        IDbConnection GetConnection();
    }
}