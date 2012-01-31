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
            return new TestEntity(id) {Name = "TestEntity"};
        }

    }

    public class TestEntity: EntityRepository.Testing.TestEntity
    {
        protected TestEntity() {}

        public TestEntity(int id)
        {
            Id = id;
        }

        public virtual string Name
        {
            get { return base.Name; }
            set { base.Name = value; }
        }
        
    }
}