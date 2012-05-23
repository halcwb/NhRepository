using System;
using System.Data;
using Informedica.DataAccess.Configurations;
using Informedica.DataAccess.Databases;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NHibernate;
using NHibernate.Cfg;
using TypeMock.ArrangeActAssert;

namespace Informedica.DataAccess.Tests
{
    [TestClass]
    public class AnEnvironmentConfigurationShould
    {
        private EnvironmentConfiguration _envConfig;
        private IDatabaseConfig _fakeDbConfig;
        private const string TestEnv = "test";

        [TestInitialize]
        public void GetEnvironmentConfiguration()
        {
            _fakeDbConfig = Isolate.Fake.Instance<IDatabaseConfig>();
            var fakeConn = Isolate.Fake.Instance<IDbConnection>();
            Isolate.WhenCalled(() => _fakeDbConfig.GetConnection()).WillReturn(fakeConn);

            var fakeConfig = Isolate.Fake.Instance<Configuration>();
            Isolate.WhenCalled(() => fakeConfig.BuildSessionFactory()).ReturnRecursiveFake();
            _envConfig = new EnvironmentConfiguration(TestEnv, fakeConfig, _fakeDbConfig);

            Isolate.WhenCalled(() => _envConfig.BuildSchema(fakeConn)).WithExactArguments().IgnoreCall();
        }

        [Isolated]
        [TestMethod]
        public void UseTheDbConfigToGetAConnection()
        {
            _envConfig.GetConnection();

            Isolate.Verify.WasCalledWithAnyArguments(() => _fakeDbConfig.GetConnection());
        }


        [Isolated]
        [TestMethod]
        public void BelongToAnEnvironment()
        {
            Assert.AreEqual(TestEnv, _envConfig.EnvironmentName);
        }

        [Isolated]
        [TestMethod]
        public void BeAbleToReturnTheSessionFactoryForThatEnvironment()
        {
            Assert.IsInstanceOfType(_envConfig.GetSessionFactory(), typeof(ISessionFactory));
        }

        [Isolated]
        [TestMethod]
        public void WillReturnTheSameSessionFactoryEachTime()
        {
            var fact = _envConfig.GetSessionFactory();

            Assert.AreEqual(fact, _envConfig.GetSessionFactory());
        }

        [Isolated]
        [TestMethod]
        public void BeAbleToBuildTheSchema()
        {
            try
            {
                _envConfig.BuildSchema();
            }
            catch (Exception e)
            {
                Assert.Fail(e.ToString());
            }
        }


    }
}
