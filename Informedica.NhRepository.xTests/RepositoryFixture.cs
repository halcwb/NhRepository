using System.Collections.Generic;
using System.Linq;
using Informedica.NhRepository.NHibernate;
using NHibernate;
using NHibernate.Linq;
using TypeMock.ArrangeActAssert;

namespace Informedica.NhRepository.xTests
{
    static internal class RepositoryFixture
    {
        public static Repository<TestEntity, int> CreateIntEntityRepository()
        {
            return CreateIntEntityRepository(0);
        }

        public static Repository<TestEntity, int> CreateIntEntityRepository(int itemCount)
        {
            var fakeFactory = IsolateSessionFactoryWithCount(itemCount);

            return new Repository<TestEntity, int>(fakeFactory);
        }

        private static ISessionFactory IsolateSessionFactoryWithCount(int count)
        {
            var fakeFactory = Isolate.Fake.Instance<ISessionFactory>();
            Isolate.WhenCalled(() => fakeFactory.GetCurrentSession()).ReturnRecursiveFake();
            Isolate.WhenCalled(() => fakeFactory.GetCurrentSession().Query<TestEntity>()).WillReturn(
                new EnumerableQuery<TestEntity>(GetTestList()));
            Isolate.WhenCalled(() => fakeFactory.GetCurrentSession().Query<TestEntity>().GetEnumerator()).ReturnRecursiveFake();

            Isolate.WhenCalled(() => fakeFactory.GetCurrentSession().Query<TestEntity>().Count()).WillReturn(count);
            return fakeFactory;
        }

        public static Repository<TestEntity, int> CreateInMemorySqLiteRepository<TMap>()
        {
            var fact = new SessionFactoryCreator<TMap>().CreateInMemorySqlLiteFactory();
            return new Repository<TestEntity, int>(fact);
        }

        private static IEnumerable<TestEntity> GetTestList()
        {
            return new List<TestEntity>();
        }

        public static SessionFactoryCreator<TestMapping> CreateSqLiteSessionFactoryCreator()
        {
            var config = new SqlLiteConfig();
            var fact = new SessionFactoryCreator<TestMapping>(config);
            return fact;
        }
    }


}