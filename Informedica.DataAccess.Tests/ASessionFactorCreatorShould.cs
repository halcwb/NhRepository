using Informedica.DataAccess.Mappings;

namespace Informedica.DataAccess.Tests
{
    public class TestMapping: EntityMap<TestEntity, int>
    {
        public TestMapping()
        {
            Map(x => x.Name);
        }
    }
}
