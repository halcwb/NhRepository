using System.Data;
using FluentNHibernate.Cfg.Db;

namespace Informedica.DataAccess.Configurations
{
    public interface IDatabaseConfig
    {
        IPersistenceConfigurer Configurer(string connectString);
        IDbConnection GetConnection();
        IPersistenceConfigurer Configurer();
    }
}