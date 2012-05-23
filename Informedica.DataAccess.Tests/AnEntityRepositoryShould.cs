using System.Data;
using Informedica.DataAccess.Configurations;
using Informedica.EntityRepository;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NHibernate;
using NHibernate.Context;
using TypeMock.ArrangeActAssert;

namespace Informedica.DataAccess.Tests
{
    [TestClass]
    public class AnEntityRepositoryShould
    {
        private static readonly EntityRepository.Testing.AnEntityRepositoryShould Tests = new EntityRepository.Testing.AnEntityRepositoryShould();
        private IRepository<TestEntity, int> _repos;
        private ConfigurationManager _confMan;
        private IEnvironmentConfiguration _envConfig;
        private ISessionFactory _factory;
        private ISession _session;
        private IDbConnection _connection;

        [TestInitialize]
        public void Init()
        {
            _confMan = ConfigurationManager.Instance;
            Isolate.WhenCalled(() => _confMan.AddInMemorySqLiteEnvironment<TestMapping>("T")).CallOriginal();
            _repos = RepositoryFixture.CreateInMemorySqLiteRepository<TestMapping>("Test");
        }

        [TestMethod]
        public void UseTheConfigurationManagerToCreateAnInMemorySqlEnvironment()
        {
            Isolate.Verify.WasCalledWithAnyArguments(() => _confMan.AddInMemorySqLiteEnvironment<TestMapping>("Test"));
        }

        [Isolated]
        [TestMethod]
        public void GetAnEnvironmentConfigurationFromTheConfigurationManager()
        {
            Isolate.Verify.WasCalledWithAnyArguments(() => _confMan.GetConfiguration("Test"));
        }


        [Isolated]
        [TestMethod]
        public void UseTheReturnedEnvironmentConfigToGetAConnection()
        {
            IsolateRepository();

            RepositoryFixture.CreateInMemorySqLiteRepository<TestMapping>("Test");
            Isolate.Verify.WasCalledWithAnyArguments(() => _envConfig.GetConnection());
        }

        [Isolated]
        [TestMethod]
        public void UseTheReturnedEnvironmentConfigToGetTheSessionFactory()
        {
            IsolateRepository();

            RepositoryFixture.CreateInMemorySqLiteRepository<TestMapping>("Test");
            Isolate.Verify.WasCalledWithAnyArguments(() => _envConfig.GetSessionFactory());
        }

        [Isolated]
        [TestMethod]
        public void PassTheConnectionToTheGetSessionMethodOfTheSessionFactory()
        {
            IsolateRepository();

            RepositoryFixture.CreateInMemorySqLiteRepository<TestMapping>("Test");
            Isolate.Verify.WasCalledWithExactArguments(() => _factory.OpenSession(_connection));
        }

        [Isolated]
        [TestMethod]
        public void BindTheSessionToTheCurrentSessionContext()
        {
            IsolateRepository();

            RepositoryFixture.CreateInMemorySqLiteRepository<TestMapping>("Test");
            Isolate.Verify.WasCalledWithAnyArguments(() => CurrentSessionContext.Bind(_session));
        }


        private void IsolateRepository()
        {
            _envConfig = Isolate.Fake.Instance<IEnvironmentConfiguration>();
            Isolate.WhenCalled(() => _confMan.GetConfiguration("Test")).WillReturn(_envConfig);

            _connection = Isolate.Fake.Instance<IDbConnection>();
            Isolate.WhenCalled(() => _envConfig.GetConnection()).WillReturn(_connection);
            
            _factory = Isolate.Fake.Instance<ISessionFactory>();
            Isolate.WhenCalled(() => _envConfig.GetSessionFactory()).WillReturn(_factory);

            _session = Isolate.Fake.Instance<ISession>();
            Isolate.WhenCalled(() => _factory.OpenSession(_connection)).WillReturn(_session);
            Isolate.WhenCalled(() => _envConfig.BuildSchema(_session)).IgnoreCall();

            Isolate.WhenCalled(() => CurrentSessionContext.Bind(_session)).IgnoreCall();
        }

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
            Tests.HaveZeroItemsWhenFirstCreated(_repos);
        }

        [TestMethod]
        public void ThrowAnErrorWhenANullReferenceIsAdded()
        {
            Tests.ThrowAnErrorWhenANullReferenceIsAdded(_repos);
        }

        [TestMethod]
        public void HaveOneItemWhenAnEntityIsAdded()
        {
            var ent = EntityFixture.CreateEntityWithId(1);

            Tests.HaveOneItemWhenAnEntityIsAdded(_repos, ent);
        }

        [TestMethod]
        public void ReturnTheEntityThatWasAdded()
        {
            var ent = EntityFixture.CreateIntIdEntity();

            Tests.ReturnTheEntityThatWasAdded(_repos, ent);
        }

        [TestMethod]
        public void HaveTwoItemsWhenTwoEntitiesAreAdded()
        {
            var ent1 = EntityFixture.CreateEntityWithId(1);
            var ent2 = EntityFixture.CreateEntityWithId(2);

            ent1.Name = "Entity1";
            ent2.Name = "Entity2";

            Tests.HaveTwoItemsWhenTwoEntitiesAreAdded(_repos, ent1, ent2);
        }

        [TestMethod]
        public void NotAcceptTheSameEntityTwice()
        {
            var ent = EntityFixture.CreateEntityWithId(1);
                
            Tests.NotAcceptTheSameEntityTwice(_repos, ent);
        }

        [TestMethod]
        public void NotAcceptADifferentEntityWithTheSameId()
        {
            var ent1 = EntityFixture.CreateEntityWithId(1);
            var ent2 = EntityFixture.CreateEntityWithId(1);
            
            Tests.NotAcceptADifferentEntityWithTheSameId(_repos, ent1, ent2);
        }

        [TestMethod]
        public void ReturnAnEntityById()
        {
            var ent1 = EntityFixture.CreateEntityWithId(1);
            ent1.Name = "Entity1";
            var ent2 = EntityFixture.CreateEntityWithId(2);
            ent2.Name = "Entity2";
            
            Tests.ReturnAnEntityById(_repos, ent1, ent2);
        }

        [TestMethod]
        public void NotAcceptAnEntityWithTheSameIdentityTwice()
        {
            var ent1 = EntityFixture.CreateEntityWithId(1);
            var ent2 = EntityFixture.CreateEntityWithId(2);
            
            Tests.NotAcceptAnEntityWithTheSameIdentityTwice(_repos, ent1, ent2);
        }

        [TestMethod]
        public void RemoveTestEntity()
        {
            var ent1 = EntityFixture.CreateEntityWithId(1);
            
            Tests.RemoveTestEntity(_repos, ent1);
        }

        [TestMethod]
        public void RemoveTestEntityById()
        {
            var ent1 = EntityFixture.CreateEntityWithId(1);
            
            Tests.RemoveTestEntityById(_repos, ent1);
        }

        [TestMethod]
        public void ThrowAnErrorWhenTryingToRemoveNullReference()
        {            
            Tests.ThrowAnErrorWhenTryingToRemoveNullReference(_repos);
        }

        [TestMethod]
        public void ThrowAnErrorWhenTryingToRemoveNonAddedEntity()
        {
            var ent = EntityFixture.CreateIntIdEntity();
            
            Tests.ThrowAnErrorWhenTryingToRemoveNonAddedEntity(_repos, ent);
        }

    }
}
