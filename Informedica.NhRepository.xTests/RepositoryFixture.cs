using FluentNHibernate.Mapping;
using Informedica.EntityRepository;
using Informedica.NhRepository.NHibernate;

namespace Informedica.NhRepository.xTests
{
    static internal class RepositoryFixture
    {

        public static IRepository<TestEntity, int> CreateInMemorySqLiteRepository<TMap>()
            where TMap: ClassMap<TestEntity>
        {
            var fact = new SessionFactoryCreator<TMap>().CreateInMemorySqlLiteFactory();
            return new Repository<TestEntity, int>(fact);
        }

        public static SessionFactoryCreator<TestMapping> CreateSqLiteSessionFactoryCreator()
        {
            var config = new SqlLiteConfig();
            var fact = new SessionFactoryCreator<TestMapping>(config);
            return fact;
        }
    }


}