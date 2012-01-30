using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Informedica.NhRepository.xTests
{
    [TestClass]
    public class EntityTestsThat
    {
        [TestMethod]
        public void ThatEntityCanBeIdentifiedByAnId()
        {
            var repos = RepositoryFixture.CreateInMemorySqLiteRepository<TestMapping>();
            var ent = EntityFixture.CreateIntIdEntity();
            repos.Add(ent);

            Assert.AreEqual(ent, repos.First(e => e.Id.Equals(ent.Id)));
        }

        [TestMethod]
        public void ThatEntityCanBeIdentitiedByItsIdentiy()
        {
            var ent = EntityFixture.CreateEntityWithId(1);
            ent.Name = "TestEntity";
            var repos = RepositoryFixture.CreateInMemorySqLiteRepository<TestMapping>();
            repos.Add(ent);

            Assert.AreEqual(ent, repos.SingleOrDefault(e => e.IsIdentical(ent)));
        }
    }
}
