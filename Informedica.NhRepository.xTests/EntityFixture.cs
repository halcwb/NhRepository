using Informedica.EntityRepository.Entities;

namespace Informedica.NhRepository.xTests
{
    static internal class EntityFixture
    {
        public static TestEntity CreateIntIdEntity()
        {
            return CreateEntityWithId(1);
        }

        public static TestEntity CreateEntityWithId(int id)
        {
            var ent = new TestEntity(id) {Name = "TestEntity"};
            return ent;
        }

    }

    public class TestEntity: IEntity<TestEntity, int>
    {
        protected TestEntity() {}

        public TestEntity(int id)
        {
            Id = id;
        }

        public virtual string Name { get; set; }

        #region Overrides of Entity<TestEntity,int>

        public virtual bool IsIdentical(TestEntity entity)
        {
            return Name == entity.Name;
        }

        public virtual bool IsTransient()
        {
            return Id == default(int);
        }

        public virtual int Id { get; protected set; }

        #endregion
    }
}