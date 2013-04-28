using System;
using System.Linq;
using FluentNHibernate.Mapping;
using Informedica.DataAccess.Configurations;
using Informedica.DataAccess.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NHibernate;
using NHibernate.Context;

namespace Informedica.DataAccess.Tests
{
    [TestClass]
    public class ARepositoryThatInheritsFromRepositoryShould : InMemorySqLiteTestBase
    {
        private static IConnectionCache _cache;

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            _cache = new TestConnectionCache();
        }

        [TestCleanup]
        public void Cleanup()
        {
            _cache.Clear();
        }

        [TestInitialize]
        public void Init()
        {
            InitCache();
        }

        public override IConnectionCache Cache
        {
            get { return _cache; }
        }

        [TestMethod]
        public void ReturnTrueWhenContainsEntity()
        {
            var ent = new InheritedEntity();
            ConfigurationManager.Instance.AddInMemorySqLiteEnvironment<InheritedEntityMap>("Test");
            var conn = ConfigurationManager.Instance.GetConfiguration("Test").GetConnection();
            var fact = ConfigurationManager.Instance.GetConfiguration("Test").GetSessionFactory();
            var session = fact.OpenSession(conn);
            ConfigurationManager.Instance.GetConfiguration("Test").BuildSchema(session);
            CurrentSessionContext.Bind(session);

            var repos = new InheritedRepository(fact);

            repos.Add(ent);
            Assert.IsTrue(repos.Contains(ent));
        }
    }

    public class InheritedEntityMap: ClassMap<InheritedEntity>
    {
        public InheritedEntityMap()
        {
// ReSharper disable DoNotCallOverridableMethodsInConstructor
            Id(x => x.Id).GeneratedBy.GuidComb();
// ReSharper restore DoNotCallOverridableMethodsInConstructor
            Map(x => x.Name);
        }
    }

    public class InheritedEntity: Entity<InheritedEntity>
    {
        #region Implementation of IEntity<in InheritedEntity,out Guid>


        #endregion

        #region Overrides of Entity<InheritedEntity,Guid>

        public override bool IsIdentical(InheritedEntity entity)
        {
            throw new NotImplementedException();
        }

        #endregion
    }


    public class InheritedRepository: Repository<InheritedEntity, Guid>, IRepository<InheritedEntity>
    {
        public InheritedRepository(ISessionFactory factory) : base(factory)
        {
        }

        #region Implementation of IRepository<InheritedEntity>

        public InheritedEntity GetByName(string name)
        {
            return this.SingleOrDefault(e => e.Name == name);
        }

        #endregion
    }


    public abstract class Entity<TEnt>: EntityRepository.Entities.Entity<TEnt, Guid> where TEnt : Entity<TEnt>
    {
        public const int NameLength = 255;

        public new virtual  Guid Id { get; protected set; }

        public virtual string Name { get; set; }

        public virtual int Version  { get; protected set; }

    }

    public interface IRepository<TEnt> : EntityRepository.IRepository<TEnt, Guid> where TEnt : Entity<TEnt>
    {
        TEnt GetByName(string name);
    }

}
