using System.Collections.Generic;
using System.Linq;
using Informedica.DataAccess.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NHibernate;
using NHibernate.Linq;
using TypeMock.ArrangeActAssert;

namespace Informedica.DataAccess.Tests
{
    [TestClass]
    public class RepositoryInstantiatedWithNHibernateTests
    {
        [TestMethod]
        public void ThatAnEntityCanBeAddedInRepositoryInstantiatedWithNHibernate()
        {
            var fact = Isolate.Fake.Instance<ISessionFactory>();
            Isolate.WhenCalled(() => fact.GetCurrentSession().Query<TestEntity>().GetEnumerator()).ReturnRecursiveFake();
            var repos = new Repository<TestEntity, int>(new TestNhRepos(Isolate.Fake.Instance<ISessionFactory>()));


            Assert.AreEqual(0, repos.Count());
        }
    }

    public class TestNhRepos: NHibernateRepository<TestEntity, int>
    {
        public TestNhRepos(ISessionFactory factory) : base(factory)
        {
        }

        #region Overrides of NHibernateRepository<TestEntity,int>

        public override void Add(TestEntity item)
        {
            base.Add(item, new EntityComparer());
        }

        #endregion
    }

    public class EntityComparer : IEqualityComparer<TestEntity>
    {
        #region Implementation of IEqualityComparer<in TestEntity>

        /// <summary>
        /// Determines whether the specified objects are equal.
        /// </summary>
        /// <returns>
        /// true if the specified objects are equal; otherwise, false.
        /// </returns>
        /// <param name="x">The first object of type <paramref name="T"/> to compare.</param><param name="y">The second object of type <paramref name="T"/> to compare.</param>
        public bool Equals(TestEntity x, TestEntity y)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Returns a hash code for the specified object.
        /// </summary>
        /// <returns>
        /// A hash code for the specified object.
        /// </returns>
        /// <param name="obj">The <see cref="T:System.Object"/> for which a hash code is to be returned.</param><exception cref="T:System.ArgumentNullException">The type of <paramref name="obj"/> is a reference type and <paramref name="obj"/> is null.</exception>
        public int GetHashCode(TestEntity obj)
        {
            throw new System.NotImplementedException();
        }

        #endregion
    }
}
