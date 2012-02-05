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
        private const string TestEnv = "test";

        [Isolated]
        [TestMethod]
        public void BelongToAnEnvironment()
        {
            var env = GetEnvironmentConfiguration();

            Assert.AreEqual(TestEnv, env.EnvironmentName);
        }

        [Isolated]
        [TestMethod]
        public void BeAbleToReturnTheSessionFactoryForThatEnvironment()
        {
            var env = GetEnvironmentConfiguration();

            Assert.IsInstanceOfType(env.GetSessionFactory(), typeof(ISessionFactory));
        }

        [Isolated]
        [TestMethod]
        public void WillReturnTheSameSessionFactoryEachTime()
        {
            var fact = GetEnvironmentConfiguration().GetSessionFactory();

            Assert.AreEqual(fact, GetEnvironmentConfiguration().GetSessionFactory());
        }

        [Isolated]
        [TestMethod]
        public void BeAbleToBuildTheSchema()
        {
            var env = GetEnvironmentConfiguration();

            try
            {
                env.BuildSchema();
            }
            catch (Exception e)
            {
                Assert.Fail(e.ToString());
            }
        }

        private static EnvironmentConfiguration GetEnvironmentConfiguration()
        {
            var fakeDbConfig = Isolate.Fake.Instance<IDatabaseConfig>();
            var fakeConn = Isolate.Fake.Instance<IDbConnection>();
            Isolate.WhenCalled(() => fakeDbConfig.GetConnection()).WillReturn(fakeConn);

            var fakeConfig = Isolate.Fake.Instance<Configuration>();
            Isolate.WhenCalled(() => fakeConfig.BuildSessionFactory()).ReturnRecursiveFake();
            var env = new EnvironmentConfiguration(TestEnv, fakeConfig, fakeDbConfig);

            Isolate.WhenCalled(() => env.BuildSchema(fakeConn)).WithExactArguments().IgnoreCall();
            return env;
        }

    }
}
