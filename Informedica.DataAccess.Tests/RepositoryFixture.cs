using FluentNHibernate.Mapping;
using Informedica.EntityRepository;

namespace Informedica.DataAccess.Tests
{
    static internal class RepositoryFixture
    {

        public static IRepository<TestEntity, int> CreateInMemorySqLiteRepository<TMap>(string name)
            where TMap: ClassMap<TestEntity>
        {
            return Repositories.Repository<TestEntity, int>.CreateInMemorySqLiteRepository<TMap>(name);
        }

    }

}