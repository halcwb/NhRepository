using FluentNHibernate.Mapping;
using Informedica.DataAccess.Databases;
using Informedica.EntityRepository;

namespace Informedica.DataAccess.Tests
{
    static internal class RepositoryFixture
    {

        public static IRepository<TestEntity, int> CreateInMemorySqLiteRepository<TMap>()
            where TMap: ClassMap<TestEntity>
        {
            var fact = new SessionFactoryCreator<TMap>().CreateInMemorySqlLiteFactory();
            return new Repositories.Repository<TestEntity, int>(fact);
        }

        public static SessionFactoryCreator<TestMapping> CreateSqLiteSessionFactoryCreator()
        {
            var config = new SqlLiteConfig();
            var fact = new SessionFactoryCreator<TestMapping>(config);
            return fact;
        }
    }


}