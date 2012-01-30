using System;
using Informedica.NhRepository.NHibernate;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Informedica.NhRepository.xTests
{
    [TestClass]
    public class ASessionFactorCreatorShould
    {
        [TestMethod]
        public void BeAbleToCreateAnInMemorySqLiteSessionFactory()
        {
            var fact = RepositoryFixture.CreateSqLiteSessionFactoryCreator();

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
        public void BeAbleToCreateAnInMemorySqLiteFactory()
        {

            try
            {
                new SessionFactoryCreator<TestMapping>().CreateInMemorySqlLiteFactory();
            }
            catch (Exception e)
            {
                Assert.Fail(e.ToString());
            }
        }
    }

    public class TestMapping: EntityMap<TestEntity, int>
    {
        
    }
}
