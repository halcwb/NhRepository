using System.Data;
using FluentNHibernate.Cfg.Db;
using Informedica.DataAccess.Configurations;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StructureMap;
using TypeMock.ArrangeActAssert;

namespace Informedica.DataAccess.Tests
{
    [TestClass]
    public class AnSqLiteConfigShould : InMemorySqLiteTestBase
    {
        private SqLiteConfig _config;
        private IDbConnection _connection;
        private IConnectionCache _cache;


        public override IConnectionCache Cache
        {
            get { return _cache; }
        }

        [TestInitialize]
        public void Init()
        {
            _cache = Isolate.Fake.Instance<IConnectionCache>();
            _connection = Isolate.Fake.Instance<IDbConnection>();
            Isolate.WhenCalled(() => _cache.SetConnection(_connection)).IgnoreCall();
            Isolate.WhenCalled(() => _cache.GetConnection()).WillReturn(_connection);
            InitCache();

            _config = new SqLiteConfig(); 
        }

        [Isolated]
        [TestMethod]
        public void RemoveTheConnectionCacheFromObjectFactoryAfterReturningAConnection()
        {
            _config.GetConnection();
            Assert.IsNull(ObjectFactory.TryGetInstance<IConnectionCache>());
        }

        [Isolated]
        [TestMethod]
        public void ReturnANhiberNatePersistenceConfigurer()
        {
            Assert.IsInstanceOfType(_config.Configurer(), typeof(IPersistenceConfigurer));
        }

        [Isolated]
        [TestMethod]
        public void UseAConnectionCacheToGetTheConnection()
        {
            Isolate.WhenCalled(() => _cache.HasNoConnection).WillReturn(false);

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
            Isolate.WhenCalled(() => Cache.HasNoConnection).WillReturn(true);
            Isolate.WhenCalled(() => Cache.GetConnection()).WillReturn(_connection);
            Isolate.WhenCalled(() => Cache.SetConnection(_connection)).IgnoreCall();
            InitCache();

            _config = new SqLiteConfig();
            _config.GetConnection();
            Isolate.Verify.WasCalledWithAnyArguments(() => Cache.SetConnection(_connection));
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
