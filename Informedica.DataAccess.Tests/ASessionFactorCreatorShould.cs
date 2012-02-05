using System;
using Informedica.DataAccess.Configurations;
using Informedica.DataAccess.Mappings;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Informedica.DataAccess.Tests
{
    [TestClass]
    public class ASessionFactorCreatorShould
    {
        [TestMethod]
        public void BeAbleToCreateAnInMemorySqLiteSessionFactory()
        {
            var fact = RepositoryFixture.CreateSqLiteSessionFactoryCreator<TestMapping>();

            try
            {
                fact.CreateSessionFactory();
            }
            catch (Exception e)
            {
                Assert.Fail(e.ToString());
            }
        }

        [TestMethod]
        public void BeAbleToCreateASqLiteSessionFactoryWithConnectionString()
        {
            try
            {
                var fact = ConfigurationManager.CreatSqLiteFactory<TestMapping>("Data Source=:memory:;Version=3;New=True;Pooling=True;Max Pool Size=1;");
                fact.BuildSchema();
                fact.CreateSessionFactory();
            }
            catch (Exception e)
            {
                Assert.Fail(e.ToString());
            }

        }

    }

    public class TestMapping: EntityMap<TestEntity, int>
    {
        public TestMapping()
        {
            Map(x => x.Name);
        }
    }
}
