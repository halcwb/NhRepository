using FluentNHibernate.Mapping;
using Informedica.DataAccess.Configurations;
using Informedica.EntityRepository;

namespace Informedica.DataAccess.Tests
{
    static internal class RepositoryFixture
    {

        public static IRepository<TestEntity, int> CreateInMemorySqLiteRepository<TMap>()
            where TMap: ClassMap<TestEntity>
        {
            return Repositories.Repository<TestEntity, int>.CreateInMemorySqLiteRepository<TMap>();
        }

        public static ConfigurationManager CreateSqLiteSessionFactoryCreator<TMap>()
        {
            return ConfigurationManager.CreateInMemorySqlLiteFactoryCreator<TMap>();

        }

    }

}