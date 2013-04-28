using System.Data;
using NHibernate;

namespace Informedica.DataAccess.Configurations
{
    public interface IEnvironmentConfiguration
    {
        string EnvironmentName { get; }
        ISessionFactory GetSessionFactory();
        void BuildSchema();
        void BuildSchema(ISession session);
        void BuildSchema(IDbConnection connection);
        IDbConnection GetConnection();
    }
}