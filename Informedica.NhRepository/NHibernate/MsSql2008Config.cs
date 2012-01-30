using System.Data;
using FluentNHibernate.Cfg.Db;
using Informedica.GenForm.DataAccess.Databases;

namespace Informedica.NhRepository.NHibernate
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
    }
}