using System.Data;
using Informedica.DataAccess.Configurations;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NHibernate;
using NHibernate.Context;
using StructureMap;
using TypeMock.ArrangeActAssert;

namespace Informedica.DataAccess.Tests
{
    [TestClass]
    public class TheFactoryMethodCreateInMemoryEntityRepositoryShould: InMemorySqLiteTestBase
    {
        private ConfigurationManager _confMan;
        private IEnvironmentConfiguration _envConfig;
        private ISessionFactory _factory;
        private ISession _session;
        private IDbConnection _connection;
        private static IConnectionCache _cache;

        [ClassInitialize]
        public static void MyClassInitialize(TestContext testContext)
        {
            _cache = new TestConnectionCache();
        }

        public override IConnectionCache Cache
        {
            get { return _cache; }
        }

        [TestInitialize]
        public void Init()
        {
            InitCache();

            _confMan = ConfigurationManager.Instance;
            Isolate.WhenCalled(() => _confMan.AddInMemorySqLiteEnvironment<TestMapping>("Test")).CallOriginal();
            RepositoryFixture.CreateInMemorySqLiteRepository<TestMapping>("Test");
        }

        [TestCleanup]
        public void CleanUp()
        {
            _cache.Clear();
        }

        [Isolated]
        [TestMethod]
        public void UseTheConfigurationManagerToCreateAnInMemorySqlEnvironment()
        {
            Isolate.Verify.WasCalledWithAnyArguments(() => _confMan.AddInMemorySqLiteEnvironment<TestMapping>("Test"));
        }

        [Isolated]
        [TestMethod]
        public void GetAnEnvironmentConfigurationFromTheConfigurationManager()
        {
            Isolate.Verify.WasCalledWithAnyArguments(() => _confMan.GetConfiguration("Test"));
        }


        [Isolated]
        [TestMethod]
        public void UseTheReturnedEnvironmentConfigToGetAConnection()
        {
            IsolateRepository();

            RepositoryFixture.CreateInMemorySqLiteRepository<TestMapping>("Test");
            Isolate.Verify.WasCalledWithAnyArguments(() => _envConfig.GetConnection());
        }

        [Isolated]
        [TestMethod]
        public void UseTheReturnedEnvironmentConfigToGetTheSessionFactory()
        {
            IsolateRepository();

            RepositoryFixture.CreateInMemorySqLiteRepository<TestMapping>("Test");
            Isolate.Verify.WasCalledWithAnyArguments(() => _envConfig.GetSessionFactory());
        }

        [Isolated]
        [TestMethod]
        public void PassTheConnectionToTheGetSessionMethodOfTheSessionFactory()
        {
            IsolateRepository();

            RepositoryFixture.CreateInMemorySqLiteRepository<TestMapping>("Test");
            Isolate.Verify.WasCalledWithExactArguments(() => _factory.OpenSession(_connection));
        }

        [Isolated]
        [TestMethod]
        public void BindTheSessionToTheCurrentSessionContext()
        {
            IsolateRepository();

            RepositoryFixture.CreateInMemorySqLiteRepository<TestMapping>("Test");
            Isolate.Verify.WasCalledWithAnyArguments(() => CurrentSessionContext.Bind(_session));
        }


        private void IsolateRepository()
        {
            ObjectFactory.EjectAllInstancesOf<IConnectionCache>();

            _envConfig = Isolate.Fake.Instance<IEnvironmentConfiguration>();
            Isolate.WhenCalled(() => _confMan.GetConfiguration("Test")).WillReturn(_envConfig);

            _connection = Isolate.Fake.Instance<IDbConnection>();
            Isolate.WhenCalled(() => _envConfig.GetConnection()).WillReturn(_connection);

            _factory = Isolate.Fake.Instance<ISessionFactory>();
            Isolate.WhenCalled(() => _envConfig.GetSessionFactory()).WillReturn(_factory);

            _session = Isolate.Fake.Instance<ISession>();
            Isolate.WhenCalled(() => _factory.OpenSession(_connection)).WillReturn(_session);
            Isolate.WhenCalled(() => _envConfig.BuildSchema(_session)).IgnoreCall();

            Isolate.WhenCalled(() => CurrentSessionContext.Bind(_session)).IgnoreCall();
        }
    }
}
