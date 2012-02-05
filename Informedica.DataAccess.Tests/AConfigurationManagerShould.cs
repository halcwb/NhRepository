using System.Linq;
using Informedica.DataAccess.Configurations;
using Informedica.DataAccess.Databases;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NHibernate.Cfg;
using TypeMock.ArrangeActAssert;

namespace Informedica.DataAccess.Tests
{
    [TestClass]
    public class AConfigurationManagerShould
    {
        [TestMethod]
        public void BeAbleToAddAndRemoveAnConfiguration()
        {
            const string name = "test";
            ConfigurationManager.Instance.AddConfiguration(name, Isolate.Fake.Instance<Configuration>(), Isolate.Fake.Instance<IDatabaseConfig>());

            Assert.AreEqual(1, ConfigurationManager.Instance.Configurations.Count());

            ConfigurationManager.Instance.RemoveConfiguration(name);
            Assert.AreEqual(0, ConfigurationManager.Instance.Configurations.Count());
        }
    }
}
