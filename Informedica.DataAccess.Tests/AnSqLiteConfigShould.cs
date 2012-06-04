using System.Data;
using System.Data.SQLite;
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
        public void UseAConnectionCacheToGetTheConnectionIfHasConnection()
        {
            Isolate.WhenCalled(() => _cache.HasNoConnection).WillReturn(false);

            Assert.AreEqual(_connection, _config.GetConnection());
            Isolate.Verify.WasCalledWithAnyArguments(() => _cache.GetConnection());
        }

        [Isolated]
        [TestMethod]
        public void SetANewConnectionWhenConnectionCacheIsEmptyAndInMemoryDatabase()
        {
            Isolate.CleanUp();
            _cache = Isolate.Fake.Instance<IConnectionCache>();

            _connection = Isolate.Fake.Instance<IDbConnection>();
            Isolate.WhenCalled(() => _cache.HasNoConnection).WillReturn(true);
            Isolate.WhenCalled(() => _cache.GetConnection()).WillReturn(_connection);
            Isolate.WhenCalled(() => _cache.SetConnection(_connection)).IgnoreCall();
            Isolate.WhenCalled(() => _connection.ConnectionString).WillReturn(SqLiteConfig.InMemoryDbConnectionString);
            InitCache();

            _config = new SqLiteConfig();
            _config.GetConnection();
            Isolate.Verify.WasCalledWithAnyArguments(() => _cache.SetConnection(_connection));
        }

        [Isolated]
        [TestMethod]
        public void NotSetANewConnectionWhenConnectionCacheIsEmptyAndNotInMemoryDatabase()
        {
            InitCache();
            var conn = Isolate.Fake.Instance<SQLiteConnection>();
            Isolate.Swap.NextInstance<SQLiteConnection>().With(conn);

            _config = new SqLiteConfig("Not an in memory connection string");
            _config.GetConnection();
            Isolate.Verify.WasNotCalled(() => _cache.SetConnection(_connection));
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
