using System.Data;
using FluentNHibernate.Cfg.Db;
using Informedica.DataAccess.Configurations;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TypeMock.ArrangeActAssert;

namespace Informedica.DataAccess.Tests
{
    [TestClass]
    public class AnSqLiteConfigShould
    {
        private SqLiteConfig _config;
        private IConnectionCache _cache;
        private IDbConnection _connection;

        [TestInitialize]
        public void Init()
        {
            _connection = Isolate.Fake.Instance<IDbConnection>();
            _cache = Isolate.Fake.Instance<IConnectionCache>();
            Isolate.WhenCalled(() => _cache.SetConnection(_connection)).IgnoreCall();
            Isolate.WhenCalled(() => _cache.GetConnection()).WillReturn(_connection);

            _config = new SqLiteConfig(_cache);
        }

        [TestMethod]
        public void ReturnANhiberNatePersistenceConfigurer()
        {
            Assert.IsInstanceOfType(_config.Configurer(), typeof(IPersistenceConfigurer));
        }

        [Isolated]
        [TestMethod]
        public void UseAConnectionCacheToGetTheConnection()
        {
            Assert.AreEqual(_connection, _config.GetConnection());
            Isolate.Verify.WasCalledWithAnyArguments(() => _cache.GetConnection());
        }

        [Isolated]
        [TestMethod]
        public void SetANewConnectionWhenConnectionCacheIsEmpty()
        {
            Isolate.CleanUp();
            _cache = Isolate.Fake.Instance<IConnectionCache>();
            _connection = Isolate.Fake.Instance<IDbConnection>();
            Isolate.WhenCalled(() => _cache.IsEmpty).WillReturn(true);
            Isolate.WhenCalled(() => _cache.GetConnection()).WillReturn(_connection);
            Isolate.WhenCalled(() => _cache.SetConnection(_connection)).IgnoreCall();

            _config = new SqLiteConfig(_cache);
            _config.GetConnection();
            Isolate.Verify.WasCalledWithAnyArguments(() => _cache.SetConnection(_connection));
        }

        [Isolated]
        [TestMethod]
        public void ReturnAConnectionThatIsOpen()
        {
            Isolate.WhenCalled(() => _connection.State).WillReturn(ConnectionState.Open);

            Assert.IsTrue(_config.GetConnection().State == ConnectionState.Open);
        }

        [Isolated]
        [TestMethod]
        public void OpenAConnectionThatIsClosed()
        {
            Isolate.WhenCalled(() => _connection.State).WillReturn(ConnectionState.Closed);

            _config.GetConnection();
            Isolate.Verify.WasCalledWithAnyArguments(() => _connection.Open());
        }
    }

}
