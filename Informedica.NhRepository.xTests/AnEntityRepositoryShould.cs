using System;
using System.Linq;
using Informedica.DataAccess.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Informedica.NhRepository.xTests
{
    [TestClass]
    public class AnEntityRepositoryShould
    {
        [TestMethod]
        public void ThrowAnErrorWhenInitiatedWithAnNullReference()
        {
            try
            {
                new Repository<TestEntity, int>(null);
                Assert.Fail("Repository should not be created with empty list");
            }
            catch (Exception e)
            {
                Assert.IsNotInstanceOfType(e, typeof(AssertFailedException));
            }
        }

        [TestMethod]
        public void HaveZeroItemsWhenFirstCreated()
        {
            var repos = RepositoryFixture.CreateIntEntityRepository();
            Assert.AreEqual(0, repos.Count);
        }

        [TestMethod]
        public void ThrowAnErrorWhenANullReferenceIsAdded()
        {
            try
            {
                var repos = RepositoryFixture.CreateIntEntityRepository();
                repos.Add(null);
                Assert.Fail("Repository should throw an error when null is added");
            }
            catch (Exception e)
            {
                Assert.IsNotInstanceOfType(e, typeof(AssertFailedException));
            }
        }

        [TestMethod]
        public void HaveOneItemWhenAnEntityIsAdded()
        {
            var repos = RepositoryFixture.CreateIntEntityRepository();
            repos.Add(EntityFixture.CreateIntIdEntity());

            Assert.AreEqual(1, repos.Count);
        }

        [TestMethod]
        public void ReturnTheEntityThatWasAdded()
        {
            var repos = RepositoryFixture.CreateIntEntityRepository();
            var ent = EntityFixture.CreateIntIdEntity();
            repos.Add(ent);

            Assert.AreEqual(ent, repos.First());
        }

        [TestMethod]
        public void HaveTwoItemsWhenTwoEntitiesAreAdded()
        {
            var repos = RepositoryFixture.CreateIntEntityRepository();
            var ent1 = EntityFixture.CreateEntityWithId(1);
            ent1.Name = "Entity1";
            var ent2 = EntityFixture.CreateEntityWithId(2);
            ent2.Name = "Entity2";
            repos.Add(ent1);
            repos.Add(ent2);

            Assert.AreEqual(2, repos.Count());
        }

        [TestMethod]
        public void NotAcceptTheSameEntityTwice()
        {
            try
            {
                var repos = RepositoryFixture.CreateIntEntityRepository();
                var ent = EntityFixture.CreateIntIdEntity();

                repos.Add(ent);
                repos.Add(ent);
                Assert.Fail("Repository should not acccept the same entity twice");
            }
            catch (Exception e)
            {
                Assert.IsNotInstanceOfType(e, typeof(AssertFailedException));
            }
        }

        [TestMethod]
        public void NotAcceptADifferentEntityWithTheSameId()
        {
            var ent1 = EntityFixture.CreateEntityWithId(1);
            var ent2 = EntityFixture.CreateEntityWithId(1);
            var repos = RepositoryFixture.CreateIntEntityRepository();

            repos.Add(ent1);

            try
            {
                repos.Add(ent2);
                Assert.Fail("A different entity with the same id cannot be added");
            }
            catch (Exception e)
            {
                Assert.IsNotInstanceOfType(e, typeof(AssertFailedException));
            }
        }

        [TestMethod]
        public void ReturnAnEntityById()
        {
            var ent1 = EntityFixture.CreateEntityWithId(1);
            ent1.Name = "TestIdentity1";
            var ent2 = EntityFixture.CreateEntityWithId(2);
            ent2.Name = "TestIdentity2";
            var repos = RepositoryFixture.CreateIntEntityRepository();

            repos.Add(ent1);
            repos.Add(ent2);
            Assert.AreEqual(ent1, repos.Single(e => e.Id.Equals(ent1.Id)));
            Assert.AreEqual(ent2, repos.Single(e => e.Id.Equals(ent2.Id)));
        }

        [TestMethod]
        public void NotAcceptAnEntityWithTheSameIdentityTwice()
        {
            var ent1 = EntityFixture.CreateEntityWithId(1);
            ent1.Name = "Entity1";
            var ent2 = EntityFixture.CreateEntityWithId(2);
            ent2.Name = "Entity1";
            var repos = RepositoryFixture.CreateIntEntityRepository();

            repos.Add(ent1);
            try
            {
                repos.Add(ent2);
                Assert.Fail("An entity with the same identity should not be accepted twice");
            }
            catch (Exception e)
            {
                Assert.IsNotInstanceOfType(e, typeof(AssertFailedException));
            }
        }

        [TestMethod]
        public void RemoveTestEntity()
        {
            var ent1 = EntityFixture.CreateEntityWithId(1);
            ent1.Name = "Entity1";
            var repos = RepositoryFixture.CreateIntEntityRepository();

            repos.Add(ent1);
            Assert.AreEqual(1, repos.Count());

            repos.Remove(ent1);
            Assert.AreEqual(0, repos.Count());
        }

        [TestMethod]
        public void RemoveTestEntityById()
        {
            var id = 1;
            var ent1 = EntityFixture.CreateEntityWithId(id);
            ent1.Name = "TestEntity1";
            var repos = RepositoryFixture.CreateIntEntityRepository();

            repos.Add(ent1);
            repos.Remove(id);
            Assert.AreEqual(0, repos.Count());
        }

        [TestMethod]
        public void ThrowAnErrorWhenTryingToRemoveNullReference()
        {
            var repos = RepositoryFixture.CreateIntEntityRepository();

            try
            {
                repos.Remove(null);
                Assert.Fail("Repository can not remover null reference");
            }
            catch (Exception e)
            {
                Assert.IsNotInstanceOfType(e, typeof(AssertFailedException));
            }            
        }

        [TestMethod]
        public void ThrowAnErrorWhenTryingToRemoveNonAddedEntity()
        {
            var ent = EntityFixture.CreateIntIdEntity();
            var repos = RepositoryFixture.CreateIntEntityRepository();

            try
            {
                repos.Remove(ent);
                Assert.Fail("Repository can not remove an entity it does not contain");
            }
            catch (Exception e)
            {
                Assert.IsNotInstanceOfType(e, typeof(AssertFailedException));
            }
        }

    }
}
