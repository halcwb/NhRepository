using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Informedica.DataAccess.Repositories;

namespace Informedica.DataAccess.Tests
{
    static internal class RepositoryFixture
    {
        public static Repository<TestEntity, int> CreateIntEntityRepository()
        {
            var fakeRepos = CreateFakeRepository();
            return new Repository<TestEntity, int>(fakeRepos);
        }

        private static IRepository<TestEntity, int> CreateFakeRepository()
        {
            return new FakeRepository();
        }
    }

    internal class FakeRepository : IRepository<TestEntity, int>
    {
        private IList<TestEntity> _entities = new List<TestEntity>();

        #region Implementation of IEnumerable

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
        /// </returns>
        /// <filterpriority>1</filterpriority>
        public IEnumerator<TestEntity> GetEnumerator()
        {
            return _entities.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        #region Implementation of IRepository<TestEntity,int>

        public TestEntity GetById(int id)
        {
            return _entities.SingleOrDefault(e => e.Id.Equals(id));
        }

        public bool Contains(TestEntity entity)
        {
            return _entities.Contains(entity);
        }

        public void Add(TestEntity entity)
        {
            _entities.Add(entity);
        }

        public void Remove(TestEntity entity)
        {
            _entities.Remove(entity);
        }

        public int Count
        {
            get { return _entities.Count; }
        }

        #endregion
    }
}