using System.Linq;
using Informedica.DataAccess.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Informedica.NHibernateRepository.Tests
{
    [TestClass]
    public class EntityTestsThat
    {
        [TestMethod]
        public void ThatEntityCanBeIdentifiedByAnId()
        {
            var repos = RepositoryFixture.CreateIntEntityRepository();
            var ent = EntityFixture.CreateIntIdEntity();
            repos.Add(ent);

            Assert.AreEqual(ent, repos.First(e => e.Id.Equals(ent.Id)));
        }

        [TestMethod]
        public void ThatEntityCanBeIdentitiedByItsIdentiy()
        {
            var ent = EntityFixture.CreateEntityWithId(1);
            ent.Name = "TestEntity";
            var repos = RepositoryFixture.CreateIntEntityRepository();
            repos.Add(ent);

            Assert.AreEqual(ent, repos.SingleOrDefault(e => e.IsIdentical(ent)));
        }
    }
}
