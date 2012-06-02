using System.Data;

namespace Informedica.DataAccess.Configurations
{
    public interface IConnectionCache
    {
        IDbConnection GetConnection();
        void SetConnection(IDbConnection connection);
        bool HasNoConnection { get; }
        void Clear();
    }
}