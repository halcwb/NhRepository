using System.Linq;
using Informedica.DataAccess.Configurations;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Informedica.DataAccess.Tests
{
    [TestClass]
    public class EntityTestsThat: InMemorySqLiteTestBase
    {
        private static IConnectionCache _cache;

        [ClassInitialize]
        public static void MyClassInitialize(TestContext testContext)
        {
            _cache = new TestConnectionCache();
        }

        public override IConnectionCache Cache
        {
            get { return _cache; }
        }

        [TestInitialize]
        public void Init()
        {
            InitCache();
        }

        [TestCleanup]
        public void CleanUp()
        {
            _cache.Clear();
        }

        [TestMethod]
        public void ThatEntityCanBeIdentifiedByAnId()
        {
            var repos = RepositoryFixture.CreateInMemorySqLiteRepository<TestMapping>("test");
            var ent = EntityFixture.CreateIntIdEntity();
            repos.Add(ent);

            Assert.AreEqual(ent, repos.First(e => e.Id.Equals(ent.Id)));
        }

        [TestMethod]
        public void ThatEntityCanBeIdentitiedByItsIdentiy()
        {
            var ent = EntityFixture.CreateEntityWithId(1);
            ent.Name = "TestEntity";
            var repos = RepositoryFixture.CreateInMemorySqLiteRepository<TestMapping>("test");
            repos.Add(ent);

            Assert.AreEqual(ent, repos.SingleOrDefault(e => e.IsIdentical(ent)));
        }
    }
}
