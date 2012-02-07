using System;
using System.Linq;
using FluentNHibernate.Mapping;
using Informedica.DataAccess.Configurations;
using Informedica.DataAccess.Mappings;
using Informedica.DataAccess.Repositories;
using Informedica.EntityRepository.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NHibernate;
using NHibernate.Context;

namespace Informedica.DataAccess.Tests
{
    [TestClass]
    public class ARepositoryThatInheritsFromRepositoryShould
    {
        [TestMethod]
        public void ReturnTrueWhenContainsEntity()
        {
            var ent = new InheritedEntity();
            ConfigurationManager.Instance.AddInMemorySqLiteEnvironment<InheritedEntityMap>("Test");
            var fact = ConfigurationManager.Instance.GetConfiguration("Test").GetSessionFactory();
            var session = fact.OpenSession();
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
            Id(x => x.Id).GeneratedBy.GuidComb();
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
