using Informedica.DataAccess.Configurations;
using StructureMap;

namespace Informedica.DataAccess.Tests
{
    public abstract class InMemorySqLiteTestBase
    {

        public void InitCache()
        {
            ObjectFactory.Configure(x => x.For<IConnectionCache>().Use(Cache));
        }

        public abstract IConnectionCache Cache { get; }

    }
}