using System;
using Informedica.DataAccess.Databases;
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

    }

    public class TestMapping: EntityMap<TestEntity, int>
    {
        public TestMapping()
        {
            Map(x => x.Name);
        }
    }
}
