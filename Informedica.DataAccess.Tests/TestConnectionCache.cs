using System.Data;
using Informedica.DataAccess.Configurations;

namespace Informedica.DataAccess.Tests
{
    public class TestConnectionCache : IConnectionCache
    {
        private IDbConnection _connection;

        #region Implementation of IConnectionCache

        public IDbConnection GetConnection()
        {
            return _connection;
        }

        public void SetConnection(IDbConnection connection)
        {
            _connection = connection;
        }

        public bool HasNoConnection
        {
            get { return _connection == null; }
        }

        public void Clear()
        {
            _connection.Close();
            _connection = null;
        }

        #endregion
    }
}