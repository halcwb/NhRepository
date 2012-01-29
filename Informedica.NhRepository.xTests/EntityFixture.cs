using Informedica.EntityRepository.Entities;

namespace Informedica.DataAccess.Tests
{
    static internal class EntityFixture
    {
        public static TestEntity CreateIntIdEntity()
        {
            return CreateEntityWithId(1);
        }

        public static TestEntity CreateEntityWithId(int id)
        {
            var ent = new TestEntity(id);
            ent.Name = "TestEntity";
            return ent;
        }

    }

    public class TestEntity: IEntity<TestEntity, int>
    {
        public TestEntity(int id)
        {
            Id = id;
        }

        public string Name { get; set; }

        #region Overrides of Entity<TestEntity,int>

        public bool IsIdentical(TestEntity entity)
        {
            return Name == entity.Name;
        }

        public bool IsTransient()
        {
            return Id == default(int);
        }

        public int Id { get; private set; }

        #endregion
    }
}