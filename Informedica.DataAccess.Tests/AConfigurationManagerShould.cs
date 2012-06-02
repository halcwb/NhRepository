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
            const string name = "testing";
            var start = ConfigurationManager.Instance.Configurations.Count();
            ConfigurationManager.Instance.AddConfiguration(name, Isolate.Fake.Instance<Configuration>(), Isolate.Fake.Instance<IDatabaseConfig>());

            Assert.AreEqual(start + 1, ConfigurationManager.Instance.Configurations.Count());

            ConfigurationManager.Instance.RemoveConfiguration(name);
            Assert.AreEqual(start, ConfigurationManager.Instance.Configurations.Count());
        }
    }
}
