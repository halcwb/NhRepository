using Informedica.EntityRepository;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Informedica.DataAccess.Tests
{
    [TestClass]
    public class AnEntityRepositoryShould
    {
        private static readonly EntityRepository.Testing.AnEntityRepositoryShould Tests = new EntityRepository.Testing.AnEntityRepositoryShould();

        [TestMethod]
        public void ThrowAnErrorWhenInitiatedWithAnNullReference()
        {
            Tests.ThrowAnErrorWhenInitiatedWithAnNullReference(CreateRepositoryWithNullReference);
        }

        private static void CreateRepositoryWithNullReference()
        {
            new Repository<TestEntity, int>(null);
        }

        [TestMethod]
        public void HaveZeroItemsWhenFirstCreated()
        {
            var repos = GetRepository();
            Tests.HaveZeroItemsWhenFirstCreated(repos);
        }

        private static IRepository<TestEntity, int> GetRepository()
        {
            return RepositoryFixture.CreateInMemorySqLiteRepository<TestMapping>();
        }

        [TestMethod]
        public void ThrowAnErrorWhenANullReferenceIsAdded()
        {
            var repos = GetRepository();
            Tests.ThrowAnErrorWhenANullReferenceIsAdded(repos);
        }

        [TestMethod]
        public void HaveOneItemWhenAnEntityIsAdded()
        {
            var repos = GetRepository();
            var ent = EntityFixture.CreateEntityWithId(1);

            Tests.HaveOneItemWhenAnEntityIsAdded(repos, ent);
        }

        [TestMethod]
        public void ReturnTheEntityThatWasAdded()
        {
            var repos = GetRepository();
            var ent = EntityFixture.CreateIntIdEntity();

            Tests.ReturnTheEntityThatWasAdded(repos, ent);
        }

        [TestMethod]
        public void HaveTwoItemsWhenTwoEntitiesAreAdded()
        {
            var repos = GetRepository();
            var ent1 = EntityFixture.CreateEntityWithId(1);
            var ent2 = EntityFixture.CreateEntityWithId(2);

            ent1.Name = "Entity1";
            ent2.Name = "Entity2";

            Tests.HaveTwoItemsWhenTwoEntitiesAreAdded(repos, ent1, ent2);
        }

        [TestMethod]
        public void NotAcceptTheSameEntityTwice()
        {
            var repos = GetRepository();
            var ent = EntityFixture.CreateEntityWithId(1);
                
            Tests.NotAcceptTheSameEntityTwice(repos, ent);
        }

        [TestMethod]
        public void NotAcceptADifferentEntityWithTheSameId()
        {
            var ent1 = EntityFixture.CreateEntityWithId(1);
            var ent2 = EntityFixture.CreateEntityWithId(1);
            var repos = GetRepository();
            
            Tests.NotAcceptADifferentEntityWithTheSameId(repos, ent1, ent2);
        }

        [TestMethod]
        public void ReturnAnEntityById()
        {
            var ent1 = EntityFixture.CreateEntityWithId(1);
            ent1.Name = "Entity1";
            var ent2 = EntityFixture.CreateEntityWithId(2);
            ent2.Name = "Entity2";
            var repos = GetRepository();
            
            Tests.ReturnAnEntityById(repos, ent1, ent2);
        }

        [TestMethod]
        public void NotAcceptAnEntityWithTheSameIdentityTwice()
        {
            var ent1 = EntityFixture.CreateEntityWithId(1);
            var ent2 = EntityFixture.CreateEntityWithId(2);
            var repos = GetRepository();
            
            Tests.NotAcceptAnEntityWithTheSameIdentityTwice(repos, ent1, ent2);
        }

        [TestMethod]
        public void RemoveTestEntity()
        {
            var ent1 = EntityFixture.CreateEntityWithId(1);
            var repos = GetRepository();
            
            Tests.RemoveTestEntity(repos, ent1);
        }

        [TestMethod]
        public void RemoveTestEntityById()
        {
            var ent1 = EntityFixture.CreateEntityWithId(1);
            var repos = GetRepository();
            
            Tests.RemoveTestEntityById(repos, ent1);
        }

        [TestMethod]
        public void ThrowAnErrorWhenTryingToRemoveNullReference()
        {
            var repos = GetRepository();
            
            Tests.ThrowAnErrorWhenTryingToRemoveNullReference(repos);
        }

        [TestMethod]
        public void ThrowAnErrorWhenTryingToRemoveNonAddedEntity()
        {
            var ent = EntityFixture.CreateIntIdEntity();
            var repos = GetRepository();
            
            Tests.ThrowAnErrorWhenTryingToRemoveNonAddedEntity(repos, ent);
        }

    }
}
