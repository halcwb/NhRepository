using Informedica.DataAccess.Configurations;
using Informedica.EntityRepository;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TypeMock.ArrangeActAssert;

namespace Informedica.DataAccess.Tests
{
    [TestClass]
    public class AnEntityRepositoryShould: InMemorySqLiteTestBase
    {
        private static readonly EntityRepository.Testing.AnEntityRepositoryShould Tests = new EntityRepository.Testing.AnEntityRepositoryShould();
        private IRepository<TestEntity, int> _repos;
        private ConfigurationManager _confMan;
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

            _confMan = ConfigurationManager.Instance;
            Isolate.WhenCalled(() => _confMan.AddInMemorySqLiteEnvironment<TestMapping>("T")).CallOriginal();
            _repos = RepositoryFixture.CreateInMemorySqLiteRepository<TestMapping>("Test");
        }

        [TestCleanup]
        public void Cleanup()
        {
            _cache.Clear();
        }

        [Isolated]
        [TestMethod]
        public void ThrowAnErrorWhenInitiatedWithAnNullReference()
        {
            Tests.ThrowAnErrorWhenInitiatedWithAnNullReference(CreateRepositoryWithNullReference);
        }

        private static void CreateRepositoryWithNullReference()
        {
            new Repository<TestEntity, int>(null);
        }

        [Isolated]
        [TestMethod]
        public void HaveZeroItemsWhenFirstCreated()
        {
            Tests.HaveZeroItemsWhenFirstCreated(_repos);
        }

        [Isolated]
        [TestMethod]
        public void ThrowAnErrorWhenANullReferenceIsAdded()
        {
            Tests.ThrowAnErrorWhenANullReferenceIsAdded(_repos);
        }

        [Isolated]
        [TestMethod]
        public void HaveOneItemWhenAnEntityIsAdded()
        {
            var ent = EntityFixture.CreateEntityWithId(1);

            Tests.HaveOneItemWhenAnEntityIsAdded(_repos, ent);
        }

        [Isolated]
        [TestMethod]
        public void ReturnTheEntityThatWasAdded()
        {
            var ent = EntityFixture.CreateIntIdEntity();

            Tests.ReturnTheEntityThatWasAdded(_repos, ent);
        }

        [Isolated]
        [TestMethod]
        public void HaveTwoItemsWhenTwoEntitiesAreAdded()
        {
            var ent1 = EntityFixture.CreateEntityWithId(1);
            var ent2 = EntityFixture.CreateEntityWithId(2);

            ent1.Name = "Entity1";
            ent2.Name = "Entity2";

            Tests.HaveTwoItemsWhenTwoEntitiesAreAdded(_repos, ent1, ent2);
        }

        [Isolated]
        [TestMethod]
        public void NotAcceptTheSameEntityTwice()
        {
            var ent = EntityFixture.CreateEntityWithId(1);
                
            Tests.NotAcceptTheSameEntityTwice(_repos, ent);
        }

        [Isolated]
        [TestMethod]
        public void NotAcceptADifferentEntityWithTheSameId()
        {
            var ent1 = EntityFixture.CreateEntityWithId(1);
            var ent2 = EntityFixture.CreateEntityWithId(1);
            
            Tests.NotAcceptADifferentEntityWithTheSameId(_repos, ent1, ent2);
        }

        [Isolated]
        [TestMethod]
        public void ReturnAnEntityById()
        {
            var ent1 = EntityFixture.CreateEntityWithId(1);
            ent1.Name = "Entity1";
            var ent2 = EntityFixture.CreateEntityWithId(2);
            ent2.Name = "Entity2";
            
            Tests.ReturnAnEntityById(_repos, ent1, ent2);
        }

        [Isolated]
        [TestMethod]
        public void NotAcceptAnEntityWithTheSameIdentityTwice()
        {
            var ent1 = EntityFixture.CreateEntityWithId(1);
            var ent2 = EntityFixture.CreateEntityWithId(2);
            
            Tests.NotAcceptAnEntityWithTheSameIdentityTwice(_repos, ent1, ent2);
        }

        [Isolated]
        [TestMethod]
        public void RemoveTestEntity()
        {
            var ent1 = EntityFixture.CreateEntityWithId(1);
            
            Tests.RemoveTestEntity(_repos, ent1);
        }

        [Isolated]
        [TestMethod]
        public void RemoveTestEntityById()
        {
            var ent1 = EntityFixture.CreateEntityWithId(1);
            
            Tests.RemoveTestEntityById(_repos, ent1);
        }

        [Isolated]
        [TestMethod]
        public void ThrowAnErrorWhenTryingToRemoveNullReference()
        {            
            Tests.ThrowAnErrorWhenTryingToRemoveNullReference(_repos);
        }

        [Isolated]
        [TestMethod]
        public void ThrowAnErrorWhenTryingToRemoveNonAddedEntity()
        {
            var ent = EntityFixture.CreateIntIdEntity();
            
            Tests.ThrowAnErrorWhenTryingToRemoveNonAddedEntity(_repos, ent);
        }

    }
}
