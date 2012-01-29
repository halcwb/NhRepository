using Informedica.DataAccess.Entities;

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

    public class TestEntity: Entity<TestEntity, int>
    {

        public TestEntity(int id) : base(id) {}

        public string Name { get; set; }

        #region Overrides of Entity<TestEntity,int>

        public override bool IsIdentical(TestEntity entity)
        {
            return Name == entity.Name;
        }

        #endregion
    }
}