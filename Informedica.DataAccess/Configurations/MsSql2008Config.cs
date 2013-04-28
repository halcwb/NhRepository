using System.Data;
using FluentNHibernate.Cfg.Db;
using Informedica.DataAccess.Configurations;

namespace Informedica.DataAccess.Configurations
{
    public class MsSql2008Config : IDatabaseConfig
    {
        public IPersistenceConfigurer Configurer(string connectString)
        {
            return MsSqlConfiguration.MsSql2008.ShowSql().AdoNetBatchSize(5).MaxFetchDepth(2).ConnectionString(connectString);
        }

        public IDbConnection GetConnection()
        {
            throw new System.NotImplementedException();
        }

        public IPersistenceConfigurer Configurer()
        {
            throw new System.NotImplementedException();
        }
    }
}