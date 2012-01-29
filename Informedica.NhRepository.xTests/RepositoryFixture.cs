using System.Collections.Generic;
using System.Linq;
using Informedica.DataAccess.Tests;
using NHibernate;
using NHibernate.Linq;
using TypeMock.ArrangeActAssert;

namespace Informedica.NhRepository.xTests
{
    static internal class RepositoryFixture
    {
        public static Repository<TestEntity, int> CreateIntEntityRepository()
        {
            var fakeFactory = Isolate.Fake.Instance<ISessionFactory>();
            Isolate.WhenCalled(() => fakeFactory.GetCurrentSession()).ReturnRecursiveFake();
            Isolate.WhenCalled(() => fakeFactory.GetCurrentSession().Query<TestEntity>()).WillReturn(new EnumerableQuery<TestEntity>(GetTestList()));
            Isolate.WhenCalled(() => fakeFactory.GetCurrentSession().Query<TestEntity>().GetEnumerator()).ReturnRecursiveFake();

            Isolate.WhenCalled(() => fakeFactory.GetCurrentSession().Query<TestEntity>().Count()).WillReturn(0);

            return new Repository<TestEntity, int>(fakeFactory);
        }

        private static IEnumerable<TestEntity> GetTestList()
        {
            return new List<TestEntity>();
        }
    }


}