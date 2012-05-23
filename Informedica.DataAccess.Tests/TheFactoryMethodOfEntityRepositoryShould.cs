using System.Data;
using Informedica.DataAccess.Configurations;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NHibernate;
using NHibernate.Context;
using TypeMock.ArrangeActAssert;

namespace Informedica.DataAccess.Tests
{
    [TestClass]
    public class TheFactoryMethodOfEntityRepositoryShould
    {
        private ConfigurationManager _confMan;
        private IEnvironmentConfiguration _envConfig;
        private ISessionFactory _factory;
        private ISession _session;
        private IDbConnection _connection;
        [TestInitialize]
        public void Init()
        {
            _confMan = ConfigurationManager.Instance;
            Isolate.WhenCalled(() => _confMan.AddInMemorySqLiteEnvironment<TestMapping>("T")).CallOriginal();
            RepositoryFixture.CreateInMemorySqLiteRepository<TestMapping>("Test");
        }

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
