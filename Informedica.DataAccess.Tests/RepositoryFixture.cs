using FluentNHibernate.Cfg;
using FluentNHibernate.Mapping;
using Informedica.DataAccess.Databases;
using Informedica.EntityRepository;
using NHibernate.Context;

namespace Informedica.DataAccess.Tests
{
    static internal class RepositoryFixture
    {

        public static IRepository<TestEntity, int> CreateInMemorySqLiteRepository<TMap>()
            where TMap: ClassMap<TestEntity>
        {
            var creator = CreateSqLiteSessionFactoryCreator<TMap>();
            var fact = creator.CreateSessionFactory();
            var session = fact.OpenSession();
            creator.BuildSchema(session);
            CurrentSessionContext.Bind(session);

            return new Repositories.Repository<TestEntity, int>(creator.CreateSessionFactory());
        }

        public static SessionFactoryCreator CreateSqLiteSessionFactoryCreator<TMap>()
        {
            var dbConfig = new SqlLiteConfig();
            var config = Fluently.Configure()
                .Mappings(x => x.FluentMappings.AddFromAssemblyOf<TMap>())
                .CurrentSessionContext<ThreadStaticSessionContext>()
                .ExposeConfiguration(x => x.SetProperty("connection.release_mode", "on_close"));

            return new SessionFactoryCreator(dbConfig, config);
        }

    }

}